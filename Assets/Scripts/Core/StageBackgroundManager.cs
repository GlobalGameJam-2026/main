using UnityEngine;
using UnityEngine.SceneManagement;

namespace ThawTheMask
{
    public class StageBackgroundManager : MonoBehaviour
    {
        public static StageBackgroundManager Instance { get; private set; }

        // 현재 스테이지용 스프라이트 저장 변수
        private Sprite currentWhiteSprite;
        private Sprite currentBlackSprite;

        [Header("Display Settings")]
        [SerializeField] private string sortingLayerName = "Background";
        [SerializeField] private int sortingOrder = -100;
        [SerializeField] private float backgroundZPosition = 10f;

        [Header("Scale Settings")]
        [SerializeField] private bool autoScale = true;
        [SerializeField] private Vector3 manualScale = new Vector3(20f, 20f, 1f);

        [Header("Parallax Settings")]
        [SerializeField] private bool enableParallax = true;
        [Range(0f, 1f)]
        [SerializeField] private float parallaxSpeed = 0.3f;
        [SerializeField] private float parallaxSizeMultiplier = 2.5f;

        [Header("Background Size Settings")]
        [SerializeField] private float verticalScaleMultiplier = 1.5f;
        [SerializeField] private bool enableHorizontalTiling = true;
        [SerializeField] private int horizontalTileCount = 1;

        private GameObject backgroundObject;
        private GameObject backgroundParent;
        private Camera mainCamera;
        private Vector3 lastCameraPosition;

        private MaskType? currentCachedMaskType = null;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // 씬 로드 이벤트는 "스테이지가 아닌 씬"에 갔을 때 배경 지우는 용도로만 사용
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        /// <summary>
        /// [핵심] 외부(StageSettings)에서 호출하여 배경을 설정하고 갱신하는 함수
        /// </summary>
        public void SetStageSprites(Sprite white, Sprite black)
        {
            currentWhiteSprite = white;
            currentBlackSprite = black;

            // 카메라 갱신
            mainCamera = Camera.main;
            if (mainCamera == null) mainCamera = FindObjectOfType<Camera>();

            // 마스크 상태 캐시 초기화 (강제 갱신 유도)
            currentCachedMaskType = null;

            Debug.Log($"[StageBackgroundManager] 배경 스프라이트 교체됨 -> 갱신 시작");

            // 배경 즉시 생성
            CreateBackground();
        }

        private void Update()
        {
            if (MaskManager.Instance == null) return;

            MaskType newMaskType = MaskManager.Instance.CurrentMask;

            // 마스크 상태가 바뀌었거나, 배경 오브젝트가 사라졌으면 갱신
            if (currentCachedMaskType != newMaskType || backgroundParent == null)
            {
                currentCachedMaskType = newMaskType;
                CreateBackground();
            }
        }

        private void LateUpdate()
        {
            if (backgroundParent != null && mainCamera != null && enableParallax)
            {
                UpdateParallaxPosition();
            }
        }

        private void UpdateParallaxPosition()
        {
            Vector3 currentCameraPosition = mainCamera.transform.position;

            if (Vector3.Distance(currentCameraPosition, lastCameraPosition) > 50f)
            {
                AlignBackgroundToCamera();
                lastCameraPosition = currentCameraPosition;
                return;
            }

            Vector3 deltaMovement = currentCameraPosition - lastCameraPosition;
            Vector3 parallaxMovement = new Vector3(deltaMovement.x * parallaxSpeed, deltaMovement.y * parallaxSpeed, 0f);

            backgroundParent.transform.position += parallaxMovement;
            lastCameraPosition = currentCameraPosition;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // 스테이지 씬이 아니라면 배경을 제거 (예: 메인 메뉴)
            if (!scene.name.Contains("Stage"))
            {
                DestroyBackground();
            }
        }

        private void CreateBackground()
        {
            DestroyBackground();

            if (MaskManager.Instance == null) return;

            // 현재 마스크에 맞는 스프라이트 선택
            MaskType currentType = MaskManager.Instance.CurrentMask;
            currentCachedMaskType = currentType; // 캐시 갱신

            Sprite targetSprite = (currentType == MaskType.White) ? currentWhiteSprite : currentBlackSprite;

            if (targetSprite == null)
            {
                Debug.LogWarning("[StageBackgroundManager] 설정된 스프라이트가 없습니다. (StageSettings 확인 필요)");
                return;
            }

            // 부모 생성 및 정렬
            backgroundParent = new GameObject("StageBackgroundParent");
            AlignBackgroundToCamera();

            // 스케일 계산
            Vector3 baseScale = autoScale ? CalculateBackgroundScale(targetSprite) : manualScale;
            if (enableParallax) baseScale *= parallaxSizeMultiplier;
            baseScale.y *= verticalScaleMultiplier;

            float spriteWorldWidth = targetSprite.bounds.size.x * baseScale.x;

            // 타일 생성
            backgroundObject = CreateBackgroundTile(targetSprite, Vector3.zero, baseScale, "Center");
            backgroundObject.transform.SetParent(backgroundParent.transform, false);

            if (enableHorizontalTiling)
            {
                for (int i = 1; i <= horizontalTileCount; i++)
                {
                    CreateBackgroundTile(targetSprite, new Vector3(-spriteWorldWidth * i, 0f, 0f), baseScale, $"Left_{i}")
                        .transform.SetParent(backgroundParent.transform, false);

                    CreateBackgroundTile(targetSprite, new Vector3(spriteWorldWidth * i, 0f, 0f), baseScale, $"Right_{i}")
                        .transform.SetParent(backgroundParent.transform, false);
                }
            }
        }

        private void AlignBackgroundToCamera()
        {
            if (mainCamera == null) mainCamera = Camera.main;
            if (mainCamera == null || backgroundParent == null) return;

            Vector3 camPos = mainCamera.transform.position;
            backgroundParent.transform.position = new Vector3(camPos.x, camPos.y, backgroundZPosition);
            lastCameraPosition = camPos;
        }

        private GameObject CreateBackgroundTile(Sprite sprite, Vector3 localPosition, Vector3 scale, string name)
        {
            GameObject tile = new GameObject($"StageBackground_{name}");
            SpriteRenderer sr = tile.AddComponent<SpriteRenderer>();
            sr.sprite = sprite;
            sr.sortingLayerName = sortingLayerName;
            sr.sortingOrder = sortingOrder;
            tile.transform.localPosition = localPosition;
            tile.transform.localScale = scale;
            return tile;
        }

        private Vector3 CalculateBackgroundScale(Sprite sprite)
        {
            if (mainCamera == null) return Vector3.one * 20f;
            float camHeight = mainCamera.orthographicSize * 2f;
            float camWidth = camHeight * mainCamera.aspect;
            float scale = Mathf.Max(camWidth / sprite.bounds.size.x, camHeight / sprite.bounds.size.y);
            return new Vector3(scale, scale, 1f);
        }

        private void DestroyBackground()
        {
            if (backgroundParent != null) Destroy(backgroundParent);
            if (backgroundObject != null) Destroy(backgroundObject);
        }
    }
}
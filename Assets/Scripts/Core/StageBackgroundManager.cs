using UnityEngine;
using UnityEngine.SceneManagement;

namespace ThawTheMask
{
    /// <summary>
    /// Automatically creates and manages background sprites for all stages.
    /// This script loads a background image from Resources and applies it to the current stage.
    /// Avoids Git sync issues by creating backgrounds at runtime instead of in scene files.
    /// </summary>
    public class StageBackgroundManager : MonoBehaviour
    {
        public static StageBackgroundManager Instance { get; private set; }

        [Header("Background Settings")]
        [Tooltip("배경 이미지 파일 이름 (Resources 폴더 내, 확장자 제외)")]
        [SerializeField] private string backgroundImageName = "StageBackground";

        [Header("Display Settings")]
        [Tooltip("배경 Sorting Layer 이름")]
        [SerializeField] private string sortingLayerName = "Background";
        
        [Tooltip("배경 Sorting Order (음수로 설정하여 뒤에 표시)")]
        [SerializeField] private int sortingOrder = -100;

        [Tooltip("배경 Z 위치 (카메라보다 뒤에)")]
        [SerializeField] private float backgroundZPosition = 10f;

        [Header("Scale Settings")]
        [Tooltip("화면에 맞춰 자동으로 스케일 조정")]
        [SerializeField] private bool autoScale = true;

        [Tooltip("수동 스케일 (autoScale이 false일 때 사용)")]
        [SerializeField] private Vector3 manualScale = new Vector3(20f, 20f, 1f);


        [Header("Parallax Settings")]
        [Tooltip("Parallax 효과 활성화 (배경이 카메라보다 천천히 움직임)")]
        [SerializeField] private bool enableParallax = true;

        [Tooltip("Parallax 속도 (0 = 고정, 1 = 카메라와 같이 움직임, 0.5 = 절반 속도)")]
        [Range(0f, 1f)]
        [SerializeField] private float parallaxSpeed = 0.3f;

        [Tooltip("배경 크기 배율 (Parallax 사용 시 배경을 더 크게)")]
        [SerializeField] private float parallaxSizeMultiplier = 2.5f;

        [Header("Background Size Settings")]
        [Tooltip("세로 크기 추가 배율 (위아래로 더 늘림)")]
        [SerializeField] private float verticalScaleMultiplier = 1.5f;

        [Tooltip("가로 타일링 활성화 (양옆에 이미지 반복)")]
        [SerializeField] private bool enableHorizontalTiling = true;

        [Tooltip("양옆에 추가할 타일 개수 (1 = 좌우 각 1개씩)")]
        [SerializeField] private int horizontalTileCount = 1;

        private GameObject backgroundObject;
        private GameObject backgroundParent;
        private Camera mainCamera;
        private Vector3 lastCameraPosition;


        private void Awake()
        {
            // Singleton pattern
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Get main camera
            mainCamera = Camera.main;

            // Subscribe to scene loaded event
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDestroy()
        {
            // Unsubscribe from scene loaded event
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void Update()
        {
            if (MaskManager.Instance.CurrentMask == MaskType.White)
            {
                backgroundImageName = "bgW";
            }
            else
            {
                backgroundImageName = "bgB";
            }

            RefreshBackground();
        }

        private void LateUpdate()
        {
            // Apply parallax effect if enabled
            if (enableParallax && backgroundParent != null && mainCamera != null)
            {
                UpdateParallaxPosition();
            }
        }

        /// <summary>
        /// Update background position with parallax effect
        /// </summary>
        private void UpdateParallaxPosition()
        {
            Vector3 currentCameraPosition = mainCamera.transform.position;
            
            // Calculate camera movement delta
            Vector3 deltaMovement = currentCameraPosition - lastCameraPosition;
            
            // Apply parallax movement (slower than camera)
            Vector3 parallaxMovement = new Vector3(
                deltaMovement.x * parallaxSpeed,
                deltaMovement.y * parallaxSpeed,
                0f
            );
            
            // Update background parent position (moves all tiles together)
            backgroundParent.transform.position += parallaxMovement;
            
            // Update last camera position
            lastCameraPosition = currentCameraPosition;
        }

        /// <summary>
        /// Called when a new scene is loaded
        /// </summary>
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // Only create background for stage scenes
            if (IsStageScene(scene.name))
            {
                CreateBackground();
            }
            else
            {
                // Destroy background if not in a stage scene
                DestroyBackground();
            }
        }

        /// <summary>
        /// Check if the scene is a stage scene
        /// </summary>
        private bool IsStageScene(string sceneName)
        {
            return sceneName == "Stage1" || 
                   sceneName == "Stage2" || 
                   sceneName == "Stage3" || 
                   sceneName == "Stage4";
        }

        /// <summary>
        /// Create background GameObject with sprite
        /// </summary>
        private void CreateBackground()
        {
            // Destroy existing background if any
            DestroyBackground();

            // Load background sprite from Resources
            Sprite backgroundSprite = Resources.Load<Sprite>(backgroundImageName);
            
            if (backgroundSprite == null)
            {
                Debug.LogWarning($"[StageBackgroundManager] 배경 이미지를 찾을 수 없습니다: Resources/{backgroundImageName}");
                Debug.LogWarning("[StageBackgroundManager] 배경 이미지를 Resources 폴더에 추가해주세요.");
                return;
            }

            // Create parent container for all background tiles
            backgroundParent = new GameObject("StageBackgroundParent");
            Vector3 cameraPosition = mainCamera != null ? mainCamera.transform.position : Vector3.zero;
            backgroundParent.transform.position = new Vector3(cameraPosition.x, cameraPosition.y, backgroundZPosition);

            // Initialize last camera position for parallax
            lastCameraPosition = cameraPosition;

            // Calculate base scale
            Vector3 baseScale = autoScale ? CalculateBackgroundScale(backgroundSprite) : manualScale;
            
            // Apply parallax size multiplier
            if (enableParallax)
            {
                baseScale *= parallaxSizeMultiplier;
            }

            // Apply vertical scale multiplier (make it taller)
            baseScale.y *= verticalScaleMultiplier;

            // Get sprite width in world units for tiling
            float spriteWorldWidth = backgroundSprite.bounds.size.x * baseScale.x;

            // Create center background
            backgroundObject = CreateBackgroundTile(backgroundSprite, Vector3.zero, baseScale, "Center");
            backgroundObject.transform.SetParent(backgroundParent.transform, false);

            // Create horizontal tiles if enabled
            if (enableHorizontalTiling)
            {
                for (int i = 1; i <= horizontalTileCount; i++)
                {
                    // Left tile
                    Vector3 leftPosition = new Vector3(-spriteWorldWidth * i, 0f, 0f);
                    GameObject leftTile = CreateBackgroundTile(backgroundSprite, leftPosition, baseScale, $"Left_{i}");
                    leftTile.transform.SetParent(backgroundParent.transform, false);

                    // Right tile
                    Vector3 rightPosition = new Vector3(spriteWorldWidth * i, 0f, 0f);
                    GameObject rightTile = CreateBackgroundTile(backgroundSprite, rightPosition, baseScale, $"Right_{i}");
                    rightTile.transform.SetParent(backgroundParent.transform, false);
                }
            }

            Debug.Log($"[StageBackgroundManager] 배경 생성 완료: {SceneManager.GetActiveScene().name}");
        }

        /// <summary>
        /// Create a single background tile
        /// </summary>
        private GameObject CreateBackgroundTile(Sprite sprite, Vector3 localPosition, Vector3 scale, string name)
        {
            GameObject tile = new GameObject($"StageBackground_{name}");
            
            // Add SpriteRenderer component
            SpriteRenderer spriteRenderer = tile.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = sprite;
            spriteRenderer.sortingLayerName = sortingLayerName;
            spriteRenderer.sortingOrder = sortingOrder;

            // Set local position and scale
            tile.transform.localPosition = localPosition;
            tile.transform.localScale = scale;

            return tile;
        }

        /// <summary>
        /// Calculate background scale to fit camera view
        /// </summary>
        private Vector3 CalculateBackgroundScale(Sprite sprite)
        {
            if (mainCamera == null || sprite == null)
            {
                return Vector3.one * 20f; // Default scale
            }

            // Get camera dimensions
            float cameraHeight = mainCamera.orthographicSize * 2f;
            float cameraWidth = cameraHeight * mainCamera.aspect;

            // Get sprite dimensions (in world units)
            float spriteWidth = sprite.bounds.size.x;
            float spriteHeight = sprite.bounds.size.y;

            // Calculate scale to cover the entire camera view
            float scaleX = cameraWidth / spriteWidth;
            float scaleY = cameraHeight / spriteHeight;

            // Use the larger scale to ensure full coverage
            float scale = Mathf.Max(scaleX, scaleY);

            return new Vector3(scale, scale, 1f);
        }

        /// <summary>
        /// Destroy existing background GameObject
        /// </summary>
        private void DestroyBackground()
        {
            if (backgroundParent != null)
            {
                Destroy(backgroundParent);
                backgroundParent = null;
            }
            
            if (backgroundObject != null)
            {
                backgroundObject = null;
            }
        }

        /// <summary>
        /// Manually refresh background (useful for testing)
        /// </summary>
        public void RefreshBackground()
        {
            if (IsStageScene(SceneManager.GetActiveScene().name))
            {
                CreateBackground();
            }
        }
    }
}

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

namespace ThawTheMask
{
    /// <summary>
    /// Manages the stage clear sequence with visual effects and audio.
    /// Handles Fade In/Out, Clear Image display, and BGM playback before loading the next stage.
    /// </summary>
    public class ClearSequenceManager : MonoBehaviour
    {
        public static ClearSequenceManager Instance { get; private set; }

        [Header("UI Elements")]
        [Tooltip("전체 화면을 덮는 페이드 패널 (검은색 패널)")]
        [SerializeField] private Image fadePanel;
        
        [Tooltip("클리어 시 보여줄 이미지 (예: 'Stage Clear!')")]
        [SerializeField] private Image clearImage;

        [Header("Audio Settings")]
        [Tooltip("클리어 시 재생할 음악")]
        [SerializeField] private AudioClip clearBGM;
        [Tooltip("음악 볼륨")]
        [Range(0f, 1f)]
        [SerializeField] private float volume = 1.0f;

        [Header("Timing Settings")]
        [Tooltip("페이드 인 시간 (검은 화면 -> 클리어 이미지)")]
        [SerializeField] private float fadeInDuration = 1.0f;
        
        [Tooltip("클리어 화면 유지 시간")]
        [SerializeField] private float displayDuration = 2.0f;
        
        [Tooltip("페이드 아웃 시간 (클리어 이미지 -> 검은 화면)")]
        [SerializeField] private float fadeOutDuration = 1.0f;

        private AudioSource audioSource;
        private bool isSequencePlaying = false;

        private void Awake()
        {
            // Singleton setup for easy access from Goal.cs
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            // AudioSource setup
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }

            // Ensure UI starts hidden
            if (fadePanel != null)
            {
                fadePanel.gameObject.SetActive(false);
                Color c = fadePanel.color;
                c.a = 0f;
                fadePanel.color = c;
            }

            if (clearImage != null)
            {
                clearImage.gameObject.SetActive(false);
                Color c = clearImage.color;
                c.a = 0f;
                clearImage.color = c;
            }
        }

        /// <summary>
        /// Starts the clear sequence and then loads the next scene.
        /// </summary>
        /// <param name="nextSceneName">Name of the scene to load after sequence</param>
        public void PlayClearSequence(string nextSceneName)
        {
            if (isSequencePlaying) return;
            StartCoroutine(SequenceRoutine(nextSceneName));
        }

        private IEnumerator SequenceRoutine(string nextSceneName)
        {
            isSequencePlaying = true;
            Debug.Log("[ClearSequenceManager] Starting Clear Sequence...");

            // 1. Setup UI
            if (fadePanel != null) fadePanel.gameObject.SetActive(true);
            if (clearImage != null) clearImage.gameObject.SetActive(true);

            // 2. Play Audio
            if (clearBGM != null && audioSource != null)
            {
                audioSource.clip = clearBGM;
                audioSource.volume = volume;
                audioSource.Play();
            }

            // 3. Fade In (Scene -> Black Background + Clear Image)
            // Fade both Panel and Image from 0 to 1 simultaneously
            float elapsed = 0f;
            while (elapsed < fadeInDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / fadeInDuration;
                
                if (fadePanel != null) SetImageAlpha(fadePanel, t);
                if (clearImage != null) SetImageAlpha(clearImage, t);
                
                yield return null;
            }
            // Ensure full opacity
            if (fadePanel != null) SetImageAlpha(fadePanel, 1f);
            if (clearImage != null) SetImageAlpha(clearImage, 1f);

            // 4. Wait
            yield return new WaitForSeconds(displayDuration);

            // 5. Fade Out (Clear Image -> Black Screen)
            // Keep FadePanel at 1 (Black) to hide scene transition, fade out ClearImage
            elapsed = 0f;
            while (elapsed < fadeOutDuration)
            {
                elapsed += Time.deltaTime;
                float t = 1f - (elapsed / fadeOutDuration); // 1 to 0
                
                if (clearImage != null) SetImageAlpha(clearImage, t);
                
                yield return null;
            }
            if (clearImage != null) SetImageAlpha(clearImage, 0f);

            // 6. Load Next Scene
            if (!string.IsNullOrEmpty(nextSceneName))
            {
                Debug.Log($"[ClearSequenceManager] Loading scene: {nextSceneName}");
                SceneManager.LoadScene(nextSceneName);
            }
            else
            {
                Debug.Log("[ClearSequenceManager] Next scene name is empty. Returning to StageSelect.");
                SceneManager.LoadScene("StageSelect");
            }

            isSequencePlaying = false;
        }

        private void SetImageAlpha(Image image, float alpha)
        {
            if (image == null) return;
            Color c = image.color;
            c.a = alpha;
            image.color = c;
        }

        private IEnumerator FadeRoutine(Image targetImage, float startAlpha, float endAlpha, float duration)
        {
            // Deprecated helper, but kept if needed for single fades
            if (targetImage == null) yield break;

            float elapsed = 0f;
            Color color = targetImage.color;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;
                color.a = Mathf.Lerp(startAlpha, endAlpha, t);
                targetImage.color = color;
                yield return null;
            }

            color.a = endAlpha;
            targetImage.color = color;
        }
    }
}

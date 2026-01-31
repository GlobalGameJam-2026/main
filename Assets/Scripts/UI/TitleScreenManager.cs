using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

namespace ThawTheMask
{
    /// <summary>
    /// Manages the title screen with fade-in effects and scene transitions
    /// </summary>
    public class TitleScreenManager : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private CanvasGroup backgroundCanvasGroup;
        [SerializeField] private CanvasGroup logoCanvasGroup;
        [SerializeField] private CanvasGroup buttonCanvasGroup;
        [SerializeField] private Button startButton;

        [Header("Animation Settings")]
        [SerializeField] private float backgroundFadeDuration = 1f;
        [SerializeField] private float logoFadeDelay = 0.5f;
        [SerializeField] private float logoFadeDuration = 1.5f;
        [SerializeField] private float buttonFadeDelay = 1f;
        [SerializeField] private float buttonFadeDuration = 1f;

        [Header("Scene Settings")]
        [SerializeField] private string firstLevelSceneName = "Stage1";

        private void Start()
        {
            // Initialize all elements as invisible
            if (backgroundCanvasGroup != null) backgroundCanvasGroup.alpha = 0;
            if (logoCanvasGroup != null) logoCanvasGroup.alpha = 0;
            if (buttonCanvasGroup != null) buttonCanvasGroup.alpha = 0;

            // Setup button
            if (startButton != null)
            {
                startButton.onClick.AddListener(OnStartButtonClicked);
            }

            // Start intro sequence
            StartCoroutine(IntroSequence());
        }

        private IEnumerator IntroSequence()
        {
            // Fade in background
            if (backgroundCanvasGroup != null)
            {
                yield return StartCoroutine(FadeIn(backgroundCanvasGroup, backgroundFadeDuration));
            }

            // Wait a bit, then fade in logo
            yield return new WaitForSeconds(logoFadeDelay);
            if (logoCanvasGroup != null)
            {
                yield return StartCoroutine(FadeIn(logoCanvasGroup, logoFadeDuration));
            }

            // Wait a bit, then fade in button
            yield return new WaitForSeconds(buttonFadeDelay);
            if (buttonCanvasGroup != null)
            {
                yield return StartCoroutine(FadeIn(buttonCanvasGroup, buttonFadeDuration));
            }
        }

        private IEnumerator FadeIn(CanvasGroup canvasGroup, float duration)
        {
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                canvasGroup.alpha = Mathf.Clamp01(elapsed / duration);
                yield return null;
            }
            canvasGroup.alpha = 1f;
        }

        private void OnStartButtonClicked()
        {
            Debug.Log($"Loading {firstLevelSceneName}...");
            SceneManager.LoadScene(firstLevelSceneName);
        }

        private void Update()
        {
            // Optional: Press any key to start
            if (Input.anyKeyDown && buttonCanvasGroup != null && buttonCanvasGroup.alpha >= 1f)
            {
                OnStartButtonClicked();
            }
        }
    }
}

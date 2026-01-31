using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace ThawTheMask
{
    /// <summary>
    /// Handles screen fade effect when switching masks.
    /// Attach this to a Canvas with an Image component.
    /// </summary>
    public class MaskTransitionEffect : MonoBehaviour
    {
        [Header("Fade Settings")]
        [SerializeField] private Image fadeImage;
        [SerializeField] private float fadeDuration = 0.2f;
        [SerializeField] private AnimationCurve fadeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        [Header("Flash Settings")]
        [SerializeField] private bool useMaskSpecificColors = true;
        [SerializeField] private Color blackMaskFlashColor = Color.white; // White flash when switching TO black mask
        [SerializeField] private Color whiteMaskFlashColor = Color.black; // Black flash when switching TO white mask
        [SerializeField] private float flashIntensity = 0.5f;

        [Header("Particle Settings")]
        [SerializeField] private ParticleSystem transitionParticles;
        [SerializeField] private bool spawnParticlesOnTransition = true;

        private Coroutine currentFadeCoroutine;

        private void Start()
        {
            // Subscribe to mask change events
            if (MaskManager.Instance != null)
            {
                MaskManager.Instance.OnMaskChanged += OnMaskChanged;
            }

            // Make sure fade image starts transparent
            if (fadeImage != null)
            {
                Color c = fadeImage.color;
                c.a = 0;
                fadeImage.color = c;
            }
        }

        private void OnDestroy()
        {
            if (MaskManager.Instance != null)
            {
                MaskManager.Instance.OnMaskChanged -= OnMaskChanged;
            }
        }

        private void OnMaskChanged(MaskType newMask)
        {
            // Stop any ongoing fade
            if (currentFadeCoroutine != null)
            {
                StopCoroutine(currentFadeCoroutine);
            }

            // Start new fade effect
            if (fadeImage != null)
            {
                currentFadeCoroutine = StartCoroutine(FadeEffect());
            }

            // Spawn particles
            if (spawnParticlesOnTransition && transitionParticles != null)
            {
                transitionParticles.Play();
            }
        }

        private IEnumerator FadeEffect()
        {
            float elapsed = 0f;
            
            // Choose flash color based on current mask
            Color targetColor;
            if (useMaskSpecificColors)
            {
                // When switching TO black mask, use white flash
                // When switching TO white mask, use black flash
                targetColor = MaskManager.Instance.CurrentMask == MaskType.Black 
                    ? blackMaskFlashColor 
                    : whiteMaskFlashColor;
            }
            else
            {
                // Fallback to white flash
                targetColor = Color.white;
            }

            // Fade in (to white/black)
            while (elapsed < fadeDuration / 2)
            {
                elapsed += Time.deltaTime;
                float t = fadeCurve.Evaluate(elapsed / (fadeDuration / 2));
                
                Color c = targetColor;
                c.a = t * flashIntensity;
                fadeImage.color = c;

                yield return null;
            }

            // Fade out (back to transparent)
            elapsed = 0f;
            while (elapsed < fadeDuration / 2)
            {
                elapsed += Time.deltaTime;
                float t = fadeCurve.Evaluate(elapsed / (fadeDuration / 2));
                
                Color c = targetColor;
                c.a = (1 - t) * flashIntensity;
                fadeImage.color = c;

                yield return null;
            }

            // Ensure fully transparent at end
            Color finalColor = targetColor;
            finalColor.a = 0;
            fadeImage.color = finalColor;

            currentFadeCoroutine = null;
        }
    }
}

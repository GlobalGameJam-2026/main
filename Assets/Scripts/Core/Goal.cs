using UnityEngine;
using UnityEngine.SceneManagement;

namespace ThawTheMask
{
    /// <summary>
    /// Goal object that triggers level completion
    /// </summary>
    public class Goal : MonoBehaviour
    {
        [Header("Visual Feedback")]
        [SerializeField] private ParticleSystem completionParticles;
        [SerializeField] private AudioClip completionSound;

        [Header("Next Level")]
        [SerializeField] private bool autoProgressToNextStage = true;
        [SerializeField] private string nextSceneName = "";
        [SerializeField] private float delayBeforeNextLevel = 2f;

        private bool hasBeenTriggered = false;
        private AudioSource audioSource;

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null && completionSound != null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.playOnAwake = false;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (hasBeenTriggered) return;

            if (other.CompareTag("Player"))
            {
                hasBeenTriggered = true;
                OnGoalReached();
            }
        }

        private void OnGoalReached()
        {
            Debug.Log("🎉 Goal Reached! Level Complete!");

            // Save progress
            SaveProgress();

            // Play particles
            if (completionParticles != null)
            {
                completionParticles.Play();
            }

            // Play sound
            if (audioSource != null && completionSound != null)
            {
                audioSource.PlayOneShot(completionSound);
            }

            // Show completion message
            ShowCompletionUI();

            // Load next level if specified
            if (!string.IsNullOrEmpty(nextSceneName))
            {
                Invoke(nameof(LoadNextLevel), delayBeforeNextLevel);
            }
        }

        private void SaveProgress()
        {
            // Get current stage number from scene name
            string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            
            if (sceneName.Contains("Stage1") || sceneName.Contains("stage1"))
            {
                ProgressManager.Instance.CompleteStage(1);
            }
            else if (sceneName.Contains("Stage2") || sceneName.Contains("stage2"))
            {
                ProgressManager.Instance.CompleteStage(2);
            }
            // Add more stages as needed
        }

        private void ShowCompletionUI()
        {
            // Simple debug message for now
            // TODO: Show proper UI panel
            Debug.Log("Level Complete! Press R to restart or wait for next level...");
        }

        private void LoadNextLevel()
        {
            // Auto-detect next stage if enabled
            if (autoProgressToNextStage && string.IsNullOrEmpty(nextSceneName))
            {
                string currentScene = SceneManager.GetActiveScene().name;
                
                // Simple stage progression
                if (currentScene.Contains("Stage1") || currentScene.Contains("stage1"))
                {
                    nextSceneName = "Stage2";
                }
                else if (currentScene.Contains("Stage2") || currentScene.Contains("stage2"))
                {
                    Debug.Log("🎊 Game Complete! No more stages.");
                    return;
                }
            }

            if (!string.IsNullOrEmpty(nextSceneName))
            {
                Debug.Log($"Loading next stage: {nextSceneName}");
                SceneManager.LoadScene(nextSceneName);
            }
        }

        private void Update()
        {
            // Debug: Press R to reload level
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }
}

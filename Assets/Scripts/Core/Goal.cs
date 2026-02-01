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

            // Play sound (Instant feedback sound)
            if (audioSource != null && completionSound != null)
            {
                audioSource.PlayOneShot(completionSound);
            }

            // Determine next scene name
            string targetNextScene = DetermineNextSceneName();

            // Trigger Clear Sequence
            if (ClearSequenceManager.Instance != null)
            {
                ClearSequenceManager.Instance.PlayClearSequence(targetNextScene);
            }
            else
            {
                // Fallback if no manager exists
                Debug.LogWarning("ClearSequenceManager not found! Loading next scene directly.");
                if (!string.IsNullOrEmpty(targetNextScene))
                {
                    Invoke(nameof(LoadNextLevelFallback), delayBeforeNextLevel);
                }
            }
        }

        private string DetermineNextSceneName()
        {
            // Use manually assigned name if available
            if (!string.IsNullOrEmpty(nextSceneName))
            {
                return nextSceneName;
            }

            // Auto-detect next stage if enabled
            if (autoProgressToNextStage)
            {
                string currentScene = SceneManager.GetActiveScene().name;
                
                if (currentScene.Contains("Stage1") || currentScene.Contains("stage1"))
                {
                    return "Stage2";
                }
                else if (currentScene.Contains("Stage2") || currentScene.Contains("stage2"))
                {
                    return "Stage3";
                }
                else if (currentScene.Contains("Stage3") || currentScene.Contains("stage3"))
                {
                    return "Stage4";
                }
                else if (currentScene.Contains("Stage4") || currentScene.Contains("stage4"))
                {
                    Debug.Log("🎊 Game Complete!");
                    return "StageSelect";
                }
            }

            return "StageSelect"; // Default fallback
        }

        private void SaveProgress()
        {
            string sceneName = SceneManager.GetActiveScene().name;
            Debug.Log($"[Goal] Saving progress for scene: {sceneName}");
            
            // Normalize checking (case insensitive)
            sceneName = sceneName.ToLower();

            int stageCompleted = 0;
            if (sceneName.Contains("stage1")) stageCompleted = 1;
            else if (sceneName.Contains("stage2")) stageCompleted = 2;
            else if (sceneName.Contains("stage3")) stageCompleted = 3;
            else if (sceneName.Contains("stage4")) stageCompleted = 4;

            if (stageCompleted > 0)
            {
                Debug.Log($"[Goal] Marking Stage {stageCompleted} as completed.");
                ProgressManager.Instance.CompleteStage(stageCompleted);
            }
            else
            {
                Debug.LogWarning($"[Goal] Could not determine stage number from scene name: {sceneName}");
            }
        }

        // Fallback method used ONLY if ClearSequenceManager is missing
        private void LoadNextLevelFallback()
        {
            string targetTextScene = DetermineNextSceneName();
            if (!string.IsNullOrEmpty(targetTextScene))
            {
                SceneManager.LoadScene(targetTextScene);
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

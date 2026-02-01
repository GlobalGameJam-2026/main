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

            // Seamless transition for Stage 4 sub-stages (excluding 4-4)
            string currentScene = SceneManager.GetActiveScene().name;
            bool isSeamlessStage = currentScene.Equals("Stage4") || 
                                   currentScene.Equals("Stage4-2") || 
                                   currentScene.Equals("Stage4-3");

            if (isSeamlessStage)
            {
                 // Skip Clear Sequence (Image/Fade) and load directly
                 Debug.Log($"[Goal] Seamless transition to {targetNextScene}");
                 SceneManager.LoadScene(targetNextScene);
            }
            else if (ClearSequenceManager.Instance != null)
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
            string currentScene = SceneManager.GetActiveScene().name;

            // SPECIAL HANDLING: Stage 4 variants
            if (currentScene.Equals("Stage4") || currentScene.Equals("stage4"))
                return "Stage4-2";
            if (currentScene.Equals("Stage4-2") || currentScene.Equals("stage4-2"))
                return "Stage4-3";
            if (currentScene.Equals("Stage4-3") || currentScene.Equals("stage4-3"))
                return "Stage4-4";
            if (currentScene.Equals("Stage4-4") || currentScene.Equals("stage4-4"))
            {
                return "Stage5";
            }
            if (currentScene.Equals("Stage5") || currentScene.Equals("stage5"))
            {
                Debug.Log("🎊 Game Complete!");
                return "StageSelect";
            }

            // Use manually assigned name if available
            if (!string.IsNullOrEmpty(nextSceneName))
            {
                return nextSceneName;
            }

            // Auto-detect next stage if enabled
            if (autoProgressToNextStage)
            {
                // Stages 1, 2, 3 return to Stage Select upon completion
                if (currentScene.Equals("Stage1") || currentScene.Equals("stage1") ||
                    currentScene.Equals("Stage2") || currentScene.Equals("stage2") ||
                    currentScene.Equals("Stage3") || currentScene.Equals("stage3"))
                {
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
            else if (sceneName.Contains("stage5")) stageCompleted = 5;

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

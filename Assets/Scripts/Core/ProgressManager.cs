using UnityEngine;

namespace ThawTheMask
{
    /// <summary>
    /// Manages game progress and stage unlock status using PlayerPrefs
    /// </summary>
    public class ProgressManager : MonoBehaviour
    {
        private static ProgressManager instance;
        public static ProgressManager Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject go = new GameObject("ProgressManager");
                    instance = go.AddComponent<ProgressManager>();
                    DontDestroyOnLoad(go);
                }
                return instance;
            }
        }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Mark a stage as completed
        /// </summary>
        public void CompleteStage(int stageNumber)
        {
            string key = $"Stage{stageNumber}_Completed";
            PlayerPrefs.SetInt(key, 1);
            PlayerPrefs.Save();
            Debug.Log($"Stage {stageNumber} marked as completed!");
        }

        /// <summary>
        /// Check if a stage is completed
        /// </summary>
        public bool IsStageCompleted(int stageNumber)
        {
            string key = $"Stage{stageNumber}_Completed";
            return PlayerPrefs.GetInt(key, 0) == 1;
        }

        /// <summary>
        /// Check if a stage is unlocked (Stage 1 is always unlocked, others require previous stage completion)
        /// </summary>
        public bool IsStageUnlocked(int stageNumber)
        {
            if (stageNumber == 1) return true; // Stage 1 is always unlocked
            return IsStageCompleted(stageNumber - 1); // Other stages require previous completion
        }

        /// <summary>
        /// Reset all progress (for debugging)
        /// </summary>
        public void ResetProgress()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            Debug.Log("All progress reset!");
        }

        /// <summary>
        /// Get the highest unlocked stage number
        /// </summary>
        public int GetHighestUnlockedStage()
        {
            int highest = 1;
            for (int i = 1; i <= 10; i++) // Check up to 10 stages
            {
                if (IsStageUnlocked(i))
                {
                    highest = i;
                }
                else
                {
                    break;
                }
            }
            return highest;
        }
    }
}

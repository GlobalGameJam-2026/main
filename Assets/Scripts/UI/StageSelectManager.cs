using UnityEngine;
using UnityEngine.SceneManagement;

namespace ThawTheMask
{
    /// <summary>
    /// Manages the stage selection screen
    /// </summary>
    public class StageSelectManager : MonoBehaviour
    {
        [Header("Stage Buttons")]
        [SerializeField] private StageButton[] stageButtons;

        [Header("Back Button")]
        [SerializeField] private UnityEngine.UI.Button backButton;
        [SerializeField] private string titleSceneName = "TitleScreen";

        private void Start()
        {
            // Setup back button
            if (backButton != null)
            {
                backButton.onClick.AddListener(OnBackButtonClicked);
            }

            // Refresh all stage buttons
            RefreshAllButtons();
        }

        private void RefreshAllButtons()
        {
            if (stageButtons != null)
            {
                // Check if stages 1-3 are all completed to unlock Stage 4
                bool allFirstThreeCompleted = ProgressManager.Instance.IsStageCompleted(1) &&
                                              ProgressManager.Instance.IsStageCompleted(2) &&
                                              ProgressManager.Instance.IsStageCompleted(3);

                foreach (var button in stageButtons)
                {
                    if (button != null)
                    {
                        // Hide Stage 4 until stages 1-3 are completed
                        if (button.StageNumber == 4)
                        {
                            button.gameObject.SetActive(allFirstThreeCompleted);
                        }
                        
                        button.RefreshState();
                    }
                }
            }
        }

        private void OnBackButtonClicked()
        {
            SceneManager.LoadScene(titleSceneName);
        }

        // Debug method to reset progress
        [ContextMenu("Reset All Progress")]
        private void ResetProgress()
        {
            ProgressManager.Instance.ResetProgress();
            RefreshAllButtons();
            Debug.Log("Progress reset! All stages locked except Stage 1.");
        }
    }
}

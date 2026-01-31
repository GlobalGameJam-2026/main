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
                foreach (var button in stageButtons)
                {
                    if (button != null)
                    {
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

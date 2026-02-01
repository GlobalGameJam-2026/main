using UnityEngine;
using UnityEngine.SceneManagement;

namespace ThawTheMask
{
    /// <summary>
    /// Manages in-game UI buttons (Restart, Exit, Help) and keyboard shortcuts.
    /// </summary>
    public class InGameUIManager : MonoBehaviour
    {
        [Header("UI Panels")]
        [Tooltip("도움말 팝업 패널 (껐다 켜질 오브젝트)")]
        [SerializeField] private GameObject helpPopupPanel;

        [Header("Valid Scenes")]
        [Tooltip("스테이지 선택 씬 이름 (나가기 버튼 클릭 시 이동)")]
        [SerializeField] private string stageSelectSceneName = "StageSelect";

        private bool isHelpPopupActive = false;

        private void Start()
        {
            // Ensure help popup is closed at start
            if (helpPopupPanel != null)
            {
                helpPopupPanel.SetActive(false);
                isHelpPopupActive = false;
            }
        }

        private void Update()
        {
            // Handle Help Popup Input
            if (isHelpPopupActive)
            {
                // Toggle off on any key press or mouse click
                if (Input.anyKeyDown)
                {
                    CloseHelpPopup();
                }
            }
            else
            {
                // Optional Keyboard Shortcuts for convenience (can be removed if strictly button-only)
                // R key to restart
                if (Input.GetKeyDown(KeyCode.R))
                {
                    RestartStage();
                }
                
                // ? key (Shift + /) or F1 for Help
                if (Input.GetKeyDown(KeyCode.Question) || Input.GetKeyDown(KeyCode.Slash) || Input.GetKeyDown(KeyCode.F1))
                {
                    ToggleHelpPopup();
                }

                // ESC or X key to Exit? (User image says 'x : Exit', '<- : Restart')
                // Implementing keyboard shortcuts based on the Help Image text
                if (Input.GetKeyDown(KeyCode.X))
                {
                    ExitToStageSelect();
                }

                // '<-' implies Backspace in many contexts, or maybe Left Arrow?
                // Given "Restart", Backspace is a common shortcut.
                if (Input.GetKeyDown(KeyCode.Backspace))
                {
                    RestartStage();
                }
            }
        }

        // --- Public Methods for UI Buttons ---

        /// <summary>
        /// Restarts the current stage.
        /// Assign this to the 'Refresh' button.
        /// </summary>
        public void RestartStage()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        /// <summary>
        /// Returns to the Stage Select screen.
        /// Assign this to the 'X' button.
        /// </summary>
        public void ExitToStageSelect()
        {
            SceneManager.LoadScene(stageSelectSceneName);
        }

        /// <summary>
        /// Toggles the help popup on/off.
        /// Assign this to the '?' button.
        /// </summary>
        public void ToggleHelpPopup()
        {
            if (helpPopupPanel != null)
            {
                isHelpPopupActive = !helpPopupPanel.activeSelf;
                helpPopupPanel.SetActive(isHelpPopupActive);

                // Optional: Pause game while help is open?
                // Time.timeScale = isHelpPopupActive ? 0f : 1f; 
            }
        }

        public void CloseHelpPopup()
        {
            if (helpPopupPanel != null)
            {
                isHelpPopupActive = false;
                helpPopupPanel.SetActive(false);
            }
        }
    }
}

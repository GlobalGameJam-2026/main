using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace ThawTheMask
{
    /// <summary>
    /// Individual stage button with lock/unlock functionality
    /// </summary>
    public class StageButton : MonoBehaviour
    {
        [Header("Stage Info")]
        [SerializeField] private int stageNumber = 1;
        [SerializeField] private string sceneName = "Stage1";

        [Header("UI Elements")]
        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI stageText;
        [SerializeField] private GameObject lockedIcon;
        [SerializeField] private GameObject completedIcon;

        [Header("Colors")]
        [SerializeField] private Color unlockedColor = Color.white;
        [SerializeField] private Color lockedColor = Color.gray;

        private void Start()
        {
            if (button == null) button = GetComponent<Button>();
            
            UpdateButtonState();
            button.onClick.AddListener(OnButtonClicked);
        }

        private void UpdateButtonState()
        {
            bool isUnlocked = ProgressManager.Instance.IsStageUnlocked(stageNumber);
            bool isCompleted = ProgressManager.Instance.IsStageCompleted(stageNumber);

            // Update button interactability
            button.interactable = isUnlocked;

            // Update text
            if (stageText != null)
            {
                stageText.text = $"Stage {stageNumber}";
                stageText.color = isUnlocked ? unlockedColor : lockedColor;
            }

            // Update icons
            if (lockedIcon != null)
            {
                lockedIcon.SetActive(!isUnlocked);
            }

            if (completedIcon != null)
            {
                completedIcon.SetActive(isCompleted);
            }

            // Update button color
            ColorBlock colors = button.colors;
            colors.normalColor = isUnlocked ? unlockedColor : lockedColor;
            colors.disabledColor = lockedColor;
            button.colors = colors;
        }

        private void OnButtonClicked()
        {
            if (ProgressManager.Instance.IsStageUnlocked(stageNumber))
            {
                Debug.Log($"Loading {sceneName}...");
                SceneManager.LoadScene(sceneName);
            }
        }

        // Public method to refresh button state (useful when returning from a stage)
        public void RefreshState()
        {
            UpdateButtonState();
        }
    }
}

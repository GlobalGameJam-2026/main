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

        // Public property to access stage number
        public int StageNumber => stageNumber;

        [Header("UI Elements")]
        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI stageText;
        [SerializeField] private UnityEngine.UI.Image stageImage;
        [SerializeField] private GameObject lockedIcon;
        [SerializeField] private GameObject completedIcon;

        [Header("Stage Images")]
        [SerializeField] private Sprite lockedSprite;
        [SerializeField] private Sprite unlockedSprite;
        [SerializeField] private Sprite completedSprite;

        [Header("Colors")]
        [SerializeField] private Color unlockedColor = Color.white;
        [SerializeField] private Color lockedColor = Color.gray;
        [SerializeField] private Color textColor = Color.black;

        private void Start()
        {
            if (button == null) button = GetComponent<Button>();

            // Auto-assign Stage Image if missing (Use the button's own image)
            if (stageImage == null)
            {
                stageImage = GetComponent<UnityEngine.UI.Image>();
                if (stageImage == null)
                {
                    Debug.LogWarning($"[StageButton {stageNumber}] Could not find Image component automatically.");
                }
                else
                {
                    Debug.Log($"[StageButton {stageNumber}] Auto-assigned Image component.");
                }
            }
            
            UpdateButtonState();
            button.onClick.AddListener(OnButtonClicked);
        }

        private void UpdateButtonState()
        {
            bool isUnlocked = ProgressManager.Instance.IsStageUnlocked(stageNumber);
            bool isCompleted = ProgressManager.Instance.IsStageCompleted(stageNumber);

            Debug.Log($"[StageButton {stageNumber}] Unlocked: {isUnlocked}, Completed: {isCompleted}");

            // Update button interactability
            button.interactable = isUnlocked;

            // Update text (always black)
            if (stageText != null)
            {
                stageText.text = $"Stage {stageNumber}";
                stageText.color = textColor;
            }

            // Update stage image based on state
            if (stageImage != null)
            {
                if (isCompleted && completedSprite != null)
                {
                    Debug.Log($"[StageButton {stageNumber}] Setting Completed Sprite");
                    stageImage.sprite = completedSprite;
                }
                else if (isUnlocked && unlockedSprite != null)
                {
                    Debug.Log($"[StageButton {stageNumber}] Setting Unlocked Sprite");
                    stageImage.sprite = unlockedSprite;
                }
                else if (lockedSprite != null)
                {
                    Debug.Log($"[StageButton {stageNumber}] Setting Locked Sprite");
                    stageImage.sprite = lockedSprite;
                }
            }
            else
            {
                Debug.LogWarning($"[StageButton {stageNumber}] Stage Image reference is missing! Assign it inInspector.");
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

            // Update button image color
            UnityEngine.UI.Image buttonImage = button.GetComponent<UnityEngine.UI.Image>();
            if (buttonImage != null)
            {
                buttonImage.color = isUnlocked ? unlockedColor : lockedColor;
            }
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

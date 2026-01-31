using UnityEngine;

namespace ThawTheMask
{
    /// <summary>
    /// Component attached to the player to handle mask switching input.
    /// Communicates with MaskManager to toggle the mask state.
    /// </summary>
    public class MaskSwitcher : MonoBehaviour
    {
        [Header("Input Settings")]
        [SerializeField] private KeyCode maskToggleKey = KeyCode.LeftShift;

        [Header("Audio (Optional)")]
        [SerializeField] private AudioClip maskSwitchSound;
        private AudioSource audioSource;

        [Header("Visual Feedback (Optional)")]
        [SerializeField] private float switchCooldown = 0.1f;
        private float lastSwitchTime;

        private void Start()
        {
            // Get or add AudioSource component
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null && maskSwitchSound != null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.playOnAwake = false;
            }
        }

        private void Update()
        {
            HandleMaskInput();
        }

        private void HandleMaskInput()
        {
            // Check if enough time has passed since last switch (prevent spam)
            if (Time.time - lastSwitchTime < switchCooldown)
                return;

            // Toggle mask when key is pressed
            if (Input.GetKeyDown(maskToggleKey))
            {
                ToggleMask();
            }
        }

        private void ToggleMask()
        {
            if (MaskManager.Instance == null)
            {
                Debug.LogWarning("MaskManager not found in scene! Please add MaskManager to the scene.");
                return;
            }

            // Toggle the mask
            MaskManager.Instance.ToggleMask();
            lastSwitchTime = Time.time;

            // Play sound effect
            if (audioSource != null && maskSwitchSound != null)
            {
                audioSource.PlayOneShot(maskSwitchSound);
            }

            // Optional: Add visual feedback here (particle effect, screen flash, etc.)
        }

        // Public method to allow other scripts to trigger mask switch
        public void TriggerMaskSwitch()
        {
            ToggleMask();
        }
    }
}

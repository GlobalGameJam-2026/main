using UnityEngine;
using System;

namespace ThawTheMask
{
    /// <summary>
    /// Singleton manager for the mask system.
    /// Handles global mask state and notifies objects when the mask changes.
    /// </summary>
    public class MaskManager : MonoBehaviour
    {
        public static MaskManager Instance { get; private set; }

        [Header("Mask State")]
        [SerializeField] private MaskType currentMask = MaskType.White;

        [Header("Visual Settings")]
        [SerializeField] private Color blackMaskBackgroundColor = Color.black;
        [SerializeField] private Color whiteMaskBackgroundColor = Color.white;
        [SerializeField] private Camera mainCamera;

        // Event fired when mask changes
        public event Action<MaskType> OnMaskChanged;

        public MaskType CurrentMask => currentMask;

        private void Awake()
        {
            // Singleton pattern
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Get main camera if not assigned
            if (mainCamera == null)
            {
                mainCamera = Camera.main;
            }
        }

        private void Start()
        {
            // Apply initial mask state
            ApplyMaskVisuals();
        }

        /// <summary>
        /// Toggle between black and white mask
        /// </summary>
        public void ToggleMask()
        {
            currentMask = currentMask == MaskType.Black ? MaskType.White : MaskType.Black;
            ApplyMaskVisuals();
            OnMaskChanged?.Invoke(currentMask);
        }

        /// <summary>
        /// Set specific mask type
        /// </summary>
        public void SetMask(MaskType maskType)
        {
            if (currentMask == maskType) return;

            currentMask = maskType;
            ApplyMaskVisuals();
            OnMaskChanged?.Invoke(currentMask);
        }

        /// <summary>
        /// Apply visual changes based on current mask
        /// </summary>
        private void ApplyMaskVisuals()
        {
            if (mainCamera != null)
            {
                mainCamera.backgroundColor = currentMask == MaskType.Black 
                    ? blackMaskBackgroundColor 
                    : whiteMaskBackgroundColor;
            }
        }

        private void OnValidate()
        {
            // Update visuals in editor when values change
            if (Application.isPlaying && mainCamera != null)
            {
                ApplyMaskVisuals();
            }
        }
    }

    /// <summary>
    /// Enum for mask types
    /// </summary>
    public enum MaskType
    {
        Black,  // Black background, white objects visible
        White   // White background, black objects visible
    }
}

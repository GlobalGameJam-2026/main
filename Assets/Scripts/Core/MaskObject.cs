using UnityEngine;

namespace ThawTheMask
{
    /// <summary>
    /// Component for objects that should only be visible/active in specific mask states.
    /// Attach this to platforms, obstacles, or any object that changes with the mask.
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class MaskObject : MonoBehaviour
    {
        [Header("Visibility Settings")]
        [SerializeField] private MaskVisibility visibleIn = MaskVisibility.Both;

        [Header("Object Behavior")]
        [SerializeField] private bool disableColliderWhenHidden = true;
        [SerializeField] private bool disableRendererWhenHidden = true;

        [Header("Visual Settings")]
        [SerializeField] private Color blackMaskColor = Color.white; // White lines in black world
        [SerializeField] private Color whiteMaskColor = Color.black; // Black lines in white world

        private SpriteRenderer spriteRenderer;
        private Collider2D objectCollider;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            objectCollider = GetComponent<Collider2D>();
        }

        private void Start()
        {
            // Subscribe to mask change events
            if (MaskManager.Instance != null)
            {
                MaskManager.Instance.OnMaskChanged += OnMaskChanged;
                // Apply initial state
                OnMaskChanged(MaskManager.Instance.CurrentMask);
            }
            else
            {
                Debug.LogWarning($"MaskManager not found! {gameObject.name} will not respond to mask changes.");
            }
        }

        private void OnDestroy()
        {
            // Unsubscribe from events
            if (MaskManager.Instance != null)
            {
                MaskManager.Instance.OnMaskChanged -= OnMaskChanged;
            }
        }

        private void OnMaskChanged(MaskType currentMask)
        {
            bool shouldBeVisible = ShouldBeVisible(currentMask);

            // Update renderer
            if (disableRendererWhenHidden && spriteRenderer != null)
            {
                spriteRenderer.enabled = shouldBeVisible;
            }

            // Update collider
            if (disableColliderWhenHidden && objectCollider != null)
            {
                objectCollider.enabled = shouldBeVisible;
            }

            // Update color based on mask
            if (spriteRenderer != null && shouldBeVisible)
            {
                spriteRenderer.color = currentMask == MaskType.Black ? blackMaskColor : whiteMaskColor;
            }
        }

        private bool ShouldBeVisible(MaskType currentMask)
        {
            switch (visibleIn)
            {
                case MaskVisibility.BlackOnly:
                    return currentMask == MaskType.Black;
                case MaskVisibility.WhiteOnly:
                    return currentMask == MaskType.White;
                case MaskVisibility.Both:
                    return true;
                case MaskVisibility.Neither:
                    return false;
                default:
                    return true;
            }
        }

        // Editor helper to preview visibility
        private void OnValidate()
        {
            if (Application.isPlaying && MaskManager.Instance != null)
            {
                OnMaskChanged(MaskManager.Instance.CurrentMask);
            }
        }
    }

    /// <summary>
    /// Defines when an object should be visible
    /// </summary>
    public enum MaskVisibility
    {
        BlackOnly,  // Only visible when wearing black mask
        WhiteOnly,  // Only visible when wearing white mask
        Both,       // Always visible (but color changes)
        Neither     // Never visible (hidden object)
    }
}

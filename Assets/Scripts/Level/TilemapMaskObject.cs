using UnityEngine;
using UnityEngine.Tilemaps;

namespace ThawTheMask
{
    /// <summary>
    /// Controls tilemap visibility based on mask state
    /// Similar to MaskObject but for Tilemaps
    /// </summary>
    [RequireComponent(typeof(Tilemap))]
    [RequireComponent(typeof(TilemapRenderer))]
    public class TilemapMaskObject : MonoBehaviour
    {
        [Header("Mask Visibility")]
        [SerializeField] private MaskVisibility visibility = MaskVisibility.Both;

        [Header("Visual Settings")]
        [SerializeField] private Color blackMaskColor = Color.white;
        [SerializeField] private Color whiteMaskColor = Color.black;

        private Tilemap tilemap;
        private TilemapRenderer tilemapRenderer;
        private TilemapCollider2D tilemapCollider;

        private void Awake()
        {
            tilemap = GetComponent<Tilemap>();
            tilemapRenderer = GetComponent<TilemapRenderer>();
            tilemapCollider = GetComponent<TilemapCollider2D>();
        }

        private void Start()
        {
            // Subscribe to mask state changes
            if (MaskManager.Instance != null)
            {
                MaskManager.Instance.OnMaskChanged += OnMaskStateChanged;
                
                // Initialize state
                OnMaskStateChanged(MaskManager.Instance.CurrentMask);
            }
        }

        private void OnDestroy()
        {
            // Unsubscribe from events
            if (MaskManager.Instance != null)
            {
                MaskManager.Instance.OnMaskChanged -= OnMaskStateChanged;
            }
        }

        private void OnMaskStateChanged(MaskType maskType)
        {
            bool shouldBeVisible = ShouldBeVisible(maskType);
            
            // Update visibility
            tilemapRenderer.enabled = shouldBeVisible;
            
            // Update collider
            if (tilemapCollider != null)
            {
                tilemapCollider.enabled = shouldBeVisible;
            }

            // Update color based on mask
            if (shouldBeVisible)
            {
                tilemapRenderer.material.color = maskType == MaskType.Black ? blackMaskColor : whiteMaskColor;
            }
        }

        private bool ShouldBeVisible(MaskType maskType)
        {
            switch (visibility)
            {
                case MaskVisibility.Both:
                    return true;
                case MaskVisibility.BlackOnly:
                    return maskType == MaskType.Black;
                case MaskVisibility.WhiteOnly:
                    return maskType == MaskType.White;
                default:
                    return true;
            }
        }

        // Editor helper to visualize visibility in Scene view
        private void OnDrawGizmos()
        {
            if (tilemap == null) return;

            // Draw a colored outline based on visibility type
            Color gizmoColor = visibility switch
            {
                MaskVisibility.BlackOnly => new Color(0, 0, 0, 0.3f),
                MaskVisibility.WhiteOnly => new Color(1, 1, 1, 0.3f),
                MaskVisibility.Both => new Color(0.5f, 0.5f, 0.5f, 0.3f),
                _ => Color.gray
            };

            Gizmos.color = gizmoColor;
            
            // Draw bounds
            Bounds bounds = tilemap.localBounds;
            Gizmos.DrawWireCube(transform.position + bounds.center, bounds.size);
        }
    }
}

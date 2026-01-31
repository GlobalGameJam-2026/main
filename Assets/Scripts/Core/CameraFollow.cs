using UnityEngine;

namespace ThawTheMask
{
    /// <summary>
    /// Simple camera follow script for 2D platformer
    /// </summary>
    public class CameraFollow : MonoBehaviour
    {
        [Header("Target")]
        [SerializeField] private Transform target;
        [SerializeField] private bool autoFindPlayer = true;

        [Header("Follow Settings")]
        [SerializeField] private Vector3 offset = new Vector3(0, 2f, -10f);
        [SerializeField] private float smoothSpeed = 5f;

        [Header("Bounds (Optional)")]
        [SerializeField] private bool useBounds = false;
        [SerializeField] private Vector2 minBounds = new Vector2(-100, -100);
        [SerializeField] private Vector2 maxBounds = new Vector2(100, 100);

        [Header("Dead Zone")]
        [SerializeField] private bool useDeadZone = true;
        [SerializeField] private float deadZoneWidth = 2f;
        [SerializeField] private float deadZoneHeight = 1f;

        private void Start()
        {
            if (autoFindPlayer && target == null)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    target = player.transform;
                    Debug.Log("CameraFollow: Auto-found player");
                }
                else
                {
                    Debug.LogWarning("CameraFollow: Player not found! Make sure Player has 'Player' tag.");
                }
            }
        }

        private void LateUpdate()
        {
            // Auto-refind player if target is lost (e.g., after scene reload)
            if (target == null && autoFindPlayer)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    target = player.transform;
                    Debug.Log("CameraFollow: Re-found player after scene reload");
                }
                else
                {
                    return; // No player found, skip this frame
                }
            }

            if (target == null) return;

            Vector3 desiredPosition = target.position + offset;

            // Apply dead zone
            if (useDeadZone)
            {
                Vector3 currentPos = transform.position;
                float deltaX = desiredPosition.x - currentPos.x;
                float deltaY = desiredPosition.y - currentPos.y;

                // Only move if outside dead zone
                if (Mathf.Abs(deltaX) < deadZoneWidth / 2f)
                {
                    desiredPosition.x = currentPos.x;
                }

                if (Mathf.Abs(deltaY) < deadZoneHeight / 2f)
                {
                    desiredPosition.y = currentPos.y;
                }
            }

            // Smooth follow
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

            // Apply bounds
            if (useBounds)
            {
                smoothedPosition.x = Mathf.Clamp(smoothedPosition.x, minBounds.x, maxBounds.x);
                smoothedPosition.y = Mathf.Clamp(smoothedPosition.y, minBounds.y, maxBounds.y);
            }

            // Keep Z position fixed
            smoothedPosition.z = offset.z;

            transform.position = smoothedPosition;
        }

        // Gizmos for visualization in editor
        private void OnDrawGizmosSelected()
        {
            if (useDeadZone)
            {
                Gizmos.color = Color.yellow;
                Vector3 center = transform.position;
                center.z = 0;
                Gizmos.DrawWireCube(center, new Vector3(deadZoneWidth, deadZoneHeight, 0));
            }

            if (useBounds)
            {
                Gizmos.color = Color.red;
                Vector3 boundsCenter = new Vector3(
                    (minBounds.x + maxBounds.x) / 2f,
                    (minBounds.y + maxBounds.y) / 2f,
                    0
                );
                Vector3 boundsSize = new Vector3(
                    maxBounds.x - minBounds.x,
                    maxBounds.y - minBounds.y,
                    0
                );
                Gizmos.DrawWireCube(boundsCenter, boundsSize);
            }
        }
    }
}

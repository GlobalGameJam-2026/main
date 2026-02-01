using UnityEngine;

namespace ThawTheMask
{
    /// <summary>
    /// Attach this to an object to make it a pushable box.
    /// It ensures the object has a Rigidbody2D and Collider, and configures them for pushing.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
    public class PushableBox : MonoBehaviour
    {
        [Header("Physics Settings")]

        
        [Tooltip("밀리지 않을 때 금방 멈추도록 하는 마찰력(저항). X축에만 적용됩니다.")]
        [SerializeField] private float friction = 10f; 

        [Header("Mask Settings")]
        [Tooltip("이 상자가 보이고 상호작용 가능한 마스크 타입")]
        [SerializeField] private MaskType targetMask = MaskType.White;

        private Rigidbody2D rb;
        private Collider2D col;
        private Renderer rend;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            col = GetComponent<Collider2D>();
            rend = GetComponent<Renderer>();

        }

        private void Start()
        {
            if (MaskManager.Instance != null)
            {
                // Register event
                MaskManager.Instance.OnMaskChanged += OnMaskChanged;
                // Initialize state
                UpdateVisibility(MaskManager.Instance.CurrentMask);
            }
        }

        private void OnDestroy()
        {
            if (MaskManager.Instance != null)
            {
                MaskManager.Instance.OnMaskChanged -= OnMaskChanged;
            }
        }

        private void OnMaskChanged(MaskType newMask)
        {
            UpdateVisibility(newMask);
        }

        private void UpdateVisibility(MaskType currentMask)
        {
            bool isVisible = (currentMask == targetMask);

            // Toggle Renderer (Visuals)
            if (rend != null) rend.enabled = isVisible;

            // Toggle Collider (Collision)
            if (col != null) col.enabled = isVisible;

            // Toggle Physics Simulation (Gravity/Movement)
            // Warning: If disabled, it stays in place. If initialized in air while hidden, it won't fall until revealed.
            if (rb != null) rb.simulated = isVisible;
        }

        private void Reset()
        {
            // This is called when the component is added or Reset is clicked in Inspector
            rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
                rb.mass = 5f; // Recommended default
                rb.gravityScale = 3f; // Recommended default
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
                rb.interpolation = RigidbodyInterpolation2D.Interpolate;
                rb.linearDamping = 0f;
                rb.angularDamping = 0f;
            }
        }

        private void FixedUpdate()
        {
            // improved Friction Logic: Apply damping ONLY to X axis
            // This prevents "floating" or slow falling behavior
            if (rb != null && rb.simulated)
            {
                Vector2 velocity = rb.linearVelocity; // Or rb.velocity in older versions
                
                // Move X velocity towards 0
                if (Mathf.Abs(velocity.x) > 0.01f)
                {
                    velocity.x = Mathf.MoveTowards(velocity.x, 0, friction * Time.fixedDeltaTime);
                    rb.linearVelocity = velocity;
                }
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            // Optional: Play sound when hitting ground or wall
        }
    }
}

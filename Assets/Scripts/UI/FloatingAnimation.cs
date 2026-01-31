using UnityEngine;

namespace ThawTheMask
{
    /// <summary>
    /// Makes an object float up and down smoothly
    /// </summary>
    public class FloatingAnimation : MonoBehaviour
    {
        [Header("Float Settings")]
        [SerializeField] private float floatAmplitude = 0.3f;
        [SerializeField] private float floatSpeed = 1f;
        
        private Vector3 startPosition;
        private float timeOffset;

        private void Start()
        {
            startPosition = transform.localPosition;
            timeOffset = Random.Range(0f, 2f * Mathf.PI);
        }

        private void Update()
        {
            float newY = startPosition.y + Mathf.Sin((Time.time + timeOffset) * floatSpeed) * floatAmplitude;
            transform.localPosition = new Vector3(startPosition.x, newY, startPosition.z);
        }
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

namespace ThawTheMask
{
    /// <summary>
    /// Death zone that restarts the level when player falls
    /// </summary>
    public class DeathZone : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float restartDelay = 1f;
        [SerializeField] private bool showDebugMessage = true;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                if (showDebugMessage)
                {
                    Debug.Log("💀 Player fell! Restarting level...");
                }

                Invoke(nameof(RestartLevel), restartDelay);
            }
        }

        private void RestartLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}

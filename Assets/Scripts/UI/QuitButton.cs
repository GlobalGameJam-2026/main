using UnityEngine;
using UnityEngine.UI;

namespace ThawTheMask
{
    /// <summary>
    /// Simple script to quit the game when button is clicked
    /// </summary>
    public class QuitButton : MonoBehaviour
    {
        private void Start()
        {
            Button button = GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(QuitGame);
            }
        }

        private void QuitGame()
        {
            Debug.Log("Quitting game...");
            
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }
    }
}

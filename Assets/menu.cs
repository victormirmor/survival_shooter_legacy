using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class menu : MonoBehaviour
{
    // Load a scene using its exact string name
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

            public void Quit()
        {
            Time.timeScale = 1f; // Restaura la velocidad del tiempo antes de salir

            #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
        }
}






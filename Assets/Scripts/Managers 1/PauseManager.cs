using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CompleteProject
{
    public class PauseManager : MonoBehaviour
    {
        [Header("Referencias de UI")]
        public GameObject pausePanel; // Panel hijo con los botones y sliders

        [Header("Audio Snapshots")]
        public AudioMixerSnapshot paused;
        public AudioMixerSnapshot unpaused;

        public bool IsPaused { get; private set; }

        private void Awake()
        {
            // Oculta el panel por código desde el frame 0
            if (pausePanel != null)
            {
                pausePanel.SetActive(false);
            }

            Time.timeScale = 1f;
            IsPaused = false;
        }

        private void Update()
        {
            // Captura "Cancel" (ESC / Gamepad Start) en lugar de hardcodear la tecla
            if (Input.GetButtonDown("Cancel"))
            {
                TogglePause();
            }
        }

        /// <summary>
        /// Alterna el estado de pausa. Ideal para asociar también a un botón de UI.
        /// </summary>
        public void TogglePause()
        {
            IsPaused = !IsPaused;

            if (pausePanel != null)
            {
                pausePanel.SetActive(IsPaused);
            }

            Time.timeScale = IsPaused ? 0f : 1f;
            
            // Limpia el foco de la UI si se está reanudando
            if (!IsPaused && EventSystem.current != null)
            {
                EventSystem.current.SetSelectedGameObject(null);
            }

            UpdateAudioSnapshot();
        }

        /// <summary>
        /// Método directo para enganchar en el OnClick() del botón RESUME de la UI.
        /// </summary>
        public void Resume()
        {
            if (IsPaused)
            {
                TogglePause();
            }
        }

        private void UpdateAudioSnapshot()
        {
            if (IsPaused)
            {
                if (paused != null) paused.TransitionTo(0.01f);
            }
            else
            {
                if (unpaused != null) unpaused.TransitionTo(0.01f);
            }
        }

        /// <summary>
        /// Método directo para enganchar en el OnClick() del botón QUIT GAME.
        /// </summary>
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
}
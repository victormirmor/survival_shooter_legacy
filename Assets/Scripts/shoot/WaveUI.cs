using UnityEngine;
using UnityEngine.UI;

namespace CompleteProject
{
    public class WaveUI : MonoBehaviour
    {
        [Header("Configuración de Display")]
        public float displayDuration = 2f; // Tiempo que permanece visible en pantalla

        [Header("Referencias (Opcional)")]
        public Text waveText;

        private void Awake()
        {
            // Si no se asignó manualmente en el Inspector, busca el Text en este mismo GameObject
            if (waveText == null)
            {
                waveText = GetComponent<Text>();
            }

            // Arranca oculto por seguridad
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Muestra el anuncio de la oleada actual en el centro de la pantalla.
        /// </summary>
        public void ShowWave(int waveNumber)
        {
            if (waveText != null)
            {
                waveText.text = $"WAVE {waveNumber}";
            }

            gameObject.SetActive(true);

            // Cancela ocultados previos e inicia el conteo para apagarlo
            CancelInvoke(nameof(HideWave));
            Invoke(nameof(HideWave), displayDuration);
        }

        private void HideWave()
        {
            gameObject.SetActive(false);
        }
    }
}
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace CompleteProject
{
    public class WaveUI : MonoBehaviour
    {
        [Header("Configuración de Display")]
        public float displayDuration = 2f;

        [Header("Referencias de Texto")]
        public Text waveText;
        public Text countdownText;

        private EnemySpawn spawner;
        private Coroutine countdownCoroutine;

        private void Awake()
        {
            if (waveText == null)
            {
                waveText = GetComponent<Text>();
            }

            ClearWaveText();
            ClearCountdownText();
        }

        private void Start()
        {
            spawner = FindObjectOfType<EnemySpawn>();
        }

        public void ShowWave(int waveNumber)
        {
            if (countdownCoroutine != null)
            {
                StopCoroutine(countdownCoroutine);
            }

            ClearCountdownText();

            if (waveText != null)
            {
                waveText.text = $"WAVE {waveNumber}";
            }

            CancelInvoke(nameof(HideWaveAndStartTracker));
            Invoke(nameof(HideWaveAndStartTracker), displayDuration);
        }

        private void HideWaveAndStartTracker()
        {
            ClearWaveText();

            if (countdownCoroutine != null)
            {
                StopCoroutine(countdownCoroutine);
            }
            countdownCoroutine = StartCoroutine(TrackNextWaveCountdown());
        }

        private IEnumerator TrackNextWaveCountdown()
        {
            if (spawner == null) yield break;

            while (spawner.enemiesAlive > 0 || spawner.enemiesSpawnedSoFar < spawner.totalEnemiesInWave)
            {
                yield return new WaitForSeconds(0.2f);
            }

            float timer = spawner.timeBetweenWaves;

            while (timer > 0)
            {
                if (countdownText != null)
                {
                    countdownText.text = $"NEXT WAVE IN {Mathf.CeilToInt(timer)}...";
                }

                yield return new WaitForSeconds(1f);
                timer -= 1f;
            }

            ClearCountdownText();
        }

        /// <summary>
        /// Muestra la cuenta regresiva previa a la Oleada 1.
        /// </summary>
        public void ShowInitialCountdown(int seconds)
        {
            ClearWaveText();

            if (countdownText != null)
            {
                countdownText.text = $"PREPARE! WAVE 1 IN {seconds}...";
            }
        }

        private void ClearWaveText()
        {
            if (waveText != null)
            {
                waveText.text = "";
            }
        }

        private void ClearCountdownText()
        {
            if (countdownText != null)
            {
                countdownText.text = "";
            }
        }
    }
}
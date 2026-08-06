using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace CompleteProject
{
    public class LeaderboardManager : MonoBehaviour
    {
        public const int MAX_ENTRIES = 5;
        private const string LEADERBOARD_KEY_PREFIX = "Leaderboard_Score_";

        [Header("UI References")]
        [Tooltip("Asignar los Text componentes en orden del 1 al 5")]
        public Text[] scoreTexts;

        void Start()
        {
            Debug.Log("[LeaderboardManager] Iniciando componente. Mostrando tabla actual...");
            DisplayLeaderboard();
        }

        /// <summary>
        /// Evalúa y guarda el puntaje actual dentro del Top 5 si corresponde.
        /// </summary>
        public static void SubmitScore(int newScore)
        {
            Debug.Log($"[LeaderboardManager] Intentando registrar nuevo puntaje: {newScore}");

            List<int> scores = GetScores();
            scores.Add(newScore);
            scores.Sort((a, b) => b.CompareTo(a)); // Orden descendente

            // Guardar solo los mejores 5
            for (int i = 0; i < MAX_ENTRIES; i++)
            {
                if (i < scores.Count)
                {
                    PlayerPrefs.SetInt(LEADERBOARD_KEY_PREFIX + i, scores[i]);
                    Debug.Log($"[LeaderboardManager] Guardado Posición {i + 1}: {scores[i]}");
                }
                else
                {
                    PlayerPrefs.SetInt(LEADERBOARD_KEY_PREFIX + i, 0);
                }
            }

            PlayerPrefs.Save();
            Debug.Log("[LeaderboardManager] PlayerPrefs.Save() ejecutado correctamente.");
        }

        /// <summary>
        /// Obtiene la lista actual del Top 5 desde PlayerPrefs.
        /// </summary>
        public static List<int> GetScores()
        {
            List<int> scores = new List<int>();
            for (int i = 0; i < MAX_ENTRIES; i++)
            {
                int val = PlayerPrefs.GetInt(LEADERBOARD_KEY_PREFIX + i, 0);
                if (val > 0)
                {
                    scores.Add(val);
                }
            }
            Debug.Log($"[LeaderboardManager] Puntajes cargados desde PlayerPrefs: {scores.Count} registros encontrados.");
            return scores;
        }

        /// <summary>
        /// Muestra los puntajes en los componentes UI Text.
        /// </summary>
        public void DisplayLeaderboard()
        {
            if (scoreTexts == null || scoreTexts.Length == 0)
            {
                Debug.LogWarning("[LeaderboardManager] ¡Atención! El array 'scoreTexts' está vacío o no asignado en el Inspector.");
                return;
            }

            List<int> scores = GetScores();

            for (int i = 0; i < scoreTexts.Length; i++)
            {
                if (scoreTexts[i] != null)
                {
                    if (i < scores.Count)
                    {
                        scoreTexts[i].text = (i + 1) + ". " + scores[i];
                    }
                    else
                    {
                        scoreTexts[i].text = (i + 1) + ". ---";
                    }
                }
                else
                {
                    Debug.LogWarning($"[LeaderboardManager] El elemento {i} en 'scoreTexts' no tiene un componente Text asignado.");
                }
            }
        }
    }
}

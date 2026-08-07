using UnityEngine;
using UnityEngine.UI;

namespace CompleteProject
{
    public class ScoreManager : MonoBehaviour
    {
        public static int score = 0;
        private static int previousScore = -1;

        [Header("Referencias de UI")]
        public Text scoreText;          // Texto de dinero/puntos (arriba a la izquierda)
        public Text highScoreText;      // Texto del trofeo (arriba al centro)

        private const string HIGH_SCORE_KEY = "HighScore";

        void Awake()
        {
            score = 0;
            previousScore = -1;
        }

        void Start()
        {
            UpdateHighScoreUI();
            UpdateScoreUI();
        }

        void Update()
        {
            if (score != previousScore)
            {
                previousScore = score;
                UpdateScoreUI();

                if (score > GetHighScore())
                {
                    if (highScoreText != null)
                    {
                        highScoreText.text = score.ToString();
                    }
                }
            }
        }

        /// <summary>
        /// Se ejecuta automáticamente cuando el GameObject se desactiva o la escena cambia.
        /// </summary>
        void OnDisable()
        {
            CheckAndSaveHighScore();
        }

        public static void AddScore(int amount)
        {
            score += amount;
        }

        public static int GetHighScore()
        {
            return PlayerPrefs.GetInt(HIGH_SCORE_KEY, 0);
        }

        public static bool CheckAndSaveHighScore()
        {
            int currentHighScore = GetHighScore();

            if (score > currentHighScore)
            {
                PlayerPrefs.SetInt(HIGH_SCORE_KEY, score);
                PlayerPrefs.Save();
                Debug.Log($"[ScoreManager] Guardado en OnDisable/GameOver. Nuevo Récord: {score}");
                return true;
            }

            return false;
        }

        public void UpdateScoreUI()
        {
            if (scoreText != null)
            {
                scoreText.text = score.ToString();
            }
        }

        public void UpdateHighScoreUI()
        {
            if (highScoreText != null)
            {
                highScoreText.text = GetHighScore().ToString();
            }
        }
    }
}
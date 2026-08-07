using UnityEngine;
using UnityEngine.UI;

namespace CompleteProject
{
    public class HighScoreDisplay : MonoBehaviour
    {
        private Text highScoreText;

        void Awake()
        {
            highScoreText = GetComponent<Text>();
        }

        void OnEnable()
        {
            UpdateHighScoreDisplay();
        }

        public void UpdateHighScoreDisplay()
        {
            if (highScoreText != null)
            {
                int topScore = PlayerPrefs.GetInt("HighScore", 0);
                highScoreText.text = topScore.ToString();
            }
        }
    }
}
using UnityEngine;

namespace CompleteProject
{
    public class GameOverManager_new : MonoBehaviour
    {
        public PlayerHealth playerHealth;
        private GameObject player;
        private Animator anim;
        private bool isGameOver = false;

        void Awake ()
        {
            anim = GetComponent <Animator> ();
            player = GameObject.FindGameObjectWithTag ("Player");
            if (player != null)
            {
                playerHealth = player.GetComponent <PlayerHealth> ();
            }
        }

        void Update ()
        {
            if (!isGameOver && playerHealth != null && playerHealth.currentHealth <= 0)
            {
                isGameOver = true;

                // Evalúa y guarda el puntaje si superó al récord anterior
                //ScoreManager.CheckAndSaveHighScore();

                if (anim != null)
                {
                    anim.SetTrigger ("GameOver");
                }
            }
        }
    }
}

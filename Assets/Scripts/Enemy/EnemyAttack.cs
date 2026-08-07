using UnityEngine;
using System.Collections;

namespace CompleteProject
{
    public class EnemyAttack : MonoBehaviour
    {
        public float timeBetweenAttacks = 0.5f;     // Tiempo en segundos entre ataques
        public int attackDamage = 10;               // Daño por ataque

        [Header("Pausa Post-Ataque")]
        [Tooltip("Tiempo de espera para volver a moverse tras perder al player de rango")]
        public float postAttackMoveDelay = 1.5f;

        Animator anim;                              // Referencia al Animator
        GameObject player;                          // Referencia al GameObject del Player
        PlayerHealth playerHealth;                  // Referencia a la salud del jugador
        EnemyHealth enemyHealth;                    // Referencia a la salud del enemigo
        EnemyMovement enemyMovement;                // Referencia al script de movimiento
        bool playerInRange;                         // Estado de rango
        float timer;                                // Temporizador de ataque


        void Awake ()
        {
            player = GameObject.FindGameObjectWithTag ("Player");
            playerHealth = player.GetComponent <PlayerHealth> ();
            enemyHealth = GetComponent<EnemyHealth>();
            enemyMovement = GetComponent<EnemyMovement>();
            anim = GetComponent <Animator> ();
            if(playerHealth == null)
            {
                Debug.Log("no hay jugador");
            }
        }


        void OnTriggerEnter (Collider other)
        {
            if(other.gameObject == player)
            {
                playerInRange = true;
            }
        }


        void OnTriggerExit (Collider other)
        {
            if(other.gameObject == player)
            {
                playerInRange = false;

                // Al salir del rango, le indica al script de movimiento que espere antes de perseguir de nuevo
                if (enemyMovement != null && enemyHealth.currentHealth > 0)
                {
                    enemyMovement.ApplyAttackCooldown(postAttackMoveDelay);
                }
            }
        }


        void Update ()
        {
            timer += Time.deltaTime;

            if(timer >= timeBetweenAttacks && playerInRange && enemyHealth.currentHealth > 0)
            {
                Attack ();
            }

            if(playerHealth.currentHealth <= 0)
            {
                anim.SetTrigger ("PlayerDead");
            }
        }


        void Attack ()
        {
            timer = 0f;

            if(playerHealth.currentHealth > 0)
            {
                playerHealth.TakeDamage (attackDamage);
            }
        }
    }
}

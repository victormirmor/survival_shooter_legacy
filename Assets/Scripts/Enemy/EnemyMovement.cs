using UnityEngine;
using System.Collections;

namespace CompleteProject
{
    public class EnemyMovement : MonoBehaviour
    {
        Transform player;               // Referencia a la posición del jugador
        PlayerHealth playerHealth;      // Referencia a la salud del jugador
        EnemyHealth enemyHealth;        // Referencia a la salud de este enemigo
        UnityEngine.AI.NavMeshAgent nav;// Referencia al NavMeshAgent
        Animator anim;                  // Referencia al Animator

        [Header("Configuración de Velocidad")]
        [Tooltip("Velocidad de movimiento de este prefab específico")]
        public float speed = 3.5f;

        [Header("Variación de Ruta / Dispersión")]
        [Tooltip("Radio de variación alrededor del player para evitar amontonamientos")]
        public float targetOffsetRadius = 1.5f;

        [Tooltip("Cada cuántos segundos recalculamos la variación del destino")]
        public float recalculateInterval = 1.0f;

        [Header("Mecánica de Pausa / Duda")]
        [Tooltip("Tiempo mínimo entre pausas de duda")]
        public float minTimeBetweenPauses = 5f;
        [Tooltip("Tiempo máximo entre pausas de duda")]
        public float maxTimeBetweenPauses = 10f;
        [Tooltip("Duración de la animación/espera de duda")]
        public float pauseDuration = 2f;

        private Vector3 currentOffset;
        private float nextRecalculateTime;
        private float pauseTimer;
        private bool isPausing = false;
        private bool isCoolingDownFromAttack = false;

        // Propiedad helper para verificar la integridad del NavMeshAgent
        private bool IsAgentActiveAndOnNavMesh => nav != null && nav.enabled && nav.isOnNavMesh;

        void Awake ()
        {
            // Configurar referencias
            player = GameObject.FindGameObjectWithTag ("Player").transform;
            playerHealth = player.GetComponent <PlayerHealth> ();
            enemyHealth = GetComponent <EnemyHealth> ();
            nav = GetComponent <UnityEngine.AI.NavMeshAgent> ();
            anim = GetComponent <Animator> ();
        }

        void Start()
        {
            if (IsAgentActiveAndOnNavMesh)
            {
                nav.speed = speed;
            }

            GenerateNewOffset();
            ScheduleNextPause();
        }

        void Update ()
        {
            // Si el enemigo o el jugador murieron, aseguramos desactivar el agente
            if (enemyHealth.currentHealth <= 0 || playerHealth.currentHealth <= 0)
            {
                if (nav != null && nav.enabled)
                {
                    nav.enabled = false;
                }
                return;
            }

            if (!IsAgentActiveAndOnNavMesh) return;

            // Si está pausado o en cooldown de ataque, frenar el agente
            if (isPausing || isCoolingDownFromAttack)
            {
                nav.isStopped = true;
                return;
            }

            nav.isStopped = false;

            // Control del ciclo de pausa/duda
            pauseTimer -= Time.deltaTime;
            if (pauseTimer <= 0f)
            {
                StartCoroutine(DoubtRoutine());
                return;
            }

            // Recalcular la variación de destino periódicamente
            if (Time.time >= nextRecalculateTime)
            {
                GenerateNewOffset();
                nextRecalculateTime = Time.time + recalculateInterval;
            }

            // Definir el destino con el offset aplicado
            Vector3 targetDestination = player.position + currentOffset;
            nav.SetDestination (targetDestination);
        }

        void GenerateNewOffset()
        {
            Vector2 randomCircle = Random.insideUnitCircle * targetOffsetRadius;
            currentOffset = new Vector3(randomCircle.x, 0f, randomCircle.y);
        }

        void ScheduleNextPause()
        {
            pauseTimer = Random.Range(minTimeBetweenPauses, maxTimeBetweenPauses);
        }

        IEnumerator DoubtRoutine()
        {
            isPausing = true;

            if (IsAgentActiveAndOnNavMesh)
            {
                nav.isStopped = true;
            }

            if (anim != null)
            {
                //anim.SetBool("IsDoubting", true);
            }

            yield return new WaitForSeconds(pauseDuration);

            if (anim != null)
            {
                //anim.SetBool("IsDoubting", false);
            }

            isPausing = false;

            // Verificación previa a reactivar la marcha
            if (IsAgentActiveAndOnNavMesh)
            {
                nav.isStopped = false;
            }

            ScheduleNextPause();
        }

        /// <summary>
        /// Llamado por EnemyAttack cuando el jugador sale del rango tras atacar.
        /// </summary>
        public void ApplyAttackCooldown(float cooldownTime)
        {
            if (gameObject.activeInHierarchy)
            {
                StartCoroutine(AttackCooldownRoutine(cooldownTime));
            }
        }

        IEnumerator AttackCooldownRoutine(float cooldownTime)
        {
            isCoolingDownFromAttack = true;

            if (IsAgentActiveAndOnNavMesh)
            {
                nav.isStopped = true;
            }

            yield return new WaitForSeconds(cooldownTime);

            isCoolingDownFromAttack = false;

            if (IsAgentActiveAndOnNavMesh)
            {
                nav.isStopped = false;
            }
        }
    }
}

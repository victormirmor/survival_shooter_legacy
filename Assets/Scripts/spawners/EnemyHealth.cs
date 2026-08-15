using UnityEngine;
using UnityEngine.AI;

    public class EnemyHealth : MonoBehaviour
    {
        [Header("Configuración de Salud y Puntos")]
        public int startingHealth = 100;
        public int currentHealth;
        public int scoreValue = 10;
        public float sinkSpeed = 2.5f;

        [Header("Audio y Efectos")]
        public AudioClip deathClip;

        private Animator anim;
        private AudioSource enemyAudio;
        private ParticleSystem hitParticles;
        private CapsuleCollider capsuleCollider;
        private NavMeshAgent navAgent;
        private Rigidbody rb;

        private bool isDead;
        private bool isSinking;

        void Awake()
        {
            anim = GetComponent<Animator>();
            enemyAudio = GetComponent<AudioSource>();
            hitParticles = GetComponentInChildren<ParticleSystem>();
            capsuleCollider = GetComponent<CapsuleCollider>();
            navAgent = GetComponent<NavMeshAgent>();
            rb = GetComponent<Rigidbody>();

            currentHealth = startingHealth;
        }

        void Update()
        {
            if (isSinking)
            {
                transform.Translate(-Vector3.up * sinkSpeed * Time.deltaTime);
            }
        }

        public void TakeDamage(int amount, Vector3 hitPoint)
        {
            if (isDead) return;

            currentHealth -= amount;

            if (enemyAudio != null && enemyAudio.clip != null)
            {
                enemyAudio.Play();
            }

            if (hitParticles != null)
            {
                hitParticles.transform.position = hitPoint;
                hitParticles.Play();
            }

            if (currentHealth <= 0)
            {
                Death();
            }
        }

        void Death()
        {
            if (isDead) return;
            isDead = true;

            // 1. Desactivar colisión y navegación inmediatamente
            if (capsuleCollider != null)
            {
                capsuleCollider.isTrigger = true;
            }

            if (navAgent != null && navAgent.enabled)
            {
                navAgent.enabled = false;
            }

            // 2. Disparar animación y sonido
            if (anim != null)
            {
                anim.SetTrigger("Dead");
            }

            if (enemyAudio != null && deathClip != null)
            {
                enemyAudio.clip = deathClip;
                enemyAudio.Play();
            }

            // 3. Sumar puntos en GameManager
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddPoints(scoreValue);
            }

            // 4. Notificar al EnemySpawn para actualizar la UI y las oleadas
            //EnemySpawn enemySpawner = FindObjectOfType<EnemySpawn>();
           /* if (enemySpawner != null)
            {
                enemySpawner.OnEnemyDied();
            }*/

            Destroy(gameObject, 2.5f);
        }

        public void StartSinking()
        {
            if (navAgent != null)
            {
                navAgent.enabled = false;
            }

            if (rb != null)
            {
                rb.isKinematic = true;
            }

            isSinking = true;
        }
}

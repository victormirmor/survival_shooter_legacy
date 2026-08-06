using System.Collections;
using UnityEngine;

namespace CompleteProject
{
    public class EnemySpawn : MonoBehaviour
    {
        public PlayerHealth playerHealth;       // Referencia a la salud del jugador
        
        [Header("Configuración de Enemigos")]
        public GameObject[] enemies;           // Prefabs de enemigos
        public Transform[] spawnPoints;         // Puntos de aparición en el mapa

        [Header("Tiempos de Spawn")][Range(1f, 15f)]
        public float initialTimeBetweenWaves = 10f;
        [Range(1f, 15f)] public float timeBetweenWaves = 4f,spawnDelay = 1.5f;     // Tiempo de descanso entre oleadas subsecuentes (2, 3, 4...)

        [Header("Configuración de Oleadas")]
        [Range(1, 50)]
        public int initialWaveEnemies = 10;     // Primera oleada: 10 enemigos

        [Range(1, 20)]
        public int enemiesPerWaveIncrease = 5;  // Incremento por oleada (+5)

        [Header("Estado Actual (Debug)")]
        public int currentWave = 0;
        public int totalEnemiesInWave;
        public int enemiesSpawnedSoFar;
        public int enemiesAlive;

        private bool isSpawningWave = false;

        void Start()
        {
            currentWave = 0;
            StartCoroutine(StartFirstWaveWithDelay());
        }

        void Update()
        {
            // Si la oleada terminó de aparecer y ya no quedan enemigos vivos, arranca la siguiente
            if (!isSpawningWave && enemiesAlive <= 0 && playerHealth != null && playerHealth.currentHealth > 0f)
            {
                StartCoroutine(WaitAndStartNextWave());
            }
        }

        /// <summary>
        /// Corrutina de tiempo de gracia previo a la Oleada 1.
        /// </summary>
        IEnumerator StartFirstWaveWithDelay()
        {
            isSpawningWave = true;

            // 1. Spawnea la munición inicial en el mapa
            BulletSpawner ammoSpawner = FindObjectOfType<BulletSpawner>();
            if (ammoSpawner != null)
            {
                ammoSpawner.SpawnWaveAmmo();
            }

            // 2. Notifica a la UI usando la variable independiente 'initialTimeBetweenWaves'
            WaveUI waveUI = FindObjectOfType<WaveUI>(true);
            float remainingTime = initialTimeBetweenWaves;

            while (remainingTime > 0)
            {
                if (waveUI != null)
                {
                    waveUI.ShowInitialCountdown(Mathf.CeilToInt(remainingTime));
                }

                yield return new WaitForSeconds(1f);
                remainingTime -= 1f;
            }

            isSpawningWave = false;

            // 3. Arranca oficialmente la Oleada 1
            StartNextWave();
        }

        void StartNextWave()
        {
            currentWave++;
            enemiesSpawnedSoFar = 0;
            
            totalEnemiesInWave = initialWaveEnemies + (currentWave - 1) * enemiesPerWaveIncrease;
            enemiesAlive = totalEnemiesInWave;

            Debug.Log($"<color=cyan><b>--- INICIANDO OLEADA {currentWave} ---</b></color> Total enemigos: {totalEnemiesInWave}");

            // --- AVISO A LA UI DE OLEADAS ---
            WaveUI waveUI = FindObjectOfType<WaveUI>(true);
            if (waveUI != null)
            {
                waveUI.ShowWave(currentWave);
            }

            // Spawnea munición adicional si es de la oleada 2 en adelante
            if (currentWave > 1)
            {
                BulletSpawner ammoSpawner = FindObjectOfType<BulletSpawner>();
                if (ammoSpawner != null)
                {
                    ammoSpawner.SpawnWaveAmmo();
                }
            }

            StartCoroutine(SpawnWaveRoutine());
        }

        IEnumerator WaitAndStartNextWave()
        {
            isSpawningWave = true;
            Debug.Log($"<b>¡Oleada {currentWave} completada!</b> Próxima oleada en {timeBetweenWaves} segundos...");
            yield return new WaitForSeconds(timeBetweenWaves);
            isSpawningWave = false;
            StartNextWave();
        }

        IEnumerator SpawnWaveRoutine()
        {
            isSpawningWave = true;

            while (enemiesSpawnedSoFar < totalEnemiesInWave)
            {
                if (playerHealth != null && playerHealth.currentHealth <= 0f)
                {
                    yield break;
                }

                SpawnOneEnemy();
                enemiesSpawnedSoFar++;

                yield return new WaitForSeconds(spawnDelay);
            }

            isSpawningWave = false;
        }

        void SpawnOneEnemy()
        {
            if (enemies == null || enemies.Length == 0 || spawnPoints == null || spawnPoints.Length == 0)
                return;

            int enemyIndex = Random.Range(0, enemies.Length);
            int spawnPointIndex = Random.Range(0, spawnPoints.Length);

            Instantiate(
                enemies[enemyIndex],
                spawnPoints[spawnPointIndex].position,
                spawnPoints[spawnPointIndex].rotation
            );
        }

        public void OnEnemyDied()
        {
            enemiesAlive--;
            Debug.Log($"Enemigo eliminado. Quedan vivos: {enemiesAlive}/{totalEnemiesInWave}");
        }
    }
}

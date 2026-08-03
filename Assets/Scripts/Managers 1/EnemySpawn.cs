using System.Collections;
using UnityEngine;

namespace CompleteProject
{
    public class EnemySpawn : MonoBehaviour
    {
        public PlayerHealth playerHealth;       // Referencia a la salud del jugador[cite: 1]
        
        [Header("Configuración de Enemigos")]
        public GameObject[] enemies;           // Prefabs de enemigos[cite: 1]
        public Transform[] spawnPoints;         // Puntos de aparición en el mapa[cite: 1]

        [Header("Tiempos de Spawn")]
        [Range(0.1f, 5f)]
        public float spawnDelay = 1.5f;         // Tiempo entre cada enemigo dentro de una misma oleada

        [Range(1f, 15f)]
        public float timeBetweenWaves = 4f;     // Tiempo de descanso entre oleadas

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
            StartNextWave();
        }

        void Update()
        {
            // Si la oleada terminó de aparecer y ya no quedan enemigos vivos, arranca la siguiente
            if (!isSpawningWave && enemiesAlive <= 0 && playerHealth != null && playerHealth.currentHealth > 0f)
            {
                StartCoroutine(WaitAndStartNextWave());
            }
        }

        void StartNextWave()
        {
            currentWave++;
            enemiesSpawnedSoFar = 0;
            
            // Oleada 1 = 10, Oleada 2 = 15, Oleada 3 = 20...
            totalEnemiesInWave = initialWaveEnemies + (currentWave - 1) * enemiesPerWaveIncrease;
            enemiesAlive = totalEnemiesInWave;

            Debug.Log($"<color=cyan><b>--- INICIANDO OLEADA {currentWave} ---</b></color> Total enemigos: {totalEnemiesInWave}");
            // --- AVISO A LA UI DE OLEADAS ---
                WaveUI waveUI = FindObjectOfType<WaveUI>(true);
                if (waveUI != null){
                waveUI.ShowWave(currentWave);
                }

                        // --- GENERACIÓN DE MUNICIÓN DE LA OLEADA ---
                BulletSpawner ammoSpawner = FindObjectOfType<BulletSpawner>();
                if (ammoSpawner != null)
                {
                    ammoSpawner.SpawnWaveAmmo();
                }
                // --------------------------------

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

        /// <summary>
        /// Método público llamado por el enemigo cuando muere.
        /// </summary>
        public void OnEnemyDied()
        {
            enemiesAlive--;
            Debug.Log($"Enemigo eliminado. Quedan vivos: {enemiesAlive}/{totalEnemiesInWave}");
        }
    }
}
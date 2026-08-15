using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CompleteProject
{
    public class EnemySpawn : MonoBehaviour
    {
        [Header("Puntos de Aparición")]
        public Transform[] spawnPoints;

        [Header("Tiempos de Spawn")]
        [Range(1f, 15f)] public float initialTimeBetweenWaves = 5f;
        [Range(1f, 15f)] public float timeBetweenWaves = 4f;
        [Range(0.1f, 5f)] public float spawnDelay = 1.0f;

        [Header("Configuración de Oleadas")]
        [Range(1, 50)] public int initialWaveEnemies = 5;
        [Range(1, 20)] public int enemiesPerWaveIncrease = 3;

        [Header("Estado Actual (Debug)")]
        public int currentWave = 0;
        public int totalEnemiesInWave;
        public int enemiesSpawnedSoFar;
        public int enemiesAlive;

        private bool isSpawningWave = false;
        private bool isWaitingForNextWave = false;

        private void Start()
        {
            currentWave = 0;
            StartCoroutine(StartFirstWaveRoutine());
        }

        private IEnumerator StartFirstWaveRoutine()
        {
            isWaitingForNextWave = true;
            Debug.Log($"<b>[EnemySpawn]</b> Primera oleada inicia en {initialTimeBetweenWaves} segundos...");
            yield return new WaitForSeconds(initialTimeBetweenWaves);

            isWaitingForNextWave = false;
            StartNextWave();
        }

        private void StartNextWave()
        {
            currentWave++;
            enemiesSpawnedSoFar = 0;

            totalEnemiesInWave = initialWaveEnemies + (currentWave - 1) * enemiesPerWaveIncrease;
            enemiesAlive = totalEnemiesInWave;

            Debug.Log($"<color=cyan><b>--- INICIANDO OLEADA {currentWave} ---</b></color> Total: {totalEnemiesInWave} enemigos.");

            StartCoroutine(SpawnWaveRoutine());
        }

        private IEnumerator SpawnWaveRoutine()
        {
            isSpawningWave = true;

            while (enemiesSpawnedSoFar < totalEnemiesInWave)
            {
                // Pedimos la instancia inactiva directamente al EnemyPool
                GameObject enemyToSpawn = null;
                if (EnemyPool.Instance != null)
                {
                    enemyToSpawn = EnemyPool.Instance.GetEnemyFromPool();
                }

                if (enemyToSpawn != null)
                {
                    if (spawnPoints != null && spawnPoints.Length > 0)
                    {
                        int spawnPointIndex = Random.Range(0, spawnPoints.Length);
                        Transform chosenPoint = spawnPoints[spawnPointIndex];

                        enemyToSpawn.transform.position = chosenPoint.position;
                        enemyToSpawn.transform.rotation = chosenPoint.rotation;
                    }

                    EnemyHealth health = enemyToSpawn.GetComponent<EnemyHealth>();
                    if (health != null)
                    {
                        //health.ResetHealth();
                    }

                    enemyToSpawn.SetActive(true);
                    enemiesSpawnedSoFar++;

                    yield return new WaitForSeconds(spawnDelay);
                }
                else
                {
                    // Si el Pool está lleno (todos los enemigos del Pool activos en pantalla), espera al siguiente frame
                    yield return null;
                }
            }

            isSpawningWave = false;
        }

        public void OnEnemyDied()
        {
            enemiesAlive--;
            if (enemiesAlive < 0) enemiesAlive = 0;

            Debug.Log($"Enemigo eliminado. Restantes en la oleada: {enemiesAlive}/{totalEnemiesInWave}");

            if (enemiesAlive == 0 && !isSpawningWave && !isWaitingForNextWave)
            {
                StartCoroutine(WaitAndStartNextWaveRoutine());
            }
        }

        private IEnumerator WaitAndStartNextWaveRoutine()
        {
            isWaitingForNextWave = true;
            Debug.Log($"<b>¡Oleada {currentWave} completada!</b> Próxima en {timeBetweenWaves}s...");

            yield return new WaitForSeconds(timeBetweenWaves);

            isWaitingForNextWave = false;
            StartNextWave();
        }

        public void ClearAllEnemies()
        {
            StopAllCoroutines();
            isSpawningWave = false;
            isWaitingForNextWave = false;

            if (EnemyPool.Instance != null)
            {
                EnemyPool.Instance.DeactivateAllEnemies();
            }

            enemiesAlive = 0;
            Debug.Log("<color=orange><b>[EnemySpawn]</b> Todos los enemigos fueron desactivados a través del Pool.</color>");
        }
    }
}
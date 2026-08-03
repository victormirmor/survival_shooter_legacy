using System.Collections.Generic;
using UnityEngine;

namespace CompleteProject
{
    public class BulletSpawner : MonoBehaviour
    {
        [Header("Variantes de Cargadores (Prefabs)")]
        public GameObject[] ammoPickupPrefabs;   // Array con las distintas variantes de cargadores (Normal, Crítico, etc.)

        [Header("Puntos de Aparición")]
        public Transform[] ammoSpawnPoints;      // Nodos/posiciones distribuidas en el escenario

        [Header("Configuración por Oleada")]
        [Range(1, 10)]
        public int minPickupsPerWave = 3;
        [Range(1, 10)]
        public int maxPickupsPerWave = 4;

        private List<GameObject> activePickups = new List<GameObject>();

        /// <summary>
        /// Genera cargadores aleatorios con variantes de munición al iniciar la oleada.
        /// </summary>
        public void SpawnWaveAmmo()
        {
            // 1. Destruye los cargadores de la oleada anterior que no fueron recogidos
            ClearOldPickups();

            if (ammoPickupPrefabs == null || ammoPickupPrefabs.Length == 0 || ammoSpawnPoints == null || ammoSpawnPoints.Length == 0)
            {
                Debug.LogWarning("[BulletSpawner] Faltan asignaciones en el Inspector (Prefabs de cargadores o Spawn Points).");
                return;
            }

            // 2. Definir cantidad de cargadores a generar
            int pickupsToSpawn = Random.Range(minPickupsPerWave, maxPickupsPerWave + 1);

            // Lista auxiliar para no repetir puntos de aparición en la misma oleada
            List<int> availablePoints = new List<int>();
            for (int i = 0; i < ammoSpawnPoints.Length; i++)
            {
                availablePoints.Add(i);
            }

            // 3. Generar los cargadores en posiciones únicas
            for (int i = 0; i < pickupsToSpawn; i++)
            {
                if (availablePoints.Count == 0) break;

                // Selección de Spawn Point aleatorio sin repetir
                int randomPointIndex = Random.Range(0, availablePoints.Count);
                int spawnPointIndex = availablePoints[randomPointIndex];
                availablePoints.RemoveAt(randomPointIndex);

                // Selección aleatoria del tipo de cargador (variante de crítico/bala)
                int randomPrefabIndex = Random.Range(0, ammoPickupPrefabs.Length);
                GameObject selectedPrefab = ammoPickupPrefabs[randomPrefabIndex];

                // Instanciación
                Transform spawnTransform = ammoSpawnPoints[spawnPointIndex];
                GameObject newPickup = Instantiate(selectedPrefab, spawnTransform.position, spawnTransform.rotation);

                activePickups.Add(newPickup);
            }

            Debug.Log($"<color=green><b>[AMMO SPAWN]</b> Se instanciaron {activePickups.Count} cargadores aleatorios en el mapa.</color>");
        }

        private void ClearOldPickups()
        {
            foreach (GameObject pickup in activePickups)
            {
                if (pickup != null)
                {
                    Destroy(pickup);
                }
            }
            activePickups.Clear();
        }
    }
}

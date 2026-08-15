using System.Collections.Generic;
using UnityEngine;

namespace CompleteProject
{
    public class EnemyPool : MonoBehaviour
    {
        public static EnemyPool Instance { get; private set; }

        [Header("Configuración del Pool")]
        public GameObject[] enemyPrefabs;
        public int poolSize = 10;

        private List<GameObject> pool = new List<GameObject>();

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);

            InitializePool();
        }

        private void InitializePool()
        {
            if (enemyPrefabs == null || enemyPrefabs.Length == 0) return;

            for (int i = 0; i < poolSize; i++)
            {
                // Seleccionar prefab aleatorio del arreglo
                int randomIndex = Random.Range(0, enemyPrefabs.Length);
                GameObject enemy = Instantiate(enemyPrefabs[randomIndex], transform);
                
                enemy.SetActive(false);
                pool.Add(enemy);
            }
        }

        public GameObject GetEnemyFromPool()
        {
            foreach (GameObject enemy in pool)
            {
                if (!enemy.activeInHierarchy)
                {
                    return enemy;
                }
            }
            // Retorna null si los 10 están vivos en la escena
            return null; 
        }

        public void DeactivateAllEnemies()
        {
            foreach (GameObject enemy in pool)
            {
                if (enemy != null)
                {
                    enemy.SetActive(false);
                }
            }
        }
    }
}
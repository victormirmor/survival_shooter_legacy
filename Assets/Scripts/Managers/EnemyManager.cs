using UnityEngine;

namespace CompleteProject
{
    public class EnemyManager : MonoBehaviour
    {
        public PlayerHealth playerHealth;       // Referencia a la salud del jugador
        
        [Header("Configuración de Enemigos")]
        public GameObject[] enemies;           // Array con los distintos prefabs de enemigos
        public float spawnTime = 3f;            // Tiempo entre apariciones

        [Header("Puntos de Aparición")]
        public Transform[] spawnPoints;         // Array con los puntos de origen en el mapa

        void Start()
        {
            // Inicia el ciclo repetitivo de apariciones
            InvokeRepeating(nameof(Spawn), spawnTime, spawnTime);
        }

        void Spawn()
        {
            // Si el jugador murió, se detiene el spawn
            if (playerHealth != null && playerHealth.currentHealth <= 0f)
            {
                return;
            }

            // Validaciones de seguridad para evitar errores en la consola
            if (enemies == null || enemies.Length == 0)
            {
                Debug.LogWarning("[EnemyManager] No hay prefabs de enemigos asignados en el array 'enemies'.");
                return;
            }

            if (spawnPoints == null || spawnPoints.Length == 0)
            {
                Debug.LogWarning("[EnemyManager] No hay puntos de aparición asignados en 'spawnPoints'.");
                return;
            }

            // 1. Selección aleatoria del enemigo
            int enemyIndex = Random.Range(0, enemies.Length);

            // 2. Selección aleatoria del spawn point
            int spawnPointIndex = Random.Range(0, spawnPoints.Length);

            // 3. Instanciación del enemigo seleccionado en el punto elegido
            Instantiate(
                enemies[enemyIndex], 
                spawnPoints[spawnPointIndex].position, 
                spawnPoints[spawnPointIndex].rotation
            );

            Debug.Log($"[EnemyManager] Spawned: {enemies[enemyIndex].name} en {spawnPoints[spawnPointIndex].name}");
        }
    }
}
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [Header("Configuración de Munición y Cadencia")]
    public int Bullets_rest = 20;                  // Balas disponibles para disparar
    public float timeBetweenBullets = 0.15f;        // Cadencia de tiro
    public GameObject bulletPrefab;                 // Prefab del proyectil
    public Transform gunTip;                        // Punto de salida del arma

    [Header("Efectos Visuales Auxiliares")]
    public Light faceLight;                         // Luz auxiliar

    private float timer;                            // Temporizador para la cadencia
    private AudioSource gunAudio;                   // Sonido de disparo
    private Light gunLight;                         // Luz del fogonazo
    private float effectsDisplayTime = 0.2f;        // Duración visual del fogonazo

    void Awake()
    {
        gunAudio = GetComponent<AudioSource>();
        gunLight = GetComponent<Light>();

        Debug.Log($"<color=cyan>[PlayerShooting] Inicializado en {gameObject.name}. Balas iniciales: {Bullets_rest} | Prefab Asignado: {(bulletPrefab != null ? bulletPrefab.name : "NULL")} | GunTip: {(gunTip != null ? gunTip.name : "NULL")}</color>");
    }

    void Update()
    {
        timer += Time.deltaTime;

        // 1. Lectura directa desde la acción lógica expuesta por InputDataMap
        bool isShootingPressed = InputDataMap.Instance != null && InputDataMap.Instance.actionShoot;

        if (isShootingPressed)
        {
            Debug.Log($"<color=yellow>[PlayerShooting Debug] Acción 'actionShoot' activa | Timer: {timer:F2}/{timeBetweenBullets} | TimeScale: {Time.timeScale} | Balas restantes: {Bullets_rest}</color>");

            // Validar escala de tiempo (juego pausado)
            if (Time.timeScale == 0f)
            {
                Debug.LogWarning("<color=orange>[PlayerShooting Bloqueado] Disparo cancelado: El juego está en Pausa (Time.timeScale = 0).</color>");
                return;
            }

            // Validar munición
            if (Bullets_rest <= 0)
            {
                Debug.LogWarning("<color=red>[PlayerShooting Sin Munición] Intentaste disparar pero Bullets_rest es 0 o menor.</color>");
                return;
            }

            // Validar cadencia de tiro
            if (timer < timeBetweenBullets)
            {
                Debug.Log($"[PlayerShooting Cadencia] Esperando enfriamiento... (Faltan {timeBetweenBullets - timer:F2}s)");
                return;
            }

            // Si pasa todas las validaciones, dispara
            Shoot();
            Bullets_rest--;
        }

        if (timer >= timeBetweenBullets * effectsDisplayTime)
        {
            DisableEffects();
        }
    }

    public void DisableEffects()
    {
        if (gunLight != null) gunLight.enabled = false;
        if (faceLight != null) faceLight.enabled = false;
    }

    void Shoot()
    {
        timer = 0f; // Reinicia el temporizador de cadencia

        Debug.Log($"<color=green><b>[DISPARO EXITOSO]</b> Instanciando proyectil. Balas restantes tras disparo: {Bullets_rest}</color>");

        // Efectos visuales y de sonido
        if (gunAudio != null) gunAudio.Play();
        if (gunLight != null) gunLight.enabled = true;
        if (faceLight != null) faceLight.enabled = true;

        // Validar referencias de instanciación
        if (bulletPrefab == null)
        {
            Debug.LogError("<color=red><b>[ERROR CRÍTICO]</b> No se puede instanciar la bala: 'bulletPrefab' es NULL en el Inspector de PlayerShooting.</color>");
            return;
        }

        if (gunTip == null)
        {
            Debug.LogError("<color=red><b>[ERROR CRÍTICO]</b> No se puede instanciar la bala: 'gunTip' es NULL en el Inspector de PlayerShooting.</color>");
            return;
        }

        Instantiate(bulletPrefab, gunTip.position, gunTip.rotation);
    }

    public void ReloadWeapon(GameObject newBulletPrefab, int amount)
    {
        if (newBulletPrefab != null)
        {
            bulletPrefab = newBulletPrefab;
        }

        Bullets_rest += amount;
        Debug.Log($"<color=cyan>[PlayerShooting Recarga] Arma recargada con +{amount} balas. Total disponible: {Bullets_rest}</color>");
    }
}
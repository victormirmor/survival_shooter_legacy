using UnityEngine;

public class FrameRateLimiter : MonoBehaviour
{
    [Header("Configuración de FPS")]
    [SerializeField] private int targetFPS = 60;

    private void Awake()
    {
        // 1. Desactivar VSync para permitir el límite manual
        QualitySettings.vSyncCount = 0;

        // 2. Establecer el límite de cuadros por segundo
        Application.targetFrameRate = targetFPS;
    }
}
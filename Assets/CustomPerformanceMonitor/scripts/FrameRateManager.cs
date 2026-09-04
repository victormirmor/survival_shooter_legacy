using UnityEngine;
using System.Diagnostics;

public class FrameRateManager : MonoBehaviour
{
    private void Awake()
    {
        // Configuración por defecto para móviles y PC
        #if UNITY_IOS || UNITY_ANDROID
            SetFrameRate(60); // Evita gastar batería excesiva
        #else
            SetFrameRateFromDropdown(0); // Opción: VSync Activado por defecto
        #endif
    }

    public void SetFrameRateFromDropdown(int dropdownIndex)
    {
        switch (dropdownIndex)
        {
            case 0: // Opción: VSync Activado (Ideal para WebGL o monitores de 75Hz/144Hz)
                QualitySettings.vSyncCount = 1;
                Application.targetFrameRate = -1;
                LogEditor("Opción: VSync Activado");
                break;

            case 1: // Opción: 30 FPS (Ahorro de batería en móviles / PC gama baja)
                QualitySettings.vSyncCount = 0;
                Application.targetFrameRate = 30;
                LogEditor("Opción: 30 FPS");
                break;

            case 2: // Opción: 60 FPS (Bajo input lag)
                QualitySettings.vSyncCount = 0;
                Application.targetFrameRate = 60;
                LogEditor("Opción: 60 FPS");
                break;

            case 3: // Opción: Sin límite / Max Rendimiento
                QualitySettings.vSyncCount = 0;
                Application.targetFrameRate = -1;
                LogEditor("Opción: sin limites");
                break;

            default:
                QualitySettings.vSyncCount = 1;
                Application.targetFrameRate = -1;
                break;
        }
    }

    public void SetFrameRate(int fps)
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = fps;
    }

    /// <summary>
    /// Registra mensajes en la consola de Unity únicamente durante la ejecución dentro del Editor.
    /// Este método y sus llamadas asociadas se remueven de forma automática durante el proceso de Build.
    /// </summary>
    [Conditional("UNITY_EDITOR")]
    private void LogEditor(string message)
    {
        UnityEngine.Debug.Log(message);
    }
}
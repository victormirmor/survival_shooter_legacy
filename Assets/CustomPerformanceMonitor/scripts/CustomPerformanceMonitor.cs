using UnityEngine;
using TMPro;
using UnityEngine.Profiling;

public class CustomPerformanceMonitor : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI dynamicText;
    [SerializeField] private TextMeshProUGUI staticSpecsText;

    [Header("Settings")]
    [SerializeField] private float updateInterval = 0.5f;

    private float accumulatedTime = 0f;
    private int accumulatedFrames = 0;
    private float lastUpdateTime = 0f;

    private float minFrameTime = float.MaxValue;
    private float maxFrameTime = float.MinValue;

    private void Start()
    {
        ShowStaticSpecs();
        lastUpdateTime = Time.realtimeSinceStartup;
    }

    private void ShowStaticSpecs()
    {
        if (staticSpecsText == null) return;

        staticSpecsText.text = 
            $"<b>GPU:</b> <color=#FF5020>{SystemInfo.graphicsDeviceName}</color> [{SystemInfo.graphicsDeviceType}]\n" +
            $"<b>VRAM:</b> <color=#FF5020>{SystemInfo.graphicsMemorySize} MB</color>\n" +
            $"<b>CPU:</b> <color=#0090CB>{SystemInfo.processorType}</color> ({SystemInfo.processorCount} Cores)\n" +
            $"<b>RAM Sistema:</b> <color=#0090CB>{SystemInfo.systemMemorySize} MB</color>\n" +
            $"<b>SO:</b> <color=#C9D700>{SystemInfo.operatingSystem}</color>";
    }

    private void Update()
    {
        float deltaTime = Time.unscaledDeltaTime;

        accumulatedTime += deltaTime;
        accumulatedFrames++;

        if (deltaTime < minFrameTime) minFrameTime = deltaTime;
        if (deltaTime > maxFrameTime) maxFrameTime = deltaTime;

        float now = Time.realtimeSinceStartup;
        if (now - lastUpdateTime >= updateInterval)
        {
            float avgFrameTime = accumulatedTime / Mathf.Max(1, accumulatedFrames);
            float avgFPS = 1.0f / avgFrameTime;

            float minFPS = 1.0f / maxFrameTime;
            float maxFPS = 1.0f / minFrameTime;
            float fpsFluctuation = Mathf.Abs(maxFPS - minFPS) / 2.0f;

            // Obtener datos de memoria de Unity en MB
            float ramUsedMb = Profiler.GetTotalAllocatedMemoryLong() / (1024f * 1024f);
            float ramReservedMb = Profiler.GetTotalReservedMemoryLong() / (1024f * 1024f);

            if (dynamicText != null)
            {
                dynamicText.text = 
                    $"<b>FPS:</b> <color=#80FF00>{avgFPS:F1}</color> ({avgFrameTime * 1000f:F1} ms)\n" +
                    $"<b>Min / Max:</b> <color=#FF8400>{minFPS:F1}</color> | <color=#00A0FF>{maxFPS:F1} FPS</color>\n" +
                    $"<b>Estabilidad (~):</b> <color=#DCEC00>±{fpsFluctuation:F1} FPS</color>\n" +
                    $"<b>RAM Usada:</b> <color=#00E5FF>{ramUsedMb:F0} MB</color> / <b>Reservada:</b> <color=#FFB300>{ramReservedMb:F0} MB</color>";
            }

            // Resetear datos para el siguiente intervalo
            minFrameTime = float.MaxValue;
            maxFrameTime = float.MinValue;
            accumulatedTime = 0f;
            accumulatedFrames = 0;
            lastUpdateTime = now;
        }
    }
}

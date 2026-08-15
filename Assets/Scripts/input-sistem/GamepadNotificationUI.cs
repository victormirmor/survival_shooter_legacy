using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GamepadNotificationUI : MonoBehaviour
{
    [Header("Referencias de UI")]
    //[SerializeField] private GameObject notificationPanel;
    [SerializeField] private Text notificationText;

    [Header("Configuración de Cartel")]
    [SerializeField] private float displayDuration = 3f;

    private int lastJoystickCount = 0;
    private Coroutine hideCoroutine;

    private void OnEnable()
    {
        InputDataMap.OnControllerStatusChanged += HandleControllerStatusChanged;
        InputDataMap.OnInputDeviceChanged += HandleInputDeviceChanged;
    }

    private void OnDisable()
    {
        InputDataMap.OnControllerStatusChanged -= HandleControllerStatusChanged;
        InputDataMap.OnInputDeviceChanged -= HandleInputDeviceChanged;
    }

    private void Awake()
    {
        notificationText.text="";
    }

    private void HandleControllerStatusChanged(int currentJoystickCount, string deviceName)
    {
        if (currentJoystickCount > lastJoystickCount)
        {
            ShowNotification($"MANDO CONECTADO:\n{deviceName}", Color.green);
        }
        else if (currentJoystickCount < lastJoystickCount)
        {
            if (currentJoystickCount == 0)
            {
                ShowNotification("MANDO DESCONECTADO\nCambiando a Teclado", Color.red);
            }
            else
            {
                ShowNotification("MANDO DESCONECTADO", Color.yellow);
            }
        }
        lastJoystickCount = currentJoystickCount;
    }

    private void HandleInputDeviceChanged(InputType currentInputType, string deviceName)
    {
        if (lastJoystickCount > 0)
        {
            string typeName = currentInputType == InputType.Modern ? "Xbox / XInput" : "Mando Genérico";
            ShowNotification($"MODO DE CONTROL:\n{typeName}", Color.cyan);
        }
    }

    public void ShowNotification(string message, Color textColor)
    {
        if (/*notificationPanel == null || */notificationText == null) return;

        notificationText.text = message;
        notificationText.color = textColor;
        //notificationPanel.SetActive(true);

        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
        }

        hideCoroutine = StartCoroutine(HideNotificationRoutine());
    }

    private IEnumerator HideNotificationRoutine()
    {
        yield return new WaitForSecondsRealtime(displayDuration);

        notificationText.text="";

        /*if (notificationPanel != null)
        {
            notificationPanel.SetActive(false);
        }*/
    }
}

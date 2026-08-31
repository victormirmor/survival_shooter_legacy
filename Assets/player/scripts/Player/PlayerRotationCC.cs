using UnityEngine;

public class PlayerRotationCC : MonoBehaviour
{
    [Header("Configuración de Rotación")]
    public float tapAngle = 15f;            // Grados por toque (Tap)
    public float turnSpeed = 15f;           // Velocidad de interpolación (Slerp)
    public float holdRotationSpeed = 180f;  // Velocidad de giro continuo
    public float holdThreshold = 0.18f;     // Umbral de tiempo para considerar Hold

    private Quaternion targetRotation;
    private float lbTimer = 0f;
    private float rbTimer = 0f;

    void Awake()
    {
        targetRotation = transform.rotation;
    }

    void Update()
    {
        // 1. Quick Turn de 180° (si presiona ambos bumpers a la vez)
        if ((Input.GetButtonDown(InputConstants.BUTTON_ROTATE_LEFT) && Input.GetButton(InputConstants.BUTTON_ROTATE_RIGHT)) ||
            (Input.GetButtonDown(InputConstants.BUTTON_ROTATE_RIGHT) && Input.GetButton(InputConstants.BUTTON_ROTATE_LEFT)))
        {
            targetRotation *= Quaternion.Euler(0f, 180f, 0f);
            lbTimer = 0f;
            rbTimer = 0f;
        }
        else
        {
            // 2. Rotación Izquierda (LB)
            HandleRotationButton(InputConstants.BUTTON_ROTATE_LEFT, ref lbTimer, -1f);

            // 3. Rotación Derecha (RB)
            HandleRotationButton(InputConstants.BUTTON_ROTATE_RIGHT, ref rbTimer, 1f);
        }

        // Interpola suavemente hacia la rotación objetivo
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }

    void HandleRotationButton(string buttonName, ref float buttonTimer, float direction)
    {
        // Toque corto instantáneo (Tap)
        if (Input.GetButtonDown(buttonName))
        {
            targetRotation *= Quaternion.Euler(0f, tapAngle * direction, 0f);
            buttonTimer = 0f;
        }

        // Mantener presionado (Hold)
        if (Input.GetButton(buttonName))
        {
            buttonTimer += Time.deltaTime;

            if (buttonTimer > holdThreshold)
            {
                float step = holdRotationSpeed * direction * Time.deltaTime;
                targetRotation *= Quaternion.Euler(0f, step, 0f);
            }
        }

        if (Input.GetButtonUp(buttonName))
        {
            buttonTimer = 0f;
        }
    }
}
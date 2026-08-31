using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerControllerCC : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public float speed = 6f;
    public float gravity = -19.62f;

    private CharacterController controller;
    private Vector3 verticalVelocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        // 1. Resetear la velocidad vertical si tocamos el suelo
        if (controller.isGrounded && verticalVelocity.y < 0)
        {
            verticalVelocity.y = -2f; 
        }

        // 2. Obtener lecturas filtradas por el InputDataMap (o fallback directo si no existe)
        float h = 0f;
        float v = 0f;

        if (InputDataMap.Instance != null)
        {
            h = InputDataMap.Instance.horizontal;
            v = InputDataMap.Instance.vertical;
        }
        else
        {
            h = Input.GetAxisRaw("Horizontal");
            v = Input.GetAxisRaw("Vertical");
        }

        // 3. Proyectar direcciones orientadas a la cámara
        Transform camTransform = Camera.main != null ? Camera.main.transform : null;
        Vector3 moveDirection = Vector3.zero;

        if (camTransform != null)
        {
            Vector3 forward = camTransform.forward;
            Vector3 right = camTransform.right;

            forward.y = 0f;
            right.y = 0f;
            forward.Normalize();
            right.Normalize();

            moveDirection = (forward * v + right * h).normalized;
        }

        // 4. Mover horizontalmente
        controller.Move(moveDirection * speed * Time.deltaTime);

        // 5. Aplicar la gravedad acumulada
        verticalVelocity.y += gravity * Time.deltaTime;
        controller.Move(verticalVelocity * Time.deltaTime);
    }
}
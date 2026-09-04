using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    const string AXIS_HORIZONTAL = "Horizontal";
    const string AXIS_VERTICAL = "Vertical";
    const string ANIM_IS_WALKING = "IsWalking";

    [Header("Movement Settings")]
    public float moveSpeed = 4f;
    public float rotationSpeed = 10f; // Velocidad con la que gira para orientarse
    public float gravity = -9.81f;

    private CharacterController controller;
    private Animator anim;
    private float verticalVelocity;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        float h = Input.GetAxisRaw(AXIS_HORIZONTAL);
        float v = Input.GetAxisRaw(AXIS_VERTICAL);

        MoveAndRotate(h, v);
    }

    void MoveAndRotate(float h, float v)
    {
        // 1. Obtener dirección de movimiento en base a los ejes de entrada
        Vector3 inputDirection = new Vector3(h, 0f, v).normalized;

        // 2. Si hay input, orientar suavemente el personaje hacia esa dirección
        if (inputDirection.magnitude >= 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(inputDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // 3. Dirección de movimiento en el mundo
        Vector3 moveDirection = inputDirection * moveSpeed;

        // 4. Aplicar gravedad para mantenerlo pegado al suelo
        if (controller.isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f;
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        moveDirection.y = verticalVelocity;

        // 5. Mover el CharacterController
        controller.Move(moveDirection * Time.deltaTime);

        // 6. Controlar la animación
        PlayAnim(inputDirection.magnitude >= 0.1f);
    }

    void PlayAnim(bool isWalking)
    {
        if (anim != null)
        {
            anim.SetBool(ANIM_IS_WALKING, isWalking);
        }
    }
}
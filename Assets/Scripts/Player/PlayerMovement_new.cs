using UnityEngine;
using UnitySampleAssets.CrossPlatformInput;

namespace CompleteProject
{
    public static class InputConstants
    {
        // Ejes de Movimiento
        public const string AXIS_HORIZONTAL = "Horizontal";
        public const string   AXIS_VERTICAL = "Vertical";

        // Botones de Acción / Rotación
        public const string BUTTON_ROTATE_RIGHT = "Rotate1"; // RB / Gatillo Derecho
        public const string  BUTTON_ROTATE_LEFT = "Rotate2";  // LB / Gatillo Izquierdo

        // Parámetros de Animator
        public const string ANIM_IS_WALKING = "IsWalking";

        // Parámetros del Blend Tree 2D
        public const string ANIM_SPEED_X = "speedx";
        public const string ANIM_SPEED_Y = "speedy";
    }

        [RequireComponent(typeof(Rigidbody))]
        public class PlayerMovement_new : MonoBehaviour
        {
            public float speed = 6f;

            private Vector3 movement;
            private Rigidbody playerRigidbody;

            void Awake ()
            {
                playerRigidbody = GetComponent<Rigidbody>();
            }

            void FixedUpdate ()
            {
                float h = CrossPlatformInputManager.GetAxisRaw(InputConstants.AXIS_HORIZONTAL);
                float v = CrossPlatformInputManager.GetAxisRaw(InputConstants.AXIS_VERTICAL);

                Move(h, v);
            }

            void Move (float h, float v)
            {
                    // Obtener las direcciones de la cámara principal
                    Transform camTransform = Camera.main.transform;
                    Vector3 forward = camTransform.forward;
                    Vector3 right = camTransform.right;

                    // Ignorar inclinación vertical de la cámara en el eje Y
                    forward.y = 0f;
                    right.y = 0f;
                    forward.Normalize();
                    right.Normalize();

                    // Calcular dirección final combinando ejes
                    Vector3 desiredDirection = (forward * v + right * h).normalized;

                    playerRigidbody.MovePosition(transform.position + desiredDirection * speed * Time.deltaTime);
                }
            }
    }

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
            movement.Set(h, 0f, v);
            movement = movement.normalized * speed * Time.deltaTime;

            playerRigidbody.MovePosition(transform.position + movement);
        }
    }
}

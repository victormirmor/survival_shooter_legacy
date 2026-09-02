using UnityEngine;
using MiJuego.InputAdaptador;

namespace CompleteProject
{
    public static class InputConstants{

        // Botones de Acción / Rotación
        public const string BUTTON_ROTATE_RIGHT = "Rotate1"; // RB / Gatillo Derecho
        public const string  BUTTON_ROTATE_LEFT = "Rotate2";  // LB / Gatillo Izquierdo;
    }

    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovement : MonoBehaviour{

        public const string HORIZONTAL = "Horizontal";
        public const string VERTICAL = "Vertical";

        PlayerAnimation playerAnimation;

           public float speed = 6f;

        private Vector3 movement;
        private Rigidbody playerRigidbody;

        void Awake ()
        {
            playerRigidbody = GetComponent<Rigidbody>();
            playerAnimation = GetComponent<PlayerAnimation>();
        }

        void FixedUpdate ()
        {
            float h = CrossPlatformInputManager.GetAxis(HORIZONTAL);
            float v = CrossPlatformInputManager.GetAxis(VERTICAL);

            Move(h, v);
            playerAnimation.PlayAnim(h,v);
        }

        void Move (float h, float v)
        {
            movement.Set(h, 0f, v);
            movement = movement.normalized * speed * Time.deltaTime;

            playerRigidbody.MovePosition(transform.position + movement);
        }
    }
}

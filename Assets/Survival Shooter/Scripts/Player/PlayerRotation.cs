using UnityEngine;
using MiJuego.InputAdaptador;

namespace CompleteProject
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerRotation : MonoBehaviour
    {
        public float turnAngle = 90f;       // Grados de giro por pulsación.
        public float turnSpeed = 15f;       // Velocidad del suavizado de rotación.

        private Rigidbody playerRigidbody;
        private Quaternion targetRotation;

        void Awake()
        {
            playerRigidbody = GetComponent<Rigidbody>();
            targetRotation = transform.rotation;
        }

        void Update()
        {
            // RB: Girar a la derecha
            if (CrossPlatformInputManager.GetButtonDown("Right"))
            {
                targetRotation *= Quaternion.Euler(0f, turnAngle, 0f);
            }

            // LB: Girar a la izquierda
            if (CrossPlatformInputManager.GetButtonDown("Left"))
            {
                targetRotation *= Quaternion.Euler(0f, -turnAngle, 0f);
            }
        }

        void FixedUpdate()
        {
            // Interpola suavemente la rotación del Rigidbody
            Quaternion newRotation = Quaternion.Slerp(playerRigidbody.rotation, targetRotation, turnSpeed * Time.deltaTime);
            playerRigidbody.MoveRotation(newRotation);
        }
    }
}

using UnityEngine;
using UnitySampleAssets.CrossPlatformInput;

namespace CompleteProject
{
    public class PlayerMovementMOD : MonoBehaviour
    {
        public float speed = 6f;            // Velocidad de movimiento del jugador.

        Vector3 movement;                   // Vector para almacenar la dirección del movimiento.
        public float turnSpeed = 15f;       // Velocidad con la que gira el personaje (a mayor valor, más rápido el giro).
        Animator anim;                      // Referencia al componente Animator.
        Rigidbody playerRigidbody;          // Referencia al Rigidbody del jugador.
        private Quaternion targetRotation;

        private bool shouldRotate = false;  // Flag para capturar la pulsación de Fire2 de forma segura.

        void Awake ()
        {
            // Referencias de componentes.
            anim = GetComponent <Animator> ();
            playerRigidbody = GetComponent <Rigidbody> ();
            targetRotation = transform.rotation;
        }

        void Update ()
        {
            // Leemos la pulsación del botón en Update para no perder el evento frame a frame.
            if (CrossPlatformInputManager.GetButtonDown("Fire2"))
            {
                targetRotation *= Quaternion.Euler(0f, 90f, 0f);
                
            }
        }

        void FixedUpdate ()
        {
            // Guardar ejes de entrada (Analógico / D-Pad / Teclado).
            float h = CrossPlatformInputManager.GetAxisRaw("Horizontal");
            float v = CrossPlatformInputManager.GetAxisRaw("Vertical");

            // Mover al personaje.
            Move (h, v);

            // Rotar 90 grados si se presionó Fire2.
            Turning ();

            // Animación del personaje.
            Animating (h, v);
        }

        void Move (float h, float v)
        {
            // Establecer dirección.
            movement.Set (h, 0f, v);

            // Normalizar y escalar por velocidad e intervalo de tiempo.
            movement = movement.normalized * speed * Time.deltaTime;

            // Mover usando el Rigidbody.
            playerRigidbody.MovePosition (transform.position + movement);
        }

        void Turning ()
        {
            // Interpola suavemente desde la rotación actual hacia la rotación objetivo.
            Quaternion newRotation = Quaternion.Slerp(playerRigidbody.rotation, targetRotation, turnSpeed * Time.deltaTime);

            // Aplicar la rotación al Rigidbody para mantener el comportamiento físico correcto.
            playerRigidbody.MoveRotation(newRotation);
        }
        void Animating (float h, float v)
        {
            // Evaluar si hay movimiento en alguno de los ejes.
            bool walking = h != 0f || v != 0f;

            // Actualizar la animación.
            anim.SetBool ("IsWalking", walking);
        }
            }

        
    }

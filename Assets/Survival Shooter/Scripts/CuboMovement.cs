using UnityEngine;

public class CuboMovement : MonoBehaviour
{
    // Instancia de las acciones generadas
    private Player inputActions;

    // Velocidad de movimiento del cubo
    [SerializeField] private float speed = 5f;

    private void Awake()
    {
        // Creamos la instancia de nuestro mapa de controles
        inputActions = new Player();
    }

    private void OnEnable()
    {
        // Activamos las acciones para que empiecen a escuchar el teclado/mando
        inputActions.movement.Enable();
    }

    private void OnDisable()
    {
        // Desactivamos las acciones al desactivar el objeto
        inputActions.movement.Disable();
    }

    private void Update()
    {
        // 1. Leemos el Vector2 que nos da la acción "move" (WASD / Flechas / Stick)
        Vector2 inputDirection = inputActions.movement.move.ReadValue<Vector2>();

        // 2. Convertimos ese Vector2 a un movimiento en 3D
        // X del input -> movimiento horizontal (derecha / izquierda)
        // Y del input -> movimiento vertical (arriba / abajo)
        Vector3 direction = new Vector3(inputDirection.x,0f, inputDirection.y);

        // 3. Aplicamos el movimiento al objeto
        transform.Translate(direction * speed * Time.deltaTime);
    }
}

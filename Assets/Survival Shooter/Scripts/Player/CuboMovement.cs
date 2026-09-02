using UnityEngine;

public class CuboMovement : MonoBehaviour{
    private Player inputActions;
    [SerializeField] private float speed = 5f;

    private void Awake(){
        inputActions = new Player();
        }

    private void OnEnable(){
        // Activamos las acciones para que empiecen a escuchar el teclado/mando
        inputActions.movement.Enable();
    }

    private void OnDisable(){
        inputActions.movement.Disable();
    }

    private void Update(){
        // 1. Leemos el Vector2 que nos da la acción "move" (WASD / Flechas / Stick)
        Vector2 inputDirection = inputActions.movement.move.ReadValue<Vector2>();

        // 2. Convertimos ese Vector2 a un movimiento en 3D
        Vector3 direction = new Vector3(inputDirection.x,0f, inputDirection.y);

        // 3. Aplicamos el movimiento al objeto
        transform.Translate(direction * speed * Time.deltaTime);
    }
}

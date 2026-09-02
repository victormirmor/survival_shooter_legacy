using UnityEngine;
using UnityEngine.InputSystem;

namespace MiJuego.InputAdaptador
{
    public static class CrossPlatformInputManager
    {
        private static Player inputActions;

        private static Player Actions
        {
            get
            {
                if (inputActions == null)
                {
                    inputActions = new Player();
                    inputActions.movement.Enable();
                }
                return inputActions;
            }
        }

        public static float GetAxis(string axisName)
        {
            Vector2 moveVector = Actions.movement.move.ReadValue<Vector2>();

            switch (axisName)
            {
                case "Horizontal":
                    return moveVector.x;

                case "Vertical":
                    return moveVector.y;

                default:
                    Debug.LogWarning($"El eje '{axisName}' no está mapeado en el adaptador.");
                    return 0f;
            }
        }

        public static bool GetButtonDown(string buttonName)
        {
            switch (buttonName)
            {
                case "Fire1":
                    return Actions.movement.Fire1.WasPressedThisFrame();

                case "Fire2":
                    return Actions.movement.Fire2.WasPressedThisFrame();
                case "Left":
                    return Actions.movement.rotate_left.WasPressedThisFrame();
                case "Right":
                    return Actions.movement.rotate_right.WasPressedThisFrame();
                case "Cancel":
                    return Actions.UI.Cancel.WasPressedThisFrame();

                default:
                    Debug.LogWarning($"El botón '{buttonName}' no está mapeado en el adaptador.");
                    return false;
            }
        }
    }
}

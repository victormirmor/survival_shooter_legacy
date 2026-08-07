using UnityEngine;
using UnitySampleAssets.CrossPlatformInput;

namespace CompleteProject{
    
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimation : MonoBehaviour
    {
        private Animator anim;

        void Awake ()
        {
            anim = GetComponent<Animator>();
        }

        void Update (){
            animation();
        }

        void animation(){
            // Leer ejes de movimiento
            float h = CrossPlatformInputManager.GetAxisRaw(InputConstants.AXIS_HORIZONTAL);
            float v = CrossPlatformInputManager.GetAxisRaw(InputConstants.AXIS_VERTICAL);

            // 1. Enviar valores al Blend Tree 2D
            anim.SetFloat(InputConstants.ANIM_SPEED_X, h);
            anim.SetFloat(InputConstants.ANIM_SPEED_Y, v);

            // Evaluar si el personaje camina y actualizar el Animator
           // bool walking = h != 0f || v != 0f;
            //anim.SetBool(InputConstants.ANIM_IS_WALKING, walking);
        }
    }
}

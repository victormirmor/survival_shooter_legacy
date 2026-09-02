using UnityEngine;
using MiJuego.InputAdaptador;


namespace CompleteProject{
    
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimation : MonoBehaviour
    {
        private Animator anim;
        const string IS_WALKING = "IsWalking";

        void Awake (){
            anim = GetComponent<Animator>();
        }

       public void PlayAnim(float H, float V){

            // Evaluar si el personaje camina y actualizar el Animator
            bool walking = H != 0f || V != 0f;
            anim.SetBool(IS_WALKING, walking);
        }
    }
}

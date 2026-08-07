using UnityEngine;

namespace CompleteProject
{
    public class BulletMovement : MonoBehaviour
    {
        [Header("Física y Movimiento")]
        public float speed = 20f;
        public float lifeTime = 2f; // Duración de vida en segundos

        private void Start()
        {
            // Autodestrucción automática al terminar su tiempo de vida
            Destroy(gameObject, lifeTime);
        }

        private void Update()
        {
            // Traslación constante hacia adelante
            transform.Translate(Vector3.forward * (speed * Time.deltaTime));
        }

        private void OnTriggerEnter(Collider other)
        {
            // Se destruye al impactar con el escenario
            if (other.CompareTag(GameTags.Environment))
            {
                Destroy(gameObject);
            }
        }
    }
}

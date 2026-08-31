using UnityEngine;

namespace CompleteProject
{
    public class BulletProjectile : MonoBehaviour
    {
        [Header("Físicas del Proyectil")]
        public float speed = 20f;
        public float maxLifetime = 5f;

        private void Start()
        {
            // Autodestrucción si vuela sin golpear nada
            Destroy(gameObject, maxLifetime);
        }

        private void Update()
        {
            // Movimiento constante hacia adelante
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }
}
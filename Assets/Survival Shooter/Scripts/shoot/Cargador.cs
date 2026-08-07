using UnityEngine;

namespace CompleteProject
{
    public class Cargador : MonoBehaviour
    {
        [Header("Configuración del Cargador")]
        public GameObject bulletPrefabToGive; // Asignás Bullet o Bullet_2 Variant
        public int ammoAmount = 20;

        private void OnTriggerEnter(Collider other)
        {
            // Usamos la constante limpia de GameTags
            if (other.CompareTag(GameTags.Player))
            {
                // Buscamos el script de disparo en el jugador
                var playerShooting = other.GetComponentInChildren<PlayerShooting_mod>();
                
                if (playerShooting != null)
                {
                    // Cambia la bala y añade la munición
                    playerShooting.ReloadWeapon(bulletPrefabToGive, ammoAmount);
                    
                    // Se destruye el pickup de la escena
                    Destroy(gameObject);
                }
            }
        }
    }
}

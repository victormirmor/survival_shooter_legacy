using UnityEngine;
using UnityEngine.EventSystems; // Required for UI check
using MiJuego.InputAdaptador;


namespace CompleteProject{

    public class PlayerShooting_mod : MonoBehaviour
    {
        public int Bullets_rest = 20;                  // Balas disponibles para disparar
        public float timeBetweenBullets = 0.15f;        // Cadencia de tiro
        public GameObject bulletPrefab;                 // Prefab del proyectil con trayectoria
        public Transform gunTip;                        // Punto de salida del arma (muro o punta del cañón)

        float timer;                                    // Temporizador para la cadencia
        ParticleSystem gunParticles;                    // Efectos de partículas
        AudioSource gunAudio;                           // Sonido de disparo
        Light gunLight;                                 // Luz del fogonazo
        public Light faceLight;                         // Luz auxiliar
        float effectsDisplayTime = 0.2f;                // Duración visual del fogonazo

        void Awake ()
        {
            // Referencias a componentes visuales y auditivos
            gunParticles = GetComponent<ParticleSystem> ();
            gunAudio = GetComponent<AudioSource> ();
            gunLight = GetComponent<Light> ();
        }

        void Update (){
    // Si el tiempo está congelado por la pausa, no procesa disparos
    if (Time.timeScale == 0f|| EventSystem.current.IsPointerOverGameObject()) return;

            timer += Time.deltaTime;

            if (CrossPlatformInputManager.GetButtonDown ("Fire1") && timer >= timeBetweenBullets && Time.timeScale != 0 && Bullets_rest > 0)
            {
                Shoot ();
                Bullets_rest--;
            }

            if(timer >= timeBetweenBullets * effectsDisplayTime)
            {
                DisableEffects ();
            }
        }

        public void DisableEffects ()
        {
            gunLight.enabled = false;
            faceLight.enabled = false;
        }

        void Shoot ()
{
    timer = 0f;

    // Efectos visuales y de sonido
    gunAudio.Play ();
    gunLight.enabled = true;
    faceLight.enabled = true;

    gunParticles.Stop ();
    gunParticles.Play ();

    // Instanciar el proyectil (respeta los valores configurados en su prefab)
    if (bulletPrefab != null && gunTip != null)
    {
        Instantiate(bulletPrefab, gunTip.position, gunTip.rotation);
    }
}
 public void ReloadWeapon(GameObject newBulletPrefab, int amount)
{
    if (newBulletPrefab != null){
        bulletPrefab = newBulletPrefab; // Cambia el proyectil a instanciar
    }

    Bullets_rest += amount;
} 
        }

          
    }

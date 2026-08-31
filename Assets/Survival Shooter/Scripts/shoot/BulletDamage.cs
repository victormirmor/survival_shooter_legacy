using UnityEngine;

namespace CompleteProject
{
    public class BulletDamage : MonoBehaviour
    {
        [Header("Configuración de Daño")]
        public int baseDamage = 20;
        public int headshotMultiplier = 2;

        public int FinalDamage { get; private set; }
        public bool IsHeadshot { get; private set; }

        private void Awake()
        {
            // 33.3% de probabilidad de crítico (1 de 3)
            IsHeadshot = (Random.Range(1, 4) == 1);
            FinalDamage = IsHeadshot ? (baseDamage * headshotMultiplier) : baseDamage;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(GameTags.Enemy))
            {
                EnemyHealth enemyHealth = other.GetComponentInParent<EnemyHealth>();

                if (enemyHealth != null)
                {
                    // --- LOGS DE DIAGNÓSTICO EN CONSOLA ---
                    if (IsHeadshot)
                    {
                        Debug.Log($"<b>¡HEADSHOT CRÍTICO!</b> Golpeado: {other.name} | Daño enviado: {FinalDamage}");
                    }
                    else
                    {
                        Debug.Log($"Tiro Normal: {other.name} | Daño enviado: {FinalDamage}");
                    }
                    // -------------------------------------

                    // Notificamos a la UI de Headshot si aplica
                    if (IsHeadshot)
                    {
                        //HeadshotUI headshotUI = FindObjectOfType<HeadshotUI>(true);
                        HeadshotUI headshotUI = FindFirstObjectByType<HeadshotUI>(FindObjectsInactive.Include);
                        if (headshotUI != null)
                        {
                            headshotUI.TriggerHeadshot(FinalDamage);
                        }
                        else
                        {
                            Debug.LogWarning("[BulletDamage] Se produjo un Headshot pero no se encontró 'HeadshotUI' en la escena.");
                        }
                    }

                    // Aplicamos el daño real al enemigo
                    enemyHealth.TakeDamage(FinalDamage, transform.position);
                }

                Destroy(gameObject);
            }
            else if (other.CompareTag(GameTags.Environment))
            {
                Destroy(gameObject);
            }
        }
    }
}

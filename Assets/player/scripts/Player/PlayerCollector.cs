using UnityEngine;
using CompleteProject;

public class PlayerCollector : MonoBehaviour
{
    [Header("Configuración de Efectos")]
    public AudioSource pickupAudioSource;
    public AudioClip coinSound;
    public AudioClip healthSound;
    public AudioClip ammoSound;

    private void OnTriggerEnter(Collider other)
    {
        // 1. Ignorar si el objeto colisionado ya está desactivado o procesándose
        if (!other.gameObject.activeSelf) return;

        string itemTag = other.tag;

        switch (itemTag)
        {
            case GameTags.Coin:
                CollectCoin(other.gameObject);
                break;

            case GameTags.Health:
                CollectHealth(other.gameObject, 20);
                break;

            case GameTags.Ammo:
                CollectAmmo(other.gameObject, 15);
                break;

            case GameTags.DamageTrap:
                TriggerTrap(other.gameObject, 25);
                break;
        }
    }

    private void CollectCoin(GameObject coinObj)
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddCoins(1);
            GameManager.Instance.AddPoints(10);
        }

        PlaySound(coinSound);
        
        // En lugar de Destroy, desactivamos para reciclar
        coinObj.SetActive(false); 
    }

    private void CollectHealth(GameObject healthObj, int healAmount)
    {
        if (HealthManager.Instance != null)
        {
            HealthManager.Instance.Heal(healAmount);
        }

        PlaySound(healthSound);
        healthObj.SetActive(false);
    }

    private void CollectAmmo(GameObject ammoObj, int ammoAmount)
    {
        PlayerShooting shooting = GetComponent<PlayerShooting>();
        if (shooting != null)
        {
            shooting.ReloadWeapon(null, ammoAmount);
        }

        PlaySound(ammoSound);
        ammoObj.SetActive(false);
    }

    private void TriggerTrap(GameObject trapObj, int damage)
    {
        if (HealthManager.Instance != null)
        {
            HealthManager.Instance.TakeDamage(damage);
        }

        trapObj.SetActive(false);
    }

    private void PlaySound(AudioClip clip)
    {
        if (pickupAudioSource != null && clip != null)
        {
            pickupAudioSource.PlayOneShot(clip);
        }
    }
}
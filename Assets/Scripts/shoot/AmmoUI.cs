using UnityEngine;
using UnityEngine.UI;
using CompleteProject;

public class AmmoUI : MonoBehaviour
{
    private PlayerShooting_mod playerShooting;
    private Text bulletText;

    void Start()
    {
        // Buscamos el componente usando la constante de UI
        foreach (Text textComponent in GetComponentsInChildren<Text>(true))
        {
            if (textComponent.gameObject.name == GameUI.BulletText)
            {
                bulletText = textComponent;
                break;
            }
        }

        // Buscamos al jugador por su Tag
        GameObject player = GameObject.FindGameObjectWithTag(GameTags.Player);
        if (player != null)
        {
            playerShooting = player.GetComponentInChildren<PlayerShooting_mod>();
        }

        // Logs de verificación
        if (bulletText == null)
            Debug.LogError($"[AmmoUI] No se encontró el objeto UI '{GameUI.BulletText}' en el Canvas.");
        if (playerShooting == null)
            Debug.LogError($"[AmmoUI] No se encontró 'PlayerShooting_mod' en el Jugador.");
    }

    void Update()
    {
        if (bulletText != null && playerShooting != null)
        {
            bulletText.text = playerShooting.Bullets_rest.ToString();
        }
    }
}
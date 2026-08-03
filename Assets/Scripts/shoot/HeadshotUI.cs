using UnityEngine;
using UnityEngine.UI;

namespace CompleteProject
{
    public class HeadshotUI : MonoBehaviour{
        public float displayDuration = 1.2f;
        private Text damageText;
        public GameObject HeadshotOBJ;

        private void Awake()
        {
            damageText = GetComponentInChildren<Text>(true);
            HeadshotOBJ.SetActive(false); // Nace oculto
        }

        public void TriggerHeadshot(int damage)
        {
            if (damageText != null)
            {
                damageText.text = $"HEADSHOT!\nDAÑO {damage}";
            }

            gameObject.SetActive(true);

            // Cancela el Ocultar anterior si metés varias balas seguidas
            CancelInvoke(nameof(Hide)); 
            Invoke(nameof(Hide), displayDuration);
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
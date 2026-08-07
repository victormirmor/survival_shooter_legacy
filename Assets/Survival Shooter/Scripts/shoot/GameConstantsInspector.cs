using UnityEngine;

namespace CompleteProject
{
    [ExecuteInEditMode]
    public class GameConstantsInspector : MonoBehaviour
    {
        [Header("Ecosistema de Etiquetas (GameTags)")]
        [TextArea(3, 5)]
        [SerializeField] private string tagsLibrary;

        [Header("Ecosistema de Interfaz (GameUI)")]
        [TextArea(2, 4)]
        [SerializeField] private string uiLibrary;

        private void OnValidate()
        {
            // Sincroniza la vista del Inspector con la biblioteca de constantes
            tagsLibrary = $@"Player: {GameTags.Player}
Enemy: {GameTags.Enemy}
Environment: {GameTags.Environment}";

            uiLibrary = $@"BulletText: {GameUI.BulletText}";
        }
    }
}
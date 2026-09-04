using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Configuración de Objetivo")]
    public string playerTag = "Player";
    private Transform target;

    [Header("Configuración de Posición y Ángulo")]
    public float cameraDistance = 6.0f;
    public Vector3 cameraRotation = new Vector3(35f, 0f, 0f);
    public float smoothing = 5f;

    void Awake()
    {
        // 1. Buscar automáticamente al objeto con el Tag configurado
        GameObject playerObj = GameObject.FindGameObjectWithTag(playerTag);
        if (playerObj != null)
        {
            target = playerObj.transform;
        }
        else
        {
            Debug.LogWarning($"CameraFollow: No se encontró ningún objeto con el Tag '{playerTag}'.");
        }
    }

    void LateUpdate()
    {
        if (target == null) return;

        // 2. Calcular la rotación y posición deseada
        Quaternion targetRotation = Quaternion.Euler(cameraRotation);
        Vector3 origin = target.position + Vector3.up * 1.0f;
        Vector3 backDirection = -(targetRotation * Vector3.forward);
        Vector3 targetCamPos = origin + (backDirection * cameraDistance);

        // 3. Aplicar rotación y posición suavemente tras el movimiento del personaje
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smoothing * Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
    }
}
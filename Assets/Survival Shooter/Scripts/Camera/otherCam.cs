using UnityEngine;
using System.Collections.Generic;

namespace CompleteProject
{
    public class otherCam: MonoBehaviour
    {
        public Transform target;            // Posición del objetivo (Player)
        public float smoothing = 5f;        // Velocidad de interpolación de la cámara

        [Header("Configuración de Posición y Ángulo")]
        public float cameraDistance = 6.0f; 
        public Vector3 cameraRotation = new Vector3(35f, 0f, 0f);

        [Header("Límites del Escenario (Bordes del Piso)")]
        public bool useClamping = true;
        public float minX = -10f;
        public float maxX = 10f;
        public float minZ = -10f;
        public float maxZ = 10f;

        [Header("Compensación de Ángulo (Anti-Franja Inferior)")]
        public float bottomEdgePadding = 3.5f; 

        [Header("Objetos Intermedios (Obstáculos a Ocultar)")]
        public LayerMask obstacleLayer;     // Capa de los objetos 3D que tapan al player

        [Header("Debugging")]
        public bool showGizmos = true;

        private List<Renderer> hiddenRenderers = new List<Renderer>();

        void FixedUpdate()
        {
            if (target == null) return;

            // 1. Aplicar rotación de la cámara
            Quaternion targetRotation = Quaternion.Euler(cameraRotation);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smoothing * Time.fixedDeltaTime);

            // 2. Calcular posición ideal respecto al player
            Vector3 origin = target.position + Vector3.up * 1.0f;
            Vector3 backDirection = -(targetRotation * Vector3.forward);
            Vector3 targetCamPos = origin + (backDirection * cameraDistance);

            // 3. Clamping por vectores para evitar las franjas/abismo
            if (useClamping)
            {
                float adjustedMinZ = minZ + bottomEdgePadding;

                targetCamPos.x = Mathf.Clamp(targetCamPos.x, minX, maxX);
                targetCamPos.z = Mathf.Clamp(targetCamPos.z, adjustedMinZ, maxZ);
            }

            // 4. Mover la cámara de forma fluida
            transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.fixedDeltaTime);

            // 5. Ocultar dinámicamente cualquier objeto que tape al personaje
            HandleObstacleOcclusion();
        }

        void HandleObstacleOcclusion()
        {
            RestoreHiddenRenderers();

            Vector3 direction = target.position - transform.position;
            float distance = direction.magnitude;

            // Detectamos si hay elementos de la capa Obstacles tapando la visión
            RaycastHit[] hits = Physics.RaycastAll(transform.position, direction.normalized, distance, obstacleLayer);

            foreach (RaycastHit obstacleHit in hits)
            {
                Renderer objRenderer = obstacleHit.collider.GetComponent<Renderer>();
                if (objRenderer != null && objRenderer.enabled)
                {
                    objRenderer.enabled = false;
                    hiddenRenderers.Add(objRenderer);
                }
            }
        }

        void RestoreHiddenRenderers()
        {
            for (int i = 0; i < hiddenRenderers.Count; i++)
            {
                if (hiddenRenderers[i] != null)
                {
                    hiddenRenderers[i].enabled = true;
                }
            }
            hiddenRenderers.Clear();
        }

        private void OnDrawGizmos()
        {
            if (!showGizmos || !useClamping) return;

            Gizmos.color = Color.yellow;
            float adjustedMinZ = minZ + bottomEdgePadding;

            Vector3 p1 = new Vector3(minX, 1f, adjustedMinZ);
            Vector3 p2 = new Vector3(maxX, 1f, adjustedMinZ);
            Vector3 p3 = new Vector3(maxX, 1f, maxZ);
            Vector3 p4 = new Vector3(minX, 1f, maxZ);

            Gizmos.DrawLine(p1, p2);
            Gizmos.DrawLine(p2, p3);
            Gizmos.DrawLine(p3, p4);
            Gizmos.DrawLine(p4, p1);
        }
    }
}
using UnityEngine;

public class HDCullingOcclusion : MonoBehaviour
{
    public Camera playerCamera;
    private Renderer[] occluders;

    private void Start()
    {
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }

        // Найти все объекты с компонентом Renderer
        occluders = FindObjectsOfType<Renderer>();
    }

    private void Update()
    {
        if (playerCamera == null)
        {
            return;
        }

        foreach (Renderer occluder in occluders)
        {
            bool isVisible = CheckVisibility(occluder);
            occluder.gameObject.SetActive(isVisible);
        }
    }

    private bool CheckVisibility(Renderer occluder)
    {
        if (!occluder.isVisible)
        {
            return true; // Объект не видим, оставляем его активным
        }

        if (CheckLineOfSight(occluder))
        {
            return false; // Объект перекрыт, скрываем его
        }

        return true; // Объект видим
    }

    private bool CheckLineOfSight(Renderer occluder)
    {
        Vector3 cameraPosition = playerCamera.transform.position;
        Vector3 playerPosition = transform.position;

        // Получить все точки границы Renderer
        Vector3[] rendererBoundsPoints = GetRendererBoundsPoints(occluder.bounds);

        // Проверить каждую точку на линию видимости между камерой и точкой
        foreach (Vector3 boundsPoint in rendererBoundsPoints)
        {
            RaycastHit hit;
            if (Physics.Linecast(cameraPosition, boundsPoint, out hit))
            {
                if (hit.transform != occluder.transform)
                {
                    return true; // Линия видимости перекрыта, объект не полностью скрыт
                }
            }
        }

        return false; // Линия видимости свободна, объект полностью скрыт
    }

    private Vector3[] GetRendererBoundsPoints(Bounds bounds)
    {
        Vector3 center = bounds.center;
        Vector3 extents = bounds.extents;

        Vector3[] points = new Vector3[8];

        points[0] = center + new Vector3(extents.x, extents.y, extents.z);
        points[1] = center + new Vector3(extents.x, extents.y, -extents.z);
        points[2] = center + new Vector3(extents.x, -extents.y, extents.z);
        points[3] = center + new Vector3(extents.x, -extents.y, -extents.z);
        points[4] = center + new Vector3(-extents.x, extents.y, extents.z);
        points[5] = center + new Vector3(-extents.x, extents.y, -extents.z);
        points[6] = center + new Vector3(-extents.x, -extents.y, extents.z);
        points[7] = center + new Vector3(-extents.x, -extents.y, -extents.z);

        return points;
    }
}
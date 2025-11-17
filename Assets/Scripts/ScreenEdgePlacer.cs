using UnityEngine;

public class ScreenEdgePlacer : MonoBehaviour
{
    
    public enum ScreenEdge { Left, Right }
    public ScreenEdge edge;
    public float offset = 0f;

    void Start()
    {
        Camera mainCamera = Camera.main;

        if (mainCamera == null || !mainCamera.orthographic)
        {
            return;
        }

        float viewPortX = (edge == ScreenEdge.Left) ? 0.0f : 1.0f;

        Vector3 worldPoint = mainCamera.ViewportToWorldPoint(new Vector3(viewPortX, 0.5f, mainCamera.nearClipPlane));

        Bounds combinedBounds = GetCombinedBounds();
        
        
        float pivotToEdgeDist;
        
        float finalX;

        if (edge == ScreenEdge.Left)
        {
            pivotToEdgeDist = transform.position.x - combinedBounds.min.x;
            finalX = worldPoint.x + pivotToEdgeDist + offset;
        }
        else
        {
            pivotToEdgeDist = combinedBounds.max.x - transform.position.x;
            finalX = worldPoint.x - pivotToEdgeDist - offset;
        }


        transform.position = new Vector3(finalX, transform.position.y, transform.position.z);
    }

    public Bounds GetCombinedBounds()
    {
        Bounds bounds = new Bounds(transform.position, Vector3.zero);

        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        if (renderers.Length > 0)
        {
            foreach (Renderer renderer in renderers)
            {
                if (bounds.extents == Vector3.zero)
                {
                    bounds = renderer.bounds;
                }
                else
                {
                    bounds.Encapsulate(renderer.bounds);
                }
            }
            return bounds;
        }

        Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
        if (colliders.Length > 0)
        {
            foreach (Collider2D col in colliders)
            {
                 if (bounds.extents == Vector3.zero)
                {
                    bounds = col.bounds;
                }
                else
                {
                    bounds.Encapsulate(col.bounds);
                }
            }
            return bounds;
        }

        return bounds;
    }
}
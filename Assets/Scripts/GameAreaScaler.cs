using UnityEngine;

public class GameAreaScaler : MonoBehaviour
{
    [Header("Border References")]
    [Tooltip("Assign your GameObject that has the ScreenEdgePlacer (Left)")]
    public ScreenEdgePlacer leftBorder;
    [Tooltip("Assign your GameObject that has the ScreenEdgePlacer (Right)")]
    public ScreenEdgePlacer rightBorder;

    [Header("Design Settings")]
    [Tooltip("The width of your game area in the Unity editor (e.g., from -2.5 to 2.5 is a width of 5)")]
    public float designWidth = 5.0f; 

    private float lastCalculatedScale = 1f;


    void LateUpdate()
    {
        float newScale = CalculateScale();
        
        if (Mathf.Abs(newScale - lastCalculatedScale) > 0.001f)
        {
            ApplyScale(newScale);
        }
    }

    float CalculateScale()
    {
        if (leftBorder == null || rightBorder == null)
        {
            Debug.LogError("Border references not set on GameAreaScaler!", this);
            return 1f;
        }

        Bounds leftBounds = leftBorder.GetCombinedBounds();
        Bounds rightBounds = rightBorder.GetCombinedBounds();

        float worldSpaceWidth = rightBounds.min.x - leftBounds.max.x;

        if (designWidth <= 0)
        {
            Debug.LogWarning("Design Width must be positive.", this);
            return 1f;
        }

        return worldSpaceWidth / designWidth;
    }
    
    void ApplyScale(float scaleFactor)
    {
        transform.localScale = new Vector3(scaleFactor, scaleFactor, 1f);
        lastCalculatedScale = scaleFactor;
    }
}
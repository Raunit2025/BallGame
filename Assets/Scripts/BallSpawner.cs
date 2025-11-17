using UnityEngine;
using System.Collections;

public class BallSpawner : MonoBehaviour
{
    public GameObject ballPrefab;
    public Transform spawnHeightReference;
    private Camera mainCamera;
    private GameManager gameManager;

    [Header("Spawn Boundaries")]
    [Tooltip("Assign the GameObject that has your LEFT ScreenEdgePlacer script.")]
    [SerializeField] private ScreenEdgePlacer leftBorder; 
    [Tooltip("Assign the GameObject that has your RIGHT ScreenEdgePlacer script.")]
    [SerializeField] private ScreenEdgePlacer rightBorder;
    
    private float worldMinX = 0f;
    private float worldMaxX = 0f;
    private bool boundariesCalculated = false;

    void Start()
    {
        mainCamera = Camera.main;
        gameManager = FindObjectOfType<GameManager>();
        
        if (gameManager == null) Debug.LogError("BallSpawner cannot find GameManager!");
        
        StartCoroutine(CalculateBoundariesAfterWait());
    }
    
    private IEnumerator CalculateBoundariesAfterWait()
    {

        yield return new WaitForEndOfFrame();
        
        if (leftBorder == null || rightBorder == null)
        {
            yield break;
        }
        
        Bounds leftBounds = leftBorder.GetCombinedBounds();
        Bounds rightBounds = rightBorder.GetCombinedBounds();

        worldMinX = leftBounds.max.x;
        worldMaxX = rightBounds.min.x;
        
        if (worldMinX >= worldMaxX)
        {
        }
        else
        {
            boundariesCalculated = true;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SpawnBall();
        }
    }

    void SpawnBall()
    {
        if (!boundariesCalculated)
        {
            return; 
        }

        if (ballPrefab == null || spawnHeightReference == null || mainCamera == null)
        {
            return;
        }

        if (gameManager != null && gameManager.UseBall())
        {
            Vector3 tapWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            
            float clampedX = Mathf.Clamp(tapWorldPosition.x, worldMinX, worldMaxX);

            Vector3 finalSpawnPos = new Vector3(
                clampedX, 
                spawnHeightReference.position.y,
                0
            );

            Instantiate(ballPrefab, finalSpawnPos, Quaternion.identity);
        }
    }
}
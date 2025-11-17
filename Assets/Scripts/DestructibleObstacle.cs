using UnityEngine;

// --- THIS IS A NEW SCRIPT ---
// Attach this to your Obstacle.prefab

public class DestructibleObstacle : MonoBehaviour
{
    public int hitsToDestroy = 2; // Set how many hits it can take
    public CollectibleSpawner parentSpawner; 

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            hitsToDestroy--;

            if (hitsToDestroy <= 0)
            {
                if (parentSpawner != null)
                {
                    parentSpawner.StartRespawn();
                }
                Destroy(gameObject);
            }
        }
    }
}
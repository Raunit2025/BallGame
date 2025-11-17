using System.Collections; 
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public enum CollectibleType { Star, Gem }

    [Header("Collectible Settings")]
    public CollectibleType type = CollectibleType.Star;
    public int value = 10; 

    [Header("References")]
    public CollectibleSpawner parentSpawner; 
    private GameManager gameManager;

    private bool isCollected = false;

    void Start()
    {
        gameManager = GameManager.Instance;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && !isCollected && gameManager != null)
        {
            isCollected = true;

            if (type == CollectibleType.Star)
            {
                gameManager.AddScore(value);
            }
            else if (type == CollectibleType.Gem)
            {
                float randomGemValue = Random.Range(0f, 1f);
                gameManager.AddGems(randomGemValue);
            }

            if (parentSpawner != null)
            {
                parentSpawner.StartRespawn();
            }

            StartCoroutine(CollectAndDestroy());
        }
    }

    private IEnumerator CollectAndDestroy()
    {
        yield return new WaitForFixedUpdate();


        Destroy(gameObject);
    }
}
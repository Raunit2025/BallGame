using System.Collections;
using UnityEngine;

public class CollectibleSpawner : MonoBehaviour
{
    [Header("Prefabs (Assign in Inspector)")]
    public GameObject starPrefab;
    public GameObject gemPrefab;
    public GameObject obstaclePrefab; 
    public GameObject powerUpPrefab;
    public Transform gameAreaContainer;


    [Header("Spawn Logic")]
    [Range(0f, 1f)]
    public float starChance = 0.5f;
    [Range(0f, 1f)]
    public float gemChance = 0.3f;  
    [Range(0f, 1f)]
    public float powerUpChance = 0.1f;

    public float respawnDelay = 2f; 

    void Start()
    {
        SpawnItem();
    }

    void SpawnItem()
    {
        float rand = Random.Range(0f, 1f);
        GameObject prefabToSpawn = null;

        if (rand < starChance)
        {
            prefabToSpawn = starPrefab;
        }
        else if (rand < starChance + gemChance)
        {
            prefabToSpawn = gemPrefab;
        }
        else if (rand < starChance + gemChance + powerUpChance)
        {
            prefabToSpawn = powerUpPrefab;
        }
        else
        {
            prefabToSpawn = obstaclePrefab;
        }

        if (prefabToSpawn != null)
        {
            GameObject spawnedItem = Instantiate(prefabToSpawn, transform.position, transform.rotation, gameAreaContainer);
            Collectible collectibleScript = spawnedItem.GetComponent<Collectible>();
            if (collectibleScript != null)
            {
                collectibleScript.parentSpawner = this;
            }

            DestructibleObstacle obstacleScript = spawnedItem.GetComponent<DestructibleObstacle>();
            if (obstacleScript != null)
            {
                obstacleScript.parentSpawner = this;
            }
        }
    }

    public void StartRespawn()
    {
        StartCoroutine(RespawnCoroutine());
    }

    private IEnumerator RespawnCoroutine()
    {
        yield return new WaitForSeconds(respawnDelay);
        SpawnItem();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 0.2f);
    }
}
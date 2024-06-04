using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject prefabToSpawn; // Reference to the prefab to spawn
    public Transform spawnPoint; // Spawn point for the prefab
    public float minYOffset = -4f; // Minimum y offset from the spawn point
    public float maxYOffset = 4f; // Maximum y offset from the spawn point
    public float spawnInterval = 2f; // Time interval between spawns
    private float timer; // Timer to keep track of spawn intervals

    void Start()
    {
        // Start the timer
        timer = spawnInterval;
    }

    void Update()
    {
        // Decrease the timer
        timer -= Time.deltaTime;

        // Check if it's time to spawn a new prefab
        if (timer <= 0f)
        {
            // Reset the timer
            timer = spawnInterval;

            // Calculate a random y offset within the specified range
            float randomYOffset = Random.Range(minYOffset, maxYOffset);

            // Calculate the spawn position by adding the y offset to the spawn point's position
            Vector3 spawnPosition = spawnPoint.position + new Vector3(0f, randomYOffset, 0f);

            // Instantiate the prefab at the calculated spawn position
            Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);

            // Log a message indicating that the prefab has been spawned
            Debug.Log("Prefab spawned at: " + spawnPosition);
        }
    }
}


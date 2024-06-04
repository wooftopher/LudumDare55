using UnityEngine;

public class ObjectInitializer : MonoBehaviour
{
    // Reference to the object whose parameters need to be set
    public EnemySpawner enemySpawner;

    // Parameters to set on the object
    public int difficultyLevel = 1;
    // Add more parameters as needed

    void Start()
    {
        // Check if the object to initialize is assigned
        if (enemySpawner != null)
        {
            // Set parameters on the object
            SetParameters();
        }
        else
        {
            Debug.LogWarning("Object to initialize is not assigned in ObjectInitializer script on " + gameObject.name);
        }
    }

    void SetParameters()
    {
        if (difficultyLevel == 1){
            enemySpawner.spawnInterval = 3f;
        }
    }
}

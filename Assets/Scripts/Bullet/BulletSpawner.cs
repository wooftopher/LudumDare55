using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public GameObject smallBulletPrefab;
    public GameObject mediumBulletPrefab;
    public GameObject bigBulletPrefab;
    public GameObject healthBulletPrefab;
    public GameObject manaBulletPrefab;

    public float bulletSpeed = 5f;
    public Vector2 bulletDirection = Vector2.left;
    public float fireRate = 2f;
    public float maxOffset = 0.5f; // Maximum offset perpendicular to the direction

    private float cooldownTimer = 0f;

    void Update()
    {
        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer <= 0f)
        {
            SpawnBullet();
            cooldownTimer = 1f / fireRate;
        }
    }

    public void SpawnBullet()
    {
        GameObject bulletPrefab = null;

        // Choose the appropriate bullet prefab based on the type
        float random = Random.value;
        if (random < 0.2f)
        {
            bulletPrefab = smallBulletPrefab;
        }
        else if (random < 0.4f)
        {
            bulletPrefab = mediumBulletPrefab;
        }
        else if (random < 0.6f)
        {
            bulletPrefab = bigBulletPrefab;
        }
        else if (random < 0.8f)
        {
            bulletPrefab = healthBulletPrefab;
        }
        else
        {
            bulletPrefab = manaBulletPrefab;
        }

        if (bulletPrefab != null)
        {
            // Calculate a random offset perpendicular to the bullet direction
            Vector2 perpendicularOffset = new Vector2(bulletDirection.y, -bulletDirection.x) * Random.Range(-maxOffset, maxOffset);

            // Calculate the spawn position with the offset
            Vector3 spawnPosition = transform.position + (Vector3)perpendicularOffset;

            // Instantiate the selected bullet prefab with the calculated spawn position
            GameObject bullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);

            // Get the Bullet component from the instantiated bullet
            Bullet bulletComponent = bullet.GetComponent<Bullet>();
            if (bulletComponent != null)
            {
                // bulletComponent.SetSpeed(bulletSpeed);
                bulletComponent.SetDirection(bulletDirection);
            }
            else
            {
                Debug.LogError("Selected bullet prefab does not have a Bullet component.");
            }
        }
        else
        {
            Debug.LogError("No bullet prefab is assigned.");
        }
    }
}

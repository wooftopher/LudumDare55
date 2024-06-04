using UnityEngine;

public class Fireball : MonoBehaviour
{
    private float speed = 5f; // Speed of the fireball
    private Vector2 direction = Vector2.left; // Default direction of the fireball

    private Rigidbody2D rb; // Reference to the Rigidbody2D component

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection.normalized; // Normalize the direction vector
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component attached to the fireball
        rb.velocity = direction * speed; // Set initial velocity of the fireball based on the specified direction and speed
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the fireball collides with the player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Get the PlayerHealth component from the collided player object
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();

            // Check if the PlayerHealth component exists
            if (playerHealth != null)
            {
                // Decrease the player's health
                playerHealth.DecreaseHealth(10); // Adjust the value as needed
            }

            // Destroy the fireball upon collision with the player
            Destroy(gameObject);
        }
    }
}

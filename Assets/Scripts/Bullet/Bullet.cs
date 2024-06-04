using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 10f; // Speed of the bullet
    [SerializeField] private float damage = 10f;
    [SerializeField] private float mana = 0f;
    private Vector2 direction = Vector2.right; // Default direction of the bullet

    // Method to set the speed of the bullet
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    // Method to set the direction of the bullet
    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection.normalized;
    }

    void Update()
    {
        // Move the bullet in its direction with the specified speed
        transform.Translate(direction * speed * Time.deltaTime);
    }

    // Method to handle collisions
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the fireball collides with the player
        if (collision.gameObject.CompareTag("Player")) {
            if (damage > 0){
                PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
                playerHealth.DecreaseHealth(damage);
            }  
            else if (damage == 0) {
                PlayerMana playerMana = collision.gameObject.GetComponent<PlayerMana>();
                playerMana.IncreaseMana(mana);
            }
            else {
                PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
                playerHealth.DecreaseHealth(damage);
            }
        }
        Destroy(gameObject);
    }
}

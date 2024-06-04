using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;
    [SerializeField] private string summonTag = "Summon"; // Tag to identify summon objects
    [SerializeField] private int damagePerSecond = 10; // Damage per second while colliding
    [SerializeField] private Vector2 direction = Vector2.left; // Default direction of the fireball
    StatusBar healthBar;
    private Vector2 currentVelocity; // Current velocity of the enemy
    private Transform myTransform; // Cached transform component
    private bool isColliding; // Flag to track collision status
   private Animator anim;


    private void Awake()
    {
        healthBar = GetComponentInChildren<StatusBar>();
        anim = GetComponent<Animator>();

        myTransform = transform; // Cache transform component for efficiency
    }

    void Start()
    {
        currentHealth = maxHealth;
        currentVelocity = direction * moveSpeed; // Set initial velocity
    }

    void Update()
    {
        // Move the enemy based on its current velocity
        myTransform.position += (Vector3)currentVelocity * Time.deltaTime;

        if (isColliding)
        {
            // Deal damage every second while colliding
            DealDamageBySecond();
            anim.SetBool("isAttacking", true);
        }
        else
            anim.SetBool("isAttacking", false);  

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collision is with an object tagged as "Enemy"
        if (other.gameObject.CompareTag(summonTag))
        {
            isColliding = true; // Set collision flag
            currentVelocity = Vector2.zero; // Stop movement
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        // Check if the collision is with an object tagged as "Enemy"
        if (other.gameObject.CompareTag(summonTag))
        {
            isColliding = true; // Set collision flag
            // Check if the summon has velocity
            if (currentVelocity != Vector2.zero)
            {
                currentVelocity = Vector2.zero; // Stop movement
            }
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        // Check if the collision is with an object tagged as "Summon"
        if (other.gameObject.CompareTag(summonTag))
        {
            isColliding = false; // Reset collision flag
            currentVelocity = Vector2.left * moveSpeed; // Resume movement to the left
        }
    }

private void DealDamageBySecond()
    {
        // Find all game objects with the summon tag colliding with this enemy
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, transform.localScale, 0f);

        // Deal damage to each game object with the summon tag colliding with this enemy
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag(summonTag))
            {
                // Damage the game object every second
                collider.SendMessage("TakeDamage", damagePerSecond * Time.deltaTime, SendMessageOptions.DontRequireReceiver);
            }
        }
    }


    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        healthBar.UpdateStatusBar(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}

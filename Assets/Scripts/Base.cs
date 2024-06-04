using UnityEngine;
using UnityEngine.SceneManagement;

public class Base : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f; // Maximum health of the base object
    [SerializeField] private string gameOverSceneName = "GameOverScene"; // Name of the game over scene
    [SerializeField] private string winnerSceneName = "WinnerScene"; // Name of the winner scene
    StatusBar healthBar;

    private float currentHealth; // Current health of the base object

    private void Awake()
    {
        healthBar = GetComponentInChildren<StatusBar>();
    }

    private void Start()
    {
        currentHealth = maxHealth; // Initialize current health to maximum health at the start
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount; // Decrease current health by the specified amount
        healthBar.UpdateStatusBar(currentHealth, maxHealth);

        // Check if current health is less than or equal to zero
        if (currentHealth <= 0)
        {
            if (gameObject.CompareTag("Enemy"))
            {
                SceneManager.LoadScene(winnerSceneName); // Load the winner scene if the destroyed base has the "EnemyBase" tag
            }
            else if (gameObject.CompareTag("Summon"))
            {
                SceneManager.LoadScene(gameOverSceneName); // Load the game over scene if the destroyed base has the "SummonBase" tag
            }
            Destroy(gameObject); // Destroy the base object if health reaches zero
        }
    }
}

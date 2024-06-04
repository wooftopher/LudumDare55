using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerHealth : MonoBehaviour {
    [SerializeField] private float maxHealth = 100f; // Maximum player health
    [SerializeField] private float currentHealth; // Current player health

    // Getter for current player health
    public float CurrentHealth {
        get { return currentHealth; }
    }

    // Getter for maximum player health
    public float MaxHealth {
        get { return maxHealth; }
    }

    void Start() {
        // Initialize current health to maximum health at the start
        currentHealth = maxHealth;
    }

    // Method to decrease player health
    public void DecreaseHealth(float amount) {
        currentHealth -= amount;

        // Ensure current health doesn't go below zero
        currentHealth = Mathf.Max(currentHealth, 0f);
        currentHealth = Mathf.Min(currentHealth, maxHealth);

        // Debug.Log("Health" + currentHealth);
        // Check if player health is zero
        if (currentHealth == 0) {
            // Perform actions for player death (e.g., game over, respawn)
            Debug.Log("Player died!");
            SceneManager.LoadScene("GameOverScene"); // Load the game over scene if the destroyed base has the "SummonBase" tag

        }
    }

    // Method to increase player health
    public void IncreaseHealth(float amount) {
        currentHealth += amount;

        // Ensure current health doesn't exceed maximum health
        currentHealth = Mathf.Min(currentHealth, maxHealth);
        Debug.Log("Health" + currentHealth);
    }
}

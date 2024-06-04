using UnityEngine;

public class PlayerMana : MonoBehaviour
{
    [SerializeField] private float maxMana = 100f; // Maximum player mana
    [SerializeField]private float currentMana; // Current player mana
    [SerializeField] private float manaRegenRate = 1f; // Mana regeneration rate per second

    

    // Getter for current player mana
    public float CurrentMana {
        get { return currentMana; }
    }

    // Getter for maximum player mana
    public float MaxMana {
        get { return maxMana; }
    }

    private void Start()
    {
        // Initialize current mana to maximum mana at the start
        currentMana = maxMana;
    }

    private void Update()
    {
        // Regenerate mana over time
        IncreaseMana(manaRegenRate * Time.deltaTime);
    }

    // Method to decrease player mana
    public void DecreaseMana(float amount)
    {
        currentMana -= amount;

        // Ensure current mana doesn't go below zero
        currentMana = Mathf.Max(currentMana, 0f);
    }

    // Method to increase player mana
    public void IncreaseMana(float amount)
    {
        currentMana += amount;

        // Ensure current mana doesn't exceed maximum mana
        currentMana = Mathf.Min(currentMana, maxMana);
    }
}

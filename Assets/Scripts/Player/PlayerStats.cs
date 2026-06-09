using UnityEngine;

/// <summary>
/// PlayerStats.cs
/// Manages all player survival stats: Hunger, Thirst, Energy, Health
/// When any stat reaches 0, the player dies
/// </summary>

public class PlayerStats : MonoBehaviour
{
    // ===== STAT VALUES =====
    // These are the main stats that keep player alive
    [SerializeField] private float maxHealth = 100f;      // Maximum health
    [SerializeField] private float maxHunger = 100f;      // Maximum hunger (fullness)
    [SerializeField] private float maxThirst = 100f;      // Maximum thirst (hydration)
    [SerializeField] private float maxEnergy = 100f;      // Maximum energy
    
    // Current values of each stat
    private float currentHealth;
    private float currentHunger;
    private float currentThirst;
    private float currentEnergy;
    
    // ===== STAT DECREASE RATES =====
    // How fast each stat decreases per second
    [SerializeField] private float hungerDecreaseRate = 5f;    // Hunger decreases slowly
    [SerializeField] private float thirstDecreaseRate = 7f;    // Thirst decreases faster than hunger
    [SerializeField] private float energyDecreaseRate = 3f;    // Energy decreases slowly
    [SerializeField] private float sprintEnergyDrain = 10f;    // Extra energy lost when sprinting
    [SerializeField] private float energyRecoverRate = 2f;     // Energy recovers when resting
    
    // ===== DAMAGE VALUES =====
    // How much damage different things do
    [SerializeField] private float hungerDamage = 0.5f;        // Damage per second when starving
    [SerializeField] private float thirstDamage = 0.8f;        // Damage per second when dehydrated
    [SerializeField] private float normalDamage = 1f;          // Damage from enemies
    
    // ===== RECOVERY VALUES =====
    // How much stats recover when eating/drinking
    [SerializeField] private float foodRestoration = 30f;      // How much food restores hunger
    [SerializeField] private float waterRestoration = 40f;     // How much water restores thirst
    [SerializeField] private float healthRestoration = 20f;    // How much eating restores health
    
    // ===== GAME REFERENCE =====
    private bool isAlive = true;                               // Is player still alive?
    private PlayerController playerController;                 // Reference to movement script
    
    private void Start()
    {
        // Initialize all stats to maximum
        currentHealth = maxHealth;
        currentHunger = maxHunger;
        currentThirst = maxThirst;
        currentEnergy = maxEnergy;
        
        // Get reference to player controller for sprint checking
        playerController = GetComponent<PlayerController>();
    }
    
    private void Update()
    {
        // Only update if player is alive
        if (!isAlive)
            return;
        
        // Decrease hunger, thirst, energy over time
        DecreaseStats();
        
        // If hunger or thirst gets too low, take damage
        CheckStarvation();
        
        // If any stat is 0 or below, die
        CheckIfDead();
    }
    
    /// <summary>
    /// DecreaseStats() - Decreases hunger, thirst, and energy each frame
    /// This makes the player need to find food and water to survive
    /// </summary>
    private void DecreaseStats()
    {
        // Decrease hunger slowly
        currentHunger -= hungerDecreaseRate * Time.deltaTime;
        
        // Decrease thirst (faster than hunger)
        currentThirst -= thirstDecreaseRate * Time.deltaTime;
        
        // Decrease energy based on movement
        // If sprinting, drain energy faster
        float energyDrain = energyDecreaseRate;
        
        // Check if player is sprinting (moving fast)
        if (Input.GetMouseButton(1) && playerController.IsGrounded())
        {
            energyDrain += sprintEnergyDrain;  // Extra drain while sprinting
        }
        
        currentEnergy -= energyDrain * Time.deltaTime;
        
        // Energy recovers when resting (not moving)
        if (playerController.GetCurrentSpeed() < 1f)
        {
            currentEnergy += energyRecoverRate * Time.deltaTime;
        }
        
        // Clamp stats so they don't go above maximum
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        currentHunger = Mathf.Clamp(currentHunger, 0, maxHunger);
        currentThirst = Mathf.Clamp(currentThirst, 0, maxThirst);
        currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);
    }
    
    /// <summary>
    /// CheckStarvation() - Takes damage if hunger or thirst is too low
    /// This adds urgency to finding food and water
    /// </summary>
    private void CheckStarvation()
    {
        // If very hungry, take damage
        if (currentHunger < 20f)
        {
            TakeDamage(hungerDamage * Time.deltaTime);
        }
        
        // If very thirsty, take damage
        if (currentThirst < 20f)
        {
            TakeDamage(thirstDamage * Time.deltaTime);
        }
    }
    
    /// <summary>
    /// CheckIfDead() - Ends game if any critical stat is 0
    /// </summary>
    private void CheckIfDead()
    {
        if (currentHealth <= 0)
        {
            Die("You died from injuries!");
        }
        else if (currentHunger <= 0)
        {
            Die("You starved to death!");
        }
        else if (currentThirst <= 0)
        {
            Die("You died of dehydration!");
        }
    }
    
    /// <summary>
    /// TakeDamage(float damage) - Reduces health by damage amount
    /// Called when attacked by enemies or from starvation
    /// </summary>
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log("Player took " + damage + " damage! Health: " + currentHealth);
    }
    
    /// <summary>
    /// Eat(float amount) - Restores hunger and health when eating food
    /// </summary>
    public void Eat(float amount = 1f)
    {
        currentHunger += foodRestoration * amount;
        currentHealth += healthRestoration * amount;
        Debug.Log("Player ate! Hunger: " + currentHunger + ", Health: " + currentHealth);
    }
    
    /// <summary>
    /// Drink(float amount) - Restores thirst when drinking water
    /// </summary>
    public void Drink(float amount = 1f)
    {
        currentThirst += waterRestoration * amount;
        Debug.Log("Player drank! Thirst: " + currentThirst);
    }
    
    /// <summary>
    /// Die(string reason) - Called when player dies
    /// </summary>
    private void Die(string reason)
    {
        isAlive = false;
        Debug.Log("GAME OVER! " + reason);
        Time.timeScale = 0f; // Freeze the game
    }
    
    // ===== GETTER FUNCTIONS =====
    // These let other scripts check player stats
    
    public float GetHealth() { return currentHealth; }
    public float GetHealthPercent() { return (currentHealth / maxHealth) * 100f; }
    
    public float GetHunger() { return currentHunger; }
    public float GetHungerPercent() { return (currentHunger / maxHunger) * 100f; }
    
    public float GetThirst() { return currentThirst; }
    public float GetThirstPercent() { return (currentThirst / maxThirst) * 100f; }
    
    public float GetEnergy() { return currentEnergy; }
    public float GetEnergyPercent() { return (currentEnergy / maxEnergy) * 100f; }
    
    public bool IsAlive() { return isAlive; }
}
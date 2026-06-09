using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// UIManager.cs
/// Manages all UI elements: health bars, stat displays
/// </summary>

public class UIManager : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    [SerializeField] private Image hungerBar;
    [SerializeField] private Image thirstBar;
    [SerializeField] private Image energyBar;
    
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI hungerText;
    [SerializeField] private TextMeshProUGUI thirstText;
    [SerializeField] private TextMeshProUGUI energyText;
    
    [SerializeField] private TextMeshProUGUI statusText;
    
    [SerializeField] private Color healthColor = Color.green;
    [SerializeField] private Color warningColor = Color.yellow;
    [SerializeField] private Color dangerColor = Color.red;
    
    private PlayerStats playerStats;
    
    private void Start()
    {
        playerStats = FindObjectOfType<PlayerStats>();
        
        if (playerStats == null)
        {
            Debug.LogError("UIManager: Could not find PlayerStats!");
        }
    }
    
    private void Update()
    {
        if (playerStats != null)
        {
            UpdateHealthBar();
            UpdateHungerBar();
            UpdateThirstBar();
            UpdateEnergyBar();
            UpdateStatusText();
        }
    }
    
    private void UpdateHealthBar()
    {
        float healthPercent = playerStats.GetHealthPercent() / 100f;
        healthBar.fillAmount = healthPercent;
        healthText.text = playerStats.GetHealth().ToString("F0") + " HP";
        
        if (healthPercent > 0.5f)
            healthBar.color = healthColor;
        else if (healthPercent > 0.25f)
            healthBar.color = warningColor;
        else
            healthBar.color = dangerColor;
    }
    
    private void UpdateHungerBar()
    {
        float hungerPercent = playerStats.GetHungerPercent() / 100f;
        hungerBar.fillAmount = hungerPercent;
        hungerText.text = playerStats.GetHunger().ToString("F0") + " Hunger";
        
        if (hungerPercent > 0.5f)
            hungerBar.color = healthColor;
        else if (hungerPercent > 0.25f)
            hungerBar.color = warningColor;
        else
            hungerBar.color = dangerColor;
    }
    
    private void UpdateThirstBar()
    {
        float thirstPercent = playerStats.GetThirstPercent() / 100f;
        thirstBar.fillAmount = thirstPercent;
        thirstText.text = playerStats.GetThirst().ToString("F0") + " Thirst";
        
        if (thirstPercent > 0.5f)
            thirstBar.color = healthColor;
        else if (thirstPercent > 0.25f)
            thirstBar.color = warningColor;
        else
            thirstBar.color = dangerColor;
    }
    
    private void UpdateEnergyBar()
    {
        float energyPercent = playerStats.GetEnergyPercent() / 100f;
        energyBar.fillAmount = energyPercent;
        energyText.text = playerStats.GetEnergy().ToString("F0") + " Energy";
        
        if (energyPercent > 0.5f)
            energyBar.color = healthColor;
        else if (energyPercent > 0.25f)
            energyBar.color = warningColor;
        else
            energyBar.color = dangerColor;
    }
    
    private void UpdateStatusText()
    {
        string status = "Surviving...";
        
        if (playerStats.GetHunger() < 30f)
            status = "⚠️ HUNGRY - Find food!";
        else if (playerStats.GetThirst() < 30f)
            status = "⚠️ THIRSTY - Find water!";
        else if (playerStats.GetHealth() < 30f)
            status = "⚠️ INJURED - Rest and eat!";
        else if (playerStats.GetEnergy() < 30f)
            status = "💤 TIRED - Rest and recover!";
        
        statusText.text = status;
    }
}
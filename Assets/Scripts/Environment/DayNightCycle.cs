using UnityEngine;

/// <summary>
/// DayNightCycle.cs
/// Creates a day/night cycle that affects the environment
/// </summary>

public class DayNightCycle : MonoBehaviour
{
    [SerializeField] private float dayLength = 120f;
    [SerializeField] private Light sunLight;
    
    [SerializeField] private Color dayColor = Color.white;
    [SerializeField] private Color nightColor = new Color(0.3f, 0.3f, 0.5f);
    
    [SerializeField] private float dayIntensity = 1f;
    [SerializeField] private float nightIntensity = 0.2f;
    
    private float currentTime = 0f;
    
    private void Update()
    {
        currentTime += Time.deltaTime;
        
        if (currentTime > dayLength)
        {
            currentTime = 0f;
        }
        
        float timePercent = currentTime / dayLength;
        
        float sunRotation = timePercent * 360f;
        sunLight.transform.rotation = Quaternion.Euler(sunRotation - 90f, 0f, 0f);
        
        sunLight.color = Color.Lerp(nightColor, dayColor, 
                                    Mathf.Sin(timePercent * Mathf.PI));
        
        sunLight.intensity = Mathf.Lerp(nightIntensity, dayIntensity,
                                        Mathf.Sin(timePercent * Mathf.PI));
    }
    
    public bool IsNight()
    {
        float timePercent = currentTime / dayLength;
        return timePercent > 0.75f || timePercent < 0.25f;
    }
    
    public float GetTimePercent()
    {
        return currentTime / dayLength;
    }
}
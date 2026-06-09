using UnityEngine;

/// <summary>
/// Food.cs
/// Represents food items that player can eat
/// </summary>

public class Food : MonoBehaviour
{
    [SerializeField] private float foodValue = 30f;
    
    private float bobHeight = 0.5f;
    private float bobSpeed = 2f;
    private Vector3 startPosition;
    
    private void Start()
    {
        startPosition = transform.position;
        GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 2f, 0);
    }
    
    private void Update()
    {
        float newY = startPosition.y + Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }
    
    public float GetFoodValue()
    {
        return foodValue;
    }
}
using UnityEngine;

/// <summary>
/// Water.cs
/// Represents water sources that player can drink from
/// </summary>

public class Water : MonoBehaviour
{
    [SerializeField] private float waterRestoration = 40f;
    [SerializeField] private float refreshRate = 2f;
    
    private Renderer waterRenderer;
    
    private void Start()
    {
        waterRenderer = GetComponent<Renderer>();
    }
    
    private void Update()
    {
        Material waterMat = waterRenderer.material;
        waterMat.mainTextureOffset += new Vector2(0.01f, 0.01f) * Time.deltaTime;
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerCombat combat = other.GetComponent<PlayerCombat>();
            if (combat != null)
            {
                combat.PickUpWater();
            }
        }
    }
    
    public float GetWaterValue()
    {
        return waterRestoration;
    }
}
using UnityEngine;

/// <summary>
/// PlayerCombat.cs
/// Handles player attacking and eating food
/// </summary>

public class PlayerCombat : MonoBehaviour
{
    // ===== COMBAT SETTINGS =====
    [SerializeField] private float attackRange = 3f;
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float attackCooldown = 0.5f;
    [SerializeField] private float attackRadius = 2f;
    
    private float lastAttackTime = 0f;
    private PlayerStats playerStats;
    private Camera playerCamera;
    
    private void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        playerCamera = GetComponentInChildren<Camera>();
    }
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }
    
    private void Attack()
    {
        if (Time.time - lastAttackTime < attackCooldown)
            return;
        
        lastAttackTime = Time.time;
        Debug.Log("Player attacked!");
        
        Vector3 attackOrigin = playerCamera.transform.position;
        Vector3 attackDirection = playerCamera.transform.forward;
        
        Collider[] hitObjects = Physics.OverlapSphere(
            attackOrigin + attackDirection * attackRange,
            attackRadius
        );
        
        foreach (Collider hit in hitObjects)
        {
            if (hit.gameObject == gameObject)
                continue;
            
            EnemyAI enemy = hit.GetComponent<EnemyAI>();
            if (enemy != null)
            {
                enemy.TakeDamage(attackDamage);
                Debug.Log("Hit enemy!");
            }
            
            Food food = hit.GetComponent<Food>();
            if (food != null)
            {
                playerStats.Eat(1f);
                Destroy(hit.gameObject);
                Debug.Log("Ate food!");
            }
        }
    }
    
    public void PickUpWater()
    {
        playerStats.Drink(1f);
        Debug.Log("Drank water!");
    }
}
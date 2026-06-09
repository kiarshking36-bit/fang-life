using UnityEngine;

/// <summary>
/// PlayerController.cs
/// Handles all player movement, camera control, and input
/// This script makes your character move around the game world
/// </summary>

public class PlayerController : MonoBehaviour
{
    // ===== MOVEMENT SETTINGS =====
    // These numbers control how fast the player moves
    [SerializeField] private float walkSpeed = 5f;           // Normal walking speed
    [SerializeField] private float sprintSpeed = 10f;        // Running speed (when holding Right Click)
    [SerializeField] private float jumpForce = 5f;           // How high the player jumps
    [SerializeField] private float groundDrag = 5f;          // Ground friction
    [SerializeField] private float airDrag = 2f;             // Air friction when jumping
    
    // ===== CAMERA SETTINGS =====
    // These control how the camera looks around
    [SerializeField] private float mouseSensitivity = 2f;    // How fast camera rotates with mouse
    [SerializeField] private float maxLookAngle = 90f;       // Max angle to look up/down
    
    // ===== GROUND CHECK =====
    [SerializeField] private float groundDist = 0.2f;        // How far down to check for ground
    [SerializeField] private LayerMask groundLayer;          // Which layer is "ground"
    
    // ===== PRIVATE VARIABLES =====
    private Rigidbody rb;                                    // The physics body
    private Camera playerCamera;                             // The camera attached to player
    private float xRotation = 0f;                            // Up/down camera rotation
    private bool isGrounded;                                 // Is player touching ground?
    private Vector3 moveDirection;                           // Which way to move
    private float currentSpeed;                              // Current speed (walk or sprint)
    
    private void Start()
    {
        // Get the Rigidbody component on this player
        rb = GetComponent<Rigidbody>();
        
        // Get the Camera component (child of player)
        playerCamera = GetComponentInChildren<Camera>();
        
        // Lock the cursor to the center of screen
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    private void Update()
    {
        // === CHECK IF ON GROUND ===
        // Draw an invisible line down from player to check for ground
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundDist, groundLayer);
        
        // === HANDLE MOVEMENT INPUT ===
        // Get input from keyboard (WASD keys)
        float horizontalInput = Input.GetAxis("Horizontal");  // A/D keys
        float verticalInput = Input.GetAxis("Vertical");      // W/S keys
        
        // === HANDLE SPRINTING ===
        // Check if player is holding Right Mouse Button to sprint
        if (Input.GetMouseButton(1) && isGrounded)
        {
            currentSpeed = sprintSpeed;  // Run faster
        }
        else
        {
            currentSpeed = walkSpeed;    // Normal walking speed
        }
        
        // === CALCULATE MOVEMENT DIRECTION ===
        // Forward direction (where player is looking)
        moveDirection = transform.forward * verticalInput + transform.right * horizontalInput;
        
        // === HANDLE JUMPING ===
        // Check if Space is pressed and player is on ground
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();  // Make player jump
        }
        
        // === HANDLE CAMERA LOOK ===
        HandleCameraLook();
        
        // === UPDATE PHYSICS DRAG ===
        // If in air, reduce friction (so player slides more)
        // If on ground, increase friction (so player doesn't slide as much)
        rb.drag = isGrounded ? groundDrag : airDrag;
    }
    
    private void FixedUpdate()
    {
        // Apply movement to the physics body
        // This happens in FixedUpdate for better physics consistency
        MovePlayer();
    }
    
    /// <summary>
    /// MovePlayer() - Applies movement to the player's rigidbody
    /// This happens every physics frame for smooth movement
    /// </summary>
    private void MovePlayer()
    {
        // Apply force in the direction the player should move
        // The force is applied at normal speed (not including jump)
        rb.AddForce(moveDirection.normalized * currentSpeed * 10f, ForceMode.Force);
        
        // Limit maximum speed so player doesn't go too fast
        // Get current velocity
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        
        // If going faster than allowed, slow down
        if (flatVel.magnitude > currentSpeed)
        {
            // Cap the speed
            Vector3 limitedVel = flatVel.normalized * currentSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }
    
    /// <summary>
    /// Jump() - Makes the player jump up into the air
    /// This adds upward force to the rigidbody
    /// </summary>
    private void Jump()
    {
        // Add upward force to make player jump
        // ForceMode.Impulse applies it instantly like a punch
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z); // Reset vertical velocity
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
    
    /// <summary>
    /// HandleCameraLook() - Makes camera rotate with mouse movement
    /// This lets the player look around by moving the mouse
    /// </summary>
    private void HandleCameraLook()
    {
        // Get mouse movement
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;  // Left/Right
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;  // Up/Down
        
        // Rotate body left/right (yaw)
        transform.Rotate(Vector3.up * mouseX);
        
        // Rotate camera up/down (pitch)
        xRotation -= mouseY;
        
        // Clamp so player can't look too far up or down
        xRotation = Mathf.Clamp(xRotation, -maxLookAngle, maxLookAngle);
        
        // Apply the camera rotation
        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
    
    /// <summary>
    /// GetCurrentSpeed() - Returns how fast the player is currently moving
    /// Used by other scripts that need to know the player's speed
    /// </summary>
    public float GetCurrentSpeed()
    {
        return currentSpeed;
    }
    
    /// <summary>
    /// IsGrounded() - Returns true if player is touching ground
    /// Used to check if player can perform ground-only actions
    /// </summary>
    public bool IsGrounded()
    {
        return isGrounded;
    }
}
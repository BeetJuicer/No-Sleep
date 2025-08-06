using UnityEngine;

public class DirectionalMover : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private LayerMask obstacleLayerMask = -1; // What layers count as obstacles
    [SerializeField] private float raycastDistance = 0.6f; // How far ahead to check for obstacles
    
    [Header("Direction Sprites")]
    [SerializeField] private Sprite upSprite;
    [SerializeField] private Sprite downSprite;
    [SerializeField] private Sprite leftSprite;
    [SerializeField] private Sprite rightSprite;
    
    [Header("Debug")]
    [SerializeField] private bool showDebugRays = true;
    [SerializeField] private Color rayColor = Color.red;
    
    // Private variables
    private Vector2 currentDirection;
    private Vector2[] possibleDirections = {
        Vector2.up,      // Up
        Vector2.down,    // Down
        Vector2.left,    // Left
        Vector2.right    // Right
    };
    
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private bool isMoving = true;
    
    // Direction enum for cleaner code
    private enum Direction
    {
        Up = 0,
        Down = 1,
        Left = 2,
        Right = 3
    }
    
    private void Start()
    {
        InitializeComponents();
        ChooseRandomDirection();
        UpdateSpriteForDirection();
    }
    
    private void InitializeComponents()
    {
        // Get required components
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        
        // Add components if missing
        if (spriteRenderer == null)
        {
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        }
        
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
        
        // Configure Rigidbody2D for smooth movement
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
    }
    
    private void Update()
    {
        if (isMoving)
        {
            CheckForObstacles();
            MoveInCurrentDirection();
        }
    }
    
    private void MoveInCurrentDirection()
    {
        // Move using Rigidbody2D for smooth physics-based movement
        rb.linearVelocity = currentDirection * moveSpeed;
    }
    
    private void CheckForObstacles()
    {
        // Cast a ray in the current direction to detect obstacles
        Vector2 rayOrigin = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, currentDirection, raycastDistance, obstacleLayerMask);
        
        // Debug visualization
        if (showDebugRays)
        {
            Debug.DrawRay(rayOrigin, currentDirection * raycastDistance, rayColor);
        }
        
        // If we hit something, change direction
        if (hit.collider != null && hit.collider.gameObject != gameObject)
        {
            ChangeDirection();
        }
    }
    
    private void ChangeDirection()
    {
        Vector2 newDirection;
        int attempts = 0;
        int maxAttempts = 10; // Prevent infinite loop
        
        do
        {
            // Choose a random direction that's different from current
            int randomIndex = Random.Range(0, possibleDirections.Length);
            newDirection = possibleDirections[randomIndex];
            attempts++;
            
        } while (newDirection == currentDirection && attempts < maxAttempts);
        
        // If we couldn't find a different direction, reverse current direction
        if (attempts >= maxAttempts)
        {
            newDirection = -currentDirection;
        }
        
        currentDirection = newDirection;
        UpdateSpriteForDirection();
        
        // Optional: Add a small delay to prevent rapid direction changes
        StartCoroutine(DirectionChangeDelay());
    }
    
    private System.Collections.IEnumerator DirectionChangeDelay()
    {
        isMoving = false;
        yield return new WaitForSeconds(0.1f);
        isMoving = true;
    }
    
    private void UpdateSpriteForDirection()
    {
        if (spriteRenderer == null) return;
        
        // Determine which sprite to use based on current direction
        if (currentDirection == Vector2.up)
        {
            spriteRenderer.sprite = upSprite;
        }
        else if (currentDirection == Vector2.down)
        {
            spriteRenderer.sprite = downSprite;
        }
        else if (currentDirection == Vector2.left)
        {
            spriteRenderer.sprite = leftSprite;
        }
        else if (currentDirection == Vector2.right)
        {
            spriteRenderer.sprite = rightSprite;
        }
    }
    
    private void ChooseRandomDirection()
    {
        int randomIndex = Random.Range(0, possibleDirections.Length);
        currentDirection = possibleDirections[randomIndex];
    }
    
    // Collision detection as backup method
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If raycast didn't catch it, this will
        if (isMoving)
        {
            ChangeDirection();
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Alternative for trigger colliders
        if (isMoving)
        {
            ChangeDirection();
        }
    }
    
    #region Public Methods for External Control
    
    public void SetSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
    }
    
    public void StartMoving()
    {
        isMoving = true;
    }
    
    public void StopMoving()
    {
        isMoving = false;
        rb.linearVelocity = Vector2.zero;
    }
    
    public void ForceChangeDirection()
    {
        ChangeDirection();
    }
    
    public void SetDirection(Vector2 newDirection)
    {
        // Ensure it's a valid cardinal direction
        if (newDirection == Vector2.up || newDirection == Vector2.down || 
            newDirection == Vector2.left || newDirection == Vector2.right)
        {
            currentDirection = newDirection;
            UpdateSpriteForDirection();
        }
    }
    
    public Vector2 GetCurrentDirection()
    {
        return currentDirection;
    }
    
    public void SetSprites(Sprite up, Sprite down, Sprite left, Sprite right)
    {
        upSprite = up;
        downSprite = down;
        leftSprite = left;
        rightSprite = right;
        UpdateSpriteForDirection();
    }
    
    #endregion
    
    #region Debug Visualization
    
    private void OnDrawGizmos()
    {
        if (showDebugRays && Application.isPlaying)
        {
            Gizmos.color = rayColor;
            Vector3 rayStart = transform.position;
            Vector3 rayEnd = rayStart + (Vector3)(currentDirection * raycastDistance);
            Gizmos.DrawLine(rayStart, rayEnd);
            Gizmos.DrawSphere(rayEnd, 0.1f);
        }
    }
    
    #endregion
}
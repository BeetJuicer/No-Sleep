using System.Collections;
using UnityEngine;

public class ErraticOrbiter2D : MonoBehaviour
{
    [Header("Center Point")]
    [SerializeField] private Transform centerTransform;
    [SerializeField] private Vector2 centerOffset = Vector2.zero;
    
    [Header("Movement Settings")]
    [SerializeField] private float baseSpeed = 2f;
    [SerializeField] private float speedVariation = 1f;
    [SerializeField] private float minRadius = 1f;
    [SerializeField] private float maxRadius = 3f;
    [SerializeField] private float radiusChangeSpeed = 1f;
    
    [Header("Erratic Behavior")]
    [SerializeField] private float jitterIntensity = 0.5f;
    [SerializeField] private float jitterFrequency = 5f;
    [SerializeField] private float directionChangeFrequency = 2f;
    [SerializeField] private float pauseProbability = 0.1f;
    [SerializeField] private float maxPauseDuration = 1f;
    
    [Header("Settings")]
    [SerializeField] private bool startOnAwake = true;
    [SerializeField] private bool useLocalPosition = false;
    
    private Vector2 centerPosition;
    private float currentAngle = 0f;
    private float currentRadius;
    private float targetRadius;
    private float currentSpeed;
    private float targetSpeed;
    private Vector2 jitterOffset;
    private bool isMoving = true;
    private Coroutine movementCoroutine;
    private Coroutine behaviorCoroutine;
    
    void Awake()
    {
        // Initialize values
        currentRadius = Random.Range(minRadius, maxRadius);
        targetRadius = currentRadius;
        currentSpeed = baseSpeed;
        targetSpeed = baseSpeed;
        currentAngle = Random.Range(0f, 360f);
    }
    
    void Start()
    {
        if (startOnAwake)
        {
            StartMovement();
        }
    }
    
    void Update()
    {
        UpdateCenterPosition();
        
        if (isMoving)
        {
            UpdateMovement();
        }
    }
    
    /// <summary>
    /// Starts the erratic movement
    /// </summary>
    public void StartMovement()
    {
        isMoving = true;
        
        if (movementCoroutine != null)
        {
            StopCoroutine(movementCoroutine);
        }
        
        if (behaviorCoroutine != null)
        {
            StopCoroutine(behaviorCoroutine);
        }
        
        behaviorCoroutine = StartCoroutine(ErraticBehaviorCoroutine());
    }
    
    /// <summary>
    /// Stops the erratic movement
    /// </summary>
    public void StopMovement()
    {
        isMoving = false;
        
        if (movementCoroutine != null)
        {
            StopCoroutine(movementCoroutine);
            movementCoroutine = null;
        }
        
        if (behaviorCoroutine != null)
        {
            StopCoroutine(behaviorCoroutine);
            behaviorCoroutine = null;
        }
    }
    
    /// <summary>
    /// Sets a new center transform
    /// </summary>
    public void SetCenter(Transform newCenter)
    {
        centerTransform = newCenter;
    }
    
    /// <summary>
    /// Sets a new center position directly
    /// </summary>
    public void SetCenterPosition(Vector2 newCenter)
    {
        centerTransform = null;
        centerPosition = newCenter;
    }
    
    /// <summary>
    /// Triggers an immediate random movement change
    /// </summary>
    public void RandomizeMovement()
    {
        targetRadius = Random.Range(minRadius, maxRadius);
        targetSpeed = baseSpeed + Random.Range(-speedVariation, speedVariation);
        
        // Random chance to change direction
        if (Random.value > 0.5f)
        {
            currentSpeed *= Random.Range(0.3f, 1.7f);
        }
    }
    
    private void UpdateCenterPosition()
    {
        if (centerTransform != null)
        {
            Vector2 worldCenter = useLocalPosition ? 
                centerTransform.localPosition : 
                centerTransform.position;
            centerPosition = worldCenter + centerOffset;
        }
    }
    
    private void UpdateMovement()
    {
        // Smoothly interpolate radius and speed
        currentRadius = Mathf.Lerp(currentRadius, targetRadius, radiusChangeSpeed * Time.deltaTime);
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime * 2f);
        
        // Update angle based on current speed
        currentAngle += currentSpeed * Time.deltaTime;
        
        // Calculate base position around center
        float radians = currentAngle * Mathf.Deg2Rad;
        Vector2 basePosition = centerPosition + new Vector2(
            Mathf.Cos(radians) * currentRadius,
            Mathf.Sin(radians) * currentRadius
        );
        
        // Add jitter for erratic movement
        Vector2 finalPosition = basePosition + jitterOffset;
        
        // Apply position
        if (useLocalPosition)
        {
            transform.localPosition = finalPosition;
        }
        else
        {
            transform.position = finalPosition;
        }
    }
    
    private IEnumerator ErraticBehaviorCoroutine()
    {
        while (true)
        {
            // Update jitter
            yield return StartCoroutine(UpdateJitter());
            
            // Random direction changes
            if (Random.value < directionChangeFrequency * Time.fixedDeltaTime)
            {
                RandomizeMovement();
            }
            
            // Random pauses
            if (Random.value < pauseProbability * Time.fixedDeltaTime)
            {
                yield return StartCoroutine(PauseMovement());
            }
            
            yield return new WaitForFixedUpdate();
        }
    }
    
    private IEnumerator UpdateJitter()
    {
        float jitterTimer = 0f;
        float jitterDuration = 1f / jitterFrequency;
        
        while (jitterTimer < jitterDuration)
        {
            // Create smooth jitter using Perlin noise
            float noiseX = Mathf.PerlinNoise(Time.time * jitterFrequency, 0f) - 0.5f;
            float noiseY = Mathf.PerlinNoise(0f, Time.time * jitterFrequency) - 0.5f;
            
            jitterOffset = new Vector2(noiseX, noiseY) * jitterIntensity;
            
            jitterTimer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
    }
    
    private IEnumerator PauseMovement()
    {
        float pauseDuration = Random.Range(0.1f, maxPauseDuration);
        float originalSpeed = currentSpeed;
        
        // Gradually slow down
        while (currentSpeed > 0.1f)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, 0f, Time.deltaTime * 5f);
            yield return null;
        }
        
        // Pause
        currentSpeed = 0f;
        yield return new WaitForSeconds(pauseDuration * 0.5f);
        
        // Gradually speed back up
        while (currentSpeed < originalSpeed * 0.9f)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, originalSpeed, Time.deltaTime * 3f);
            yield return null;
        }
        
        currentSpeed = originalSpeed;
    }
    
    void OnDestroy()
    {
        StopMovement();
    }
    
    //void OnDrawGizmosSelected()
    //{
    //    if (centerTransform != null || centerPosition != Vector2.zero)
    //    {
    //        Vector2 center = centerTransform != null ? 
    //            (Vector2)centerTransform.position + centerOffset : 
    //            centerPosition;
            
    //        // Draw center point
    //        Gizmos.color = Color.red;
    //        Gizmos.DrawWireSphere(center, 0.1f);
            
    //        // Draw min/max radius circles
    //        Gizmos.color = Color.yellow;
    //        Gizmos.DrawWireCircle(center, minRadius);
            
    //        Gizmos.color = Color.orange;
    //        Gizmos.DrawWireCircle(center, maxRadius);
            
    //        // Draw line to current object position
    //        Gizmos.color = Color.green;
    //        Gizmos.DrawLine(center, transform.position);
    //    }
    //}
    
    #if UNITY_EDITOR
    void OnValidate()
    {
        if (minRadius < 0f) minRadius = 0f;
        if (maxRadius < minRadius) maxRadius = minRadius;
        if (baseSpeed < 0f) baseSpeed = 0f;
        if (speedVariation < 0f) speedVariation = 0f;
        if (jitterIntensity < 0f) jitterIntensity = 0f;
        if (jitterFrequency < 0.1f) jitterFrequency = 0.1f;
        if (radiusChangeSpeed < 0f) radiusChangeSpeed = 0f;
    }
    #endif
}
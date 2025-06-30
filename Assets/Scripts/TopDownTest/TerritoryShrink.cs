using UnityEngine;

public class TerritoryShrink : MonoBehaviour
{
    [Header("Shrink Settings")]
    [SerializeField] private float shrinkRate = 0.1f; // Units per second
    [SerializeField] private float minimumScale = 0.1f; // Minimum scale before stopping
    [SerializeField] private bool shrinkOnStart = true;

    [Header("Advanced Settings")]
    [SerializeField] private bool useTimeBasedShrinking = false;
    [SerializeField] private float shrinkInterval = 1f; // Time between shrink steps
    [SerializeField] private float shrinkAmount = 0.05f; // Amount to shrink each interval

    [Header("Optional Components")]
    [SerializeField] private bool affectCollider = true;
    [SerializeField] private bool smoothShrinking = true;

    private Vector3 initialScale;
    private float timer = 0f;
    private bool isShrinking = false;
    private Collider2D col2D;
    private Collider col3D;

    void Start()
    {
        initialScale = transform.localScale;
        isShrinking = shrinkOnStart;

        // Get collider components for optional scaling
        col2D = GetComponent<Collider2D>();
        col3D = GetComponent<Collider>();
    }

    void Update()
    {
        if (!isShrinking) return;

        if (useTimeBasedShrinking)
        {
            UpdateIntervalShrinking();
        }
        else
        {
            UpdateContinuousShrinking();
        }

        // Update collider if needed
        if (affectCollider)
        {
            UpdateCollider();
        }
    }

    private void UpdateContinuousShrinking()
    {
        Vector3 currentScale = transform.localScale;
        float newScale = Mathf.Max(currentScale.x - (shrinkRate * Time.deltaTime), minimumScale);

        if (smoothShrinking)
        {
            transform.localScale = Vector3.Lerp(currentScale,
                new Vector3(newScale, newScale, currentScale.z), Time.deltaTime * 2f);
        }
        else
        {
            transform.localScale = new Vector3(newScale, newScale, currentScale.z);
        }

        // Stop shrinking if we've reached minimum
        if (newScale <= minimumScale)
        {
            isShrinking = false;
            OnMinimumScaleReached();
        }
    }

    private void UpdateIntervalShrinking()
    {
        timer += Time.deltaTime;

        if (timer >= shrinkInterval)
        {
            timer = 0f;
            Vector3 currentScale = transform.localScale;
            float newScale = Mathf.Max(currentScale.x - shrinkAmount, minimumScale);

            transform.localScale = new Vector3(newScale, newScale, currentScale.z);

            if (newScale <= minimumScale)
            {
                isShrinking = false;
                OnMinimumScaleReached();
            }
        }
    }

    private void UpdateCollider()
    {
        // This assumes you're using a CircleCollider2D for circular territory
        if (col2D is CircleCollider2D circleCol)
        {
            // Collider will scale automatically with transform, but you can modify radius here if needed
        }

        // For 3D sphere colliders
        if (col3D is SphereCollider sphereCol)
        {
            // Collider will scale automatically with transform
        }
    }

    // Public methods for external control
    public void StartShrinking()
    {
        isShrinking = true;
    }

    public void ExpandTerritory(float amount)
    {
        transform.localScale += Vector3.one * amount;
    }

    public void StopShrinking()
    {
        isShrinking = false;
    }

    public void ResetScale()
    {
        transform.localScale = initialScale;
        isShrinking = shrinkOnStart;
    }

    public void SetShrinkRate(float newRate)
    {
        shrinkRate = newRate;
    }

    public float GetCurrentScalePercentage()
    {
        return transform.localScale.x / initialScale.x;
    }

    // Called when minimum scale is reached
    private void OnMinimumScaleReached()
    {
        // Override this method or add UnityEvents here for custom behavior
        Debug.Log("Territory has reached minimum size!");

        // Example: Could trigger game over, special event, etc.
        // GameManager.Instance.OnTerritoryCollapsed();
    }

    // Optional: Visual feedback in Scene view
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, minimumScale);

        Gizmos.color = Color.yellow;
        if (Application.isPlaying)
        {
            Gizmos.DrawWireSphere(transform.position, transform.localScale.x);
        }
    }
}
using UnityEngine;

public class YPositionScaler : MonoBehaviour
{
    [SerializeField] private Transform targetToScale;       // The object to scale (e.g., player)
    [SerializeField] private bool lowerYMeansBigger = true; // If true: lower Y = bigger scale
    [SerializeField] private float addToScalePerYUnit = 1f;    // How much the Y position affects scale

    private Vector3 originalScale;
    private bool isPlayerInside = false;
    [SerializeField] private float maxScaleFactor;
    [SerializeField] private float  minScaleFactor;

    [SerializeField] private Transform topBound;
    [SerializeField] private Transform bottomBound;
    private float totalYDistance;

    private void Start()
    {
        if (targetToScale != null)
        {
            originalScale = targetToScale.localScale;
        }
        else
        {
            Debug.LogWarning("YPositionScaler: targetToScale is not assigned.");
        }

        totalYDistance = topBound.position.y - bottomBound.position.y;
    }

    private void Update()
    {
        if (isPlayerInside && targetToScale != null)
        {
            float yPos = targetToScale.position.y;
            //depending on how far up the player is, multiply it by the amount.
            float relativeDistance = Mathf.InverseLerp(topBound.position.y, bottomBound.position.y, yPos);//go from top to down, because we want higher y = lower scale
            float scaleMultiplier = Mathf.Lerp(minScaleFactor, maxScaleFactor, relativeDistance); 

            targetToScale.transform.localScale = originalScale * scaleMultiplier;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out TopDown.PlayerMovement p)) // Assumes the player has the "Player" tag
        {
            isPlayerInside = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out TopDown.PlayerMovement p))
        {
            isPlayerInside = false;
            if (targetToScale != null)
            {
                targetToScale.localScale = originalScale;
            }
        }
    }
}

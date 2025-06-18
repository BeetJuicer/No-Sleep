using UnityEngine;

public class EyelidController : MonoBehaviour
{
    // --- Public Fields ---
    // Drag your UI Image objects here in the Inspector
    public RectTransform topEyelid;
    public RectTransform bottomEyelid;

    // Drag the object with the PlayerHealth script here
    public Health playerHealth;

    // --- Private Fields ---
    private float screenHeight;
    private Vector2 topEyelidClosedPos;
    private Vector2 bottomEyelidClosedPos;
    private Vector2 topEyelidOpenPos;
    private Vector2 bottomEyelidOpenPos;

    void Start()
    {
        // Get the height of the canvas (screen)
        screenHeight = GetComponent<RectTransform>().rect.height;

        // --- Define the Open and Closed Positions ---

        // Open positions are where they start (at the very edge or just off-screen)
        topEyelidOpenPos = topEyelid.anchoredPosition;
        bottomEyelidOpenPos = bottomEyelid.anchoredPosition;

        // Closed positions are at the center of the screen
        // For a top-anchored object, Y=0 is the anchor point (top edge). We move it down.
        // For a bottom-anchored object, Y=0 is the anchor point (bottom edge). We move it up.
        // We go slightly past halfway to ensure a good overlap.
        topEyelidClosedPos = new Vector2(topEyelid.anchoredPosition.x, -screenHeight / 2);
        bottomEyelidClosedPos = new Vector2(bottomEyelid.anchoredPosition.x, screenHeight / 2);
    }

    void Update()
    {
        // Get the current health percentage (a value from 1.0 down to 0.0)
        float healthPercent = playerHealth.GetHealthPercentage();

        // We want to invert the percentage for the eyelid effect.
        // 100% health (1.0) = 0% closed.
        // 0% health (0.0) = 100% closed.
        float closedAmount = 1.0f - healthPercent;

        // Use Lerp (Linear Interpolation) to find the current position for each eyelid.
        // Lerp smoothly transitions between two points based on a third value (t).
        topEyelid.anchoredPosition = Vector2.Lerp(topEyelidOpenPos, topEyelidClosedPos, closedAmount);
        bottomEyelid.anchoredPosition = Vector2.Lerp(bottomEyelidOpenPos, bottomEyelidClosedPos, closedAmount);
    }
}
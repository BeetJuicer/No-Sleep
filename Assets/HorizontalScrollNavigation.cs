using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HorizontalScrollNavigation : MonoBehaviour
{
    [Header("UI References")]
    public ScrollRect scrollRect;
    public Button nextButton;
    public Button previousButton;

    [Header("Settings")]
    public float scrollDuration = 0.3f;
    public AnimationCurve scrollCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private int currentIndex = 0;
    private int totalItems;
    private bool isScrolling = false;

    void Start()
    {
        // Setup button listeners
        if (nextButton != null)
            nextButton.onClick.AddListener(NextItem);

        if (previousButton != null)
            previousButton.onClick.AddListener(PreviousItem);

        // Calculate total items based on content children
        if (scrollRect != null && scrollRect.content != null)
        {
            totalItems = scrollRect.content.childCount;
        }

        // Initialize at first item (index 0)
        currentIndex = 0;

        // Initial button state update
        UpdateButtonStates();
    }

    public void NextItem()
    {
        if (isScrolling || currentIndex >= totalItems - 1)
            return;

        currentIndex++;
        ScrollToItem(currentIndex);
    }

    public void PreviousItem()
    {
        if (isScrolling || currentIndex <= 0)
            return;

        currentIndex--;
        ScrollToItem(currentIndex);
    }

    public void ScrollToItem(int index)
    {
        if (scrollRect == null || scrollRect.content == null)
            return;

        // Clamp index to valid range
        index = Mathf.Clamp(index, 0, totalItems - 1);
        currentIndex = index;

        // Calculate target position (0 = leftmost, 1 = rightmost)
        float targetPosition = totalItems > 1 ? (float)index / (totalItems - 1) : 0f;

        StartCoroutine(SmoothScrollTo(targetPosition));
    }

    private IEnumerator SmoothScrollTo(float targetPosition)
    {
        isScrolling = true;
        UpdateButtonStates();

        float startPosition = scrollRect.horizontalNormalizedPosition;
        float elapsedTime = 0f;

        while (elapsedTime < scrollDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / scrollDuration;
            float curveValue = scrollCurve.Evaluate(progress);

            scrollRect.horizontalNormalizedPosition = Mathf.Lerp(startPosition, targetPosition, curveValue);
            yield return null;
        }

        scrollRect.horizontalNormalizedPosition = targetPosition;
        isScrolling = false;
        UpdateButtonStates();
    }

    private void UpdateButtonStates()
    {
        // Update next button
        if (nextButton != null)
        {
            nextButton.interactable = !isScrolling && currentIndex < totalItems - 1;
        }

        // Update previous button
        if (previousButton != null)
        {
            previousButton.interactable = !isScrolling && currentIndex > 0;
        }
    }

    // Public method to set current index manually
    public void SetCurrentIndex(int index)
    {
        if (index >= 0 && index < totalItems)
        {
            currentIndex = index;
            UpdateButtonStates();
        }
    }

    // Public method to refresh item count (call this if you add/remove items dynamically)
    public void RefreshItemCount()
    {
        if (scrollRect != null && scrollRect.content != null)
        {
            totalItems = scrollRect.content.childCount;

            // Reset to first item if we have items, or stay at 0 if no items
            if (totalItems > 0)
            {
                currentIndex = Mathf.Clamp(currentIndex, 0, totalItems - 1);
            }
            else
            {
                currentIndex = 0;
            }

            UpdateButtonStates();
        }
    }

    // Optional: Detect when user manually scrolls and update current index
    public void OnScrollValueChanged(Vector2 scrollPosition)
    {
        if (!isScrolling && totalItems > 1)
        {
            // Calculate closest item index based on scroll position
            float normalizedPosition = scrollRect.horizontalNormalizedPosition;
            int closestIndex = Mathf.RoundToInt(normalizedPosition * (totalItems - 1));

            if (closestIndex != currentIndex)
            {
                currentIndex = closestIndex;
                UpdateButtonStates();
            }
        }
    }

    void OnDestroy()
    {
        // Clean up button listeners
        if (nextButton != null)
            nextButton.onClick.RemoveListener(NextItem);

        if (previousButton != null)
            previousButton.onClick.RemoveListener(PreviousItem);
    }
}
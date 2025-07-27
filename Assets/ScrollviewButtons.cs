using UnityEngine;
using UnityEngine.UI;

public class ScrollviewButtons : MonoBehaviour
{

    [SerializeField] Button next;
    [SerializeField] Button previous;
    [SerializeField] ScrollRect scrollRect;

    //inventory specific
    [SerializeField] Inventory inventory;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inventory = FindFirstObjectByType<Inventory>();

        next.onClick.AddListener(Next);
        previous.onClick.AddListener(Previous);

    }

    private void Update()
    {
        if (scrollRect.horizontalNormalizedPosition <= 1f)
            next.interactable = true;
    }

    float CalculatePercentagePerItem()
    {
        if (scrollRect.content.childCount == 0)
        {
            Debug.LogError("Dividing by zero!");
            return -1;
        }

        return 1f / scrollRect.content.childCount;
    }

    void Next()
    {
        float percentage = CalculatePercentagePerItem();
        scrollRect.horizontalNormalizedPosition += percentage;

        next.interactable = (scrollRect.horizontalNormalizedPosition + percentage <= 1f);
        previous.interactable = true;
    }

    void Previous()
    {
        float percentage = CalculatePercentagePerItem();

        scrollRect.horizontalNormalizedPosition -= percentage;
        previous.interactable = (scrollRect.horizontalNormalizedPosition - percentage >= 0f);
        next.interactable = true;
    }
}

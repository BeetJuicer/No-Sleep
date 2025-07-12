using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject uiPanel;
    [SerializeField] private GameObject content;
    [SerializeField] private GameObject itemTemplatePrefab;
    private Inventory inventory;
    Dictionary<string, GameObject> itemCards = new();

    void Start()
    {
        inventory = FindFirstObjectByType<Inventory>();
        inventory.onItemObtained += Inventory_onItemObtained;
        inventory.onItemRemoved += Inventory_onItemRemoved;
        inventory.onItemUsed += Inventory_onItemUsed;
    }

    private void OnDestroy()
    {
        if (inventory != null)
        {
            inventory.onItemObtained -= Inventory_onItemObtained;
            inventory.onItemRemoved -= Inventory_onItemRemoved;
            inventory.onItemUsed -= Inventory_onItemUsed;
        }
    }

    private void Inventory_onItemObtained(string id, InventoryItem item)
    {
        var itemCard = Instantiate(itemTemplatePrefab, content.transform);

        // Set up the item card
        var itemImage = itemCard.GetComponent<Image>();
        if (itemImage != null)
        {
            itemImage.sprite = item.uiImage;
        }

        // Set up use button if item is usable
        var useButton = itemCard.GetComponentInChildren<Button>();
        if (useButton != null)
        {
            if (item.isUsable)
            {
                useButton.gameObject.SetActive(true);
                useButton.onClick.AddListener(() => inventory.UseItem(id));

                // Optional: Update button text
                var buttonText = useButton.GetComponentInChildren<TextMeshProUGUI>();
                if (buttonText != null)
                {
                    buttonText.text = "Use";
                }
            }
            else
            {
                useButton.gameObject.SetActive(false);
            }
        }

        itemCards.Add(id, itemCard);
    }

    private void Inventory_onItemRemoved(string id, InventoryItem item)
    {
        if (itemCards.TryGetValue(id, out GameObject itemCard))
        {
            itemCards.Remove(id);
            Destroy(itemCard);
        }
    }

    private void Inventory_onItemUsed(string id, InventoryItem item)
    {
        Debug.Log($"Item {id} was used!");
        // Add any UI feedback here (particles, sound, etc.)
    }

    public void OpenCloseUI()
    {
        uiPanel.SetActive(!uiPanel.activeSelf);
    }
}
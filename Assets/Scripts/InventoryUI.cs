using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject uiPanel;
    [SerializeField] private GameObject content;
    [SerializeField] private GameObject itemTemplatePrefab;
    [SerializeField] private Sprite lockedImage;
    private Inventory inventory;
    Dictionary<string, GameObject> itemCards = new();

    [SerializeField] private ItemDatabase effects;

    void Start()
    {
        inventory = FindFirstObjectByType<Inventory>();
        inventory.onItemObtained += Inventory_onItemObtained;
        inventory.onItemRemoved += Inventory_onItemRemoved;
        inventory.onItemUsed += Inventory_onItemUsed;

        foreach (var item in effects.items)
        {
            var itemCard = Instantiate(itemTemplatePrefab, content.transform);

            // Set up the item card
            var itemImage = itemCard.GetComponent<Image>();
            if (itemImage != null)
            {
                itemImage.sprite = lockedImage;
            }

            // Set up use button if item is usable
            itemCards.Add(item.Key, itemCard);
        }
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
        itemCards[id].GetComponent<Image>().sprite = item.uiImage;
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
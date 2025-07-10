using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject uiPanel;
    [SerializeField] private GameObject content;
    [SerializeField] private GameObject itemTemplatePrefab;

    private Inventory inventory;
    Dictionary<string, GameObject> itemCards = new();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inventory = FindFirstObjectByType<Inventory>();
        inventory.onItemObtained += Inventory_onItemObtained;
        inventory.onItemRemoved += Inventory_onItemRemoved;
    }

    private void OnDestroy()
    {
        inventory.onItemObtained -= Inventory_onItemObtained;
        inventory.onItemRemoved -= Inventory_onItemRemoved;
    }

    private void Inventory_onItemObtained(string id, InventoryItem item)
    {
        var itemCard = Instantiate(itemTemplatePrefab, content.transform);
        itemCard.GetComponent<Image>().sprite = item.uiImage;
        itemCards.Add(id, itemCard);
    }

    private void Inventory_onItemRemoved(string id, InventoryItem item)
    {
        var toDelete = itemCards[id];
        itemCards.Remove(id);
        Destroy(toDelete);
    }

    public void OpenCloseUI()
    {
        uiPanel.SetActive(!uiPanel.activeSelf);
    }
}

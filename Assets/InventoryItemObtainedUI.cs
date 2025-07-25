using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryItemObtainedUI : MonoBehaviour
{
    Inventory inventory;
    [SerializeField] TextMeshProUGUI itemNameText;
    [SerializeField] Image icon;
    WavePopupAnimation popup = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inventory = FindFirstObjectByType<Inventory>();
        inventory.onItemObtained += Inventory_onItemObtained;
        TryGetComponent<WavePopupAnimation>(out popup);
    }

    private void Inventory_onItemObtained(string key, InventoryItem item)
    {
        itemNameText.text = key;
        icon.sprite = item.uiImage;

        if (popup)
            popup.Show();
    }
}

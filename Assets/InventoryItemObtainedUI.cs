using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
public class InventoryItemObtainedUI : MonoBehaviour
{
    Inventory inventory;
    [SerializeField] TextMeshProUGUI itemNameText;
    [SerializeField] Image icon;
    [SerializeField] bool showIcon;
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

        if(showIcon)
            icon.sprite = item.uiImage;

        if (popup)
            popup.Show();
    }
}

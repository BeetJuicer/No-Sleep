using UnityEngine;
using AYellowpaper.SerializedCollections;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "ScriptableObjects/Item")]
public class ItemDatabase : ScriptableObject
{
    [SerializeField]
    [SerializedDictionary]
    public SerializedDictionary<string, InventoryItem> items = new SerializedDictionary<string, InventoryItem>();
}
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;
using System.Linq;
using System.Collections.Generic;
using System;

[System.Serializable]
public struct InventoryItem
{
    public Sprite uiImage;
    public bool owned;
    public bool isUsable; // New field to determine if item can be used
    public ItemEffect[] effects; // Array of effects this item has
}

[System.Serializable]
public abstract class ItemEffect : ScriptableObject
{
    public abstract void ApplyEffect();
    public abstract string GetEffectDescription();
}

// Updated Inventory class
public class Inventory : MonoBehaviour
{
    public event Action<string, InventoryItem> onItemObtained;
    public event Action<string, InventoryItem> onItemRemoved;
    public event Action<string, InventoryItem> onItemUsed; // New event for item usage

    [SerializeField] private ItemDatabase[] databases;

    [SerializeField]
    [SerializedDictionary]
    private SerializedDictionary<string, InventoryItem> inventory = new SerializedDictionary<string, InventoryItem>();

    private void Start()
    {
        LoadFromDatabases();

        foreach (var item in inventory)
        {
            print(item.Key);
        }
    }

    private void LoadFromDatabases()
    {
        foreach (var database in databases)
        {
            if (database != null && database.items != null)
            {
                print($"Loading {database.items.Count} items from database");

                foreach (var kvp in database.items)
                {
                    if (!inventory.ContainsKey(kvp.Key))
                    {
                        inventory.Add(kvp.Key, kvp.Value);
                    }
                    else
                    {
                        Debug.LogWarning($"Item {kvp.Key} already exists in inventory. Skipping.");
                    }
                }
            }
        }
    }

    public void SetItemAsOwned(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            Debug.LogError("Item ID cannot be null or empty");
            return;
        }

        if (inventory.TryGetValue(id, out InventoryItem item))
        {
            if (!item.owned)
                onItemObtained?.Invoke(id, item);

            item.owned = true;
            inventory[id] = item;
        }
        else
        {
            Debug.LogError($"{id} is not in inventory");
        }
    }

    public void SetItemAsNotOwned(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            Debug.LogError("Item ID cannot be null or empty");
            return;
        }

        if (inventory.TryGetValue(id, out InventoryItem item))
        {
            if (item.owned)
                onItemRemoved?.Invoke(id, item);

            item.owned = false;
            inventory[id] = item;
        }
        else
        {
            Debug.LogError($"{id} is not in inventory");
        }
    }

    // New method to use an item
    public void UseItem(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            Debug.LogError("Item ID cannot be null or empty");
            return;
        }

        if (inventory.TryGetValue(id, out InventoryItem item))
        {
            if (!item.owned)
            {
                Debug.LogWarning($"Item {id} is not owned and cannot be used");
                return;
            }

            if (!item.isUsable)
            {
                Debug.LogWarning($"Item {id} is not usable");
                return;
            }

            // Apply all effects
            if (item.effects != null)
            {
                foreach (var effect in item.effects)
                {
                    if (effect != null)
                    {
                        effect.ApplyEffect();
                    }
                }
            }

            onItemUsed?.Invoke(id, item);
            Debug.Log($"Used item: {id}");
        }
        else
        {
            Debug.LogError($"{id} is not in inventory");
        }
    }

    public bool IsItemOwned(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            Debug.LogWarning("Item ID cannot be null or empty");
            return false;
        }

        return inventory.TryGetValue(id, out InventoryItem item) && item.owned;
    }

    public bool IsItemUsable(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            Debug.LogWarning("Item ID cannot be null or empty");
            return false;
        }

        return inventory.TryGetValue(id, out InventoryItem item) && item.isUsable;
    }

    public Sprite GetItemUIImage(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            Debug.LogWarning("Item ID cannot be null or empty");
            return null;
        }

        return inventory.TryGetValue(id, out InventoryItem item) ? item.uiImage : null;
    }

    public bool HasItem(string id)
    {
        return !string.IsNullOrEmpty(id) && inventory.ContainsKey(id);
    }

    public string[] GetAllItemIds()
    {
        var ids = new string[inventory.Count];
        inventory.Keys.CopyTo(ids, 0);
        return ids;
    }

    public string[] GetOwnedItemIds()
    {
        var ownedIds = new List<string>();
        foreach (var kvp in inventory)
        {
            if (kvp.Value.owned)
            {
                ownedIds.Add(kvp.Key);
            }
        }
        return ownedIds.ToArray();
    }

    public int GetTotalItemCount()
    {
        return inventory.Count;
    }

    public int GetOwnedItemCount()
    {
        int count = 0;
        foreach (var item in inventory.Values)
        {
            if (item.owned) count++;
        }
        return count;
    }

    public void AddItem(string id, Sprite uiImage = null, bool owned = false, bool isUsable = false, ItemEffect[] effects = null)
    {
        if (string.IsNullOrEmpty(id))
        {
            Debug.LogError("Item ID cannot be null or empty");
            return;
        }

        if (inventory.ContainsKey(id))
        {
            Debug.LogWarning($"Item {id} already exists in inventory");
            return;
        }

        var newItem = new InventoryItem
        {
            uiImage = uiImage,
            owned = owned,
            isUsable = isUsable,
            effects = effects
        };

        inventory[id] = newItem;
    }

    public void RemoveItem(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            Debug.LogError("Item ID cannot be null or empty");
            return;
        }

        if (!inventory.Remove(id))
        {
            Debug.LogWarning($"Item {id} not found in inventory");
        }
    }
}
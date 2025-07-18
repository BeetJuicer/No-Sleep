using DialogueEditor;
using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.Events;
using AYellowpaper.SerializedCollections;

public class ConditionalConversation : MonoBehaviour
{
    [System.Serializable]
    enum Condition
    {
        None,
        AllItemsAcquired,
        IsSpecificItemAcquired,
    }

    public string forSpecificItem;
    [SerializeField] Condition condition;
    [SerializedDictionary]
    [SerializeField] SerializedDictionary<bool, NPCConversation> conversations = new();

    //refs
    private Inventory inventory;

    private void Start()
    {
        inventory = FindFirstObjectByType<Inventory>();
    }

    public void StartConversation()
    {
        foreach (var conversation in conversations)
        {
            bool passed = EvaluateCondition();
            conversations[passed].StartConversation();
        }
    }

    private bool EvaluateCondition()
    {
        switch(condition)
        {
            case Condition.AllItemsAcquired:
                return inventory.AreAllItemsAcquired();
            case Condition.IsSpecificItemAcquired:
                return inventory.IsItemOwned(forSpecificItem);
            case Condition.None:
                return true;
        }

        return true;
    }

}

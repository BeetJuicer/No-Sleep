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
        FiveStudentsTalked
    }

    public string forSpecificItem;
    [SerializeField] Condition condition;
    [SerializedDictionary]
    [SerializeField] SerializedDictionary<bool, NPCConversation> conversations = new();

    //refs
    private Inventory inventory;
    ClassroomData classroom;
    private void Start()
    {
        inventory = FindFirstObjectByType<Inventory>();
        classroom = FindFirstObjectByType<ClassroomData>();
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
            case Condition.FiveStudentsTalked:
                return classroom.studentsTalkedTo >= 5;
            case Condition.None:
                return true;
            default:
                Debug.LogError("unimplementedcase!");
                break;
        }

        return true;
    }

}

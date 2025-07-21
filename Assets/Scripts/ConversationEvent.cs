using DialogueEditor;
using UnityEngine;
using UnityEngine.Events;

public class ConversationEvent : MonoBehaviour
{
    public UnityEvent OnConversationEnd;
    NPCConversation npcConversation;

    private void Start()
    {
        npcConversation = GetComponent<NPCConversation>();
        npcConversation.OnNPCConversationEnd += ConversationEnd;
    }

    public void ConversationEnd()
    {
        OnConversationEnd?.Invoke();
    }
}

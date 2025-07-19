using UnityEngine;
using DialogueEditor;

public class Race : MonoBehaviour
{
    Inventory inv;
    [SerializeField] NPCConversation winConversation;
    [SerializeField] NPCConversation lossConversation;
    [SerializeField] NPCMovement npc;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inv = FindFirstObjectByType<Inventory>();
    }

    public void StartRace()
    {
        npc.StartMovement();
    }

    public void FinishRace(bool win)
    {
        if (win)
        {
            inv.SetItemAsOwned("Athlete");
            winConversation.StartConversation();
        }
        else
        {
            lossConversation.StartConversation();
        }
    }
}

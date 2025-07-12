using UnityEngine;
using DialogueEditor;

public class Door : MonoBehaviour
{
    [SerializeField] private bool requireKey;
    [SerializeField] private string keyName;
    
    [SerializeField] private Transform enterLocation;
    [SerializeField] private NPCConversation lockedConversation;
    
    Inventory inventory;
    TopDown.PlayerMovement player;

    LoopingRoom loopingRoom;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inventory = FindFirstObjectByType<Inventory>();
        player = FindFirstObjectByType<TopDown.PlayerMovement>();
        loopingRoom = FindFirstObjectByType<LoopingRoom>();
    }

   public void Interact()
   {
        if(requireKey && !inventory.IsItemOwned(keyName))
        {
            lockedConversation.StartConversation();
            return;
        }

        loopingRoom.ExitRoom();
        EnterDoor();
    }

    private void EnterDoor()
    {
        player.transform.position = enterLocation.position;
    }
}

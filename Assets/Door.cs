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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inventory = FindFirstObjectByType<Inventory>();
        player = FindFirstObjectByType<TopDown.PlayerMovement>();
    }

   public void Interact()
   {
        if(requireKey && !inventory.IsItemOwned(keyName))
        {
            lockedConversation.StartConversation();
            return;
        }

        EnterDoor();
    }

    private void EnterDoor()
    {
        player.transform.position = enterLocation.position;
    }
}

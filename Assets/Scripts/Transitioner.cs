using UnityEngine;
using TopDown;
using DialogueEditor;
public class Transitioner : MonoBehaviour
{
    private Transform playerTransform;
    [SerializeField] private Transform position;
    private NPCConversation npcConversation;

    private void Start()
    {
        playerTransform = FindFirstObjectByType<PlayerMovement>().transform;
        npcConversation = GetComponent<NPCConversation>();
    }

    public void Transfer()
    {
        Transition.Instance.TransitionWithAction(() => 
            playerTransform.position = position.position
        );
    }
}

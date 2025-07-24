using UnityEngine;
using DialogueEditor;

public class ClassroomData : MonoBehaviour
{
    public int studentsTalkedTo { get; private set; }
    [SerializeField] private int studentsToTalkBeforeExile = 5;
    [SerializeField] private NPCConversation npcConversation;

    public void Talk()
    {
        studentsTalkedTo++;
        if (studentsTalkedTo >= studentsToTalkBeforeExile)
            npcConversation.StartConversation();

    }
}

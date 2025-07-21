using UnityEngine;

public class ClassroomData : MonoBehaviour
{
    public int studentsTalkedTo { get; private set; }

    public void Talk()
    {
        studentsTalkedTo++;
    }
}

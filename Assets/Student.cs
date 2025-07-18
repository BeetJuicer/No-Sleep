using UnityEngine;

public class Student : MonoBehaviour
{
    ClassroomData classroom;
    bool talked;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        classroom = FindFirstObjectByType<ClassroomData>();
    }

    public void finishTalk()
    {
        print("atetmpting finish");
        if(!talked)
        {
        print(" finish");
            classroom.Talk();
            talked = true;
        }
    }
}

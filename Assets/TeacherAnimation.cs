using UnityEngine;

public class TeacherAnimation : MonoBehaviour
{
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Talk(bool angry)
    {
        animator.SetBool("talk", true);
        animator.SetBool("angry", angry);
    }

    public void EndTalk()
    {
        animator.SetBool("talk", false);
        animator.SetBool("angry", false);
    }
}

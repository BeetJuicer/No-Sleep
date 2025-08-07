using UnityEngine;
using UnityEngine.Android;

public class OCAnimation : MonoBehaviour
{
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Talk()
    {
        animator.SetBool("talk", true);
    }

    public void TalkDone()
    {
        animator.SetBool("talk", false);
    }
}

using UnityEngine;

public class MirrorAnimation : MonoBehaviour
{
    [SerializeField] private Animator original;
    [SerializeField] private Transform originalTransform;
    SpriteRenderer origRenderer;
    SpriteRenderer myRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        origRenderer = originalTransform.GetComponent<SpriteRenderer>();
        myRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (original.GetCurrentAnimatorStateInfo(0).IsName("PlayerIdleDown"))
        {
            GetComponent<Animator>().Play("PlayerIdleUp");
        }

        if (original.GetCurrentAnimatorStateInfo(0).IsName("PlayerIdleUp"))
        {
            GetComponent<Animator>().Play("PlayerIdleDown");
        }

        if (original.GetCurrentAnimatorStateInfo(0).IsName("PlayerWalkDown"))
        {
            GetComponent<Animator>().Play("PlayerWalkUp");
        }


        if (original.GetCurrentAnimatorStateInfo(0).IsName("PlayerWalkUp"))
        {
            GetComponent<Animator>().Play("PlayerWalkDown");
        }

        if (original.GetCurrentAnimatorStateInfo(0).IsName("PlayerWalkSide"))
        {
            GetComponent<Animator>().Play("PlayerWalkSide"); // set my animation clip as this
        }

        myRenderer.flipX = origRenderer.flipX;

    }

    private void LateUpdate()
    {
        transform.position = new Vector3(originalTransform.position.x, transform.position.y, transform.position.z);
    }
}

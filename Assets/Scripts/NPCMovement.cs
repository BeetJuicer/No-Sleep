using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    Animator animator;
    
    [SerializeField] float speed = 5f;
    
    [Tooltip("Only set 0 to 1")]
    [SerializeField]Vector3 direction;
    
    bool moving;
    bool raceFinished;
    private void Start()
    {
        animator = GetComponent<Animator>();
        raceFinished = false;
        animator.SetBool("raceFinished", raceFinished);
    }
    // Update is called once per frame
    void Update()
    {
        if(moving)
        {
            transform.position += direction * speed * Time.deltaTime; 
        }
    }

    [ContextMenu("move")]
    public void StartMovement()
    {
        moving = true;
        animator.SetBool("moving", moving);
    }

    [ContextMenu("stop")]
    public void StopMovement()
    {
        moving = false;
        animator.SetBool("moving", moving);
    }

    public void FinishRace(bool win)
    {
        animator.SetBool("win", win);
        raceFinished = true;
        animator.SetBool("raceFinished", raceFinished);
    }
}

using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    bool moving;
    [SerializeField] float speed = 5f;
    [Tooltip("Only set 0 to 1")]
    [SerializeField]Vector3 direction;

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
    }

    [ContextMenu("stop")]
    public void StopMovement()
    {
        moving = false;
    }
}

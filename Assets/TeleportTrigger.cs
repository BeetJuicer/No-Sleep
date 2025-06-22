using UnityEngine;

public class TeleportTrigger : MonoBehaviour
{
    [SerializeField] Transform teleportTo;
    [SerializeField] LayerMask whatIsTeleportable;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if((whatIsTeleportable & (1 << collision.gameObject.layer)) != 0)
        {
            collision.transform.position = teleportTo.position;
        }
    }
}

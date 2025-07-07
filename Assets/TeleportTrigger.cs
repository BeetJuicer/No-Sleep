using UnityEngine;

public class TeleportTrigger : MonoBehaviour
{
    private enum TPMode
    {
        X,
        Y,
        Both
    }

    [SerializeField] Transform teleportTo;
    [SerializeField] LayerMask whatIsTeleportable;
    [SerializeField] TPMode tpMode;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if((whatIsTeleportable & (1 << collision.gameObject.layer)) != 0)
        {
            switch(tpMode)
            {
                case TPMode.X:
                    collision.transform.position = new Vector3(teleportTo.position.x, collision.transform.position.y, collision.transform.position.z);
                    break;
                case TPMode.Y:
                    collision.transform.position = new Vector3(collision.transform.position.x, teleportTo.position.y, collision.transform.position.z);
                    break;
                case TPMode.Both:
                    collision.transform.position = teleportTo.position;
                    break;
            }

        }
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

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
    [SerializeField] float wait;
    private HashSet<Collider2D> objectsInTrigger = new HashSet<Collider2D>();

    public event Action onTeleport;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((whatIsTeleportable & (1 << collision.gameObject.layer)) != 0)
        {
            StartCoroutine(TeleportAfterWait(collision));
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        objectsInTrigger.Remove(collision);
    }

    private IEnumerator TeleportAfterWait(Collider2D collision)
    {
        objectsInTrigger.Add(collision);
        yield return new WaitForSeconds(wait);

        // Check if the collision object still exists and is still in the trigger
        if (collision != null && collision.gameObject != null && objectsInTrigger.Contains(collision))
        {
            switch (tpMode)
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

            onTeleport?.Invoke();
        }

        objectsInTrigger.Remove(collision);
    }
}
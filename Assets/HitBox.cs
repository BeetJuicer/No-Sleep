using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class HitBox : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private bool oneTimeHit;
    private int hitAmountsDone;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (oneTimeHit && hitAmountsDone > 0)
            return;

        if(collision.TryGetComponent(out Health health))
        {
            health.TakeDamage(damage);
            hitAmountsDone += 1;
        }
    }
}

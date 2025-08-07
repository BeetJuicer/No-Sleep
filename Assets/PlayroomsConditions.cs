using TopDown;
using UnityEngine;
using UnityEngine.Events;

public class PlayroomsConditions : MonoBehaviour
{
    Inventory inv;
    public string[] keys;
    int count;
    public UnityEvent onRequirementFulfilled;

    private void Start()
    {
        inv = FindFirstObjectByType<Inventory>();
        count = keys.Length;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        int ownedCount = 0;

        if(collision.TryGetComponent(out PlayerMovement mv))
        {
            //check inventory
            foreach (var item in keys)
            {
                if (inv.IsItemOwned(item))
                {
                    print("has item");
                    ownedCount++;
                }
            }

            if(ownedCount >= count)
            {
                onRequirementFulfilled?.Invoke();
            }
        }
    }
}

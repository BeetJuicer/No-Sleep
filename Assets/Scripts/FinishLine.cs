using UnityEngine;

//  To use this script, make an object with a collider and bound it to cover the player's entire body.
//  Make sure it's in a layer that can collide with the layer of this finishline's collider.
// The separate player racer object is to ensure that the edge of the body is what's detected by the finish line.
// Some player colliders don't cover all the way.
public class FinishLine : MonoBehaviour
{
    [SerializeField] private Race race;
    [SerializeField] string playerTag;
    bool finished;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out NPCMovement npc))
        {
            npc.StopMovement();
        }

        if (finished)
            return;

        bool win = collision.GetComponentInParent<TopDown.PlayerMovement>();
        print("Win: " + win);
       race.FinishRace(win);
       finished = true;
    }
}

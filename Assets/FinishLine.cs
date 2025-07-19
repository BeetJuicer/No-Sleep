using UnityEngine;

public class FinishLine : MonoBehaviour
{
    [SerializeField] private Race race;
    bool finished;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (finished)
            return;

       bool win = collision.CompareTag("Player");
       
       if(!win)
       {
            if(collision.TryGetComponent(out NPCMovement npc))
            {
                npc.StopMovement();
            }
       }
       
       race.FinishRace(win);
       finished = true;
    }
}

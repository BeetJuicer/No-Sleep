using UnityEngine;

public class LoopingRoomOpening : MonoBehaviour
{
    public enum OpeningMode
    {
        Entry, Exit
    }

    TeleportTrigger tp;
    LoopingRoom lp;
    [Tooltip("Entering the looping room or no?")]
    [SerializeField] OpeningMode mode;

    private void Start()
    {
        tp = GetComponent<TeleportTrigger>();
        lp = FindFirstObjectByType<LoopingRoom>();
        tp.onTeleport += Tp_onTeleport;
    }

    private void Tp_onTeleport()
    {
        if (mode == OpeningMode.Entry)
            lp.EnterRoom();
        else
            lp.ExitRoom();
    }
}

using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

public class Cutscene : MonoBehaviour
{
    [SerializeField] private bool freezeInput;
    PlayableDirector pDirector;

    private void Start()
    {
        pDirector = GetComponent<PlayableDirector>();
        pDirector.played += PDirector_played;
        pDirector.stopped += PDirector_stopped;
    }

    private void PDirector_stopped(PlayableDirector obj)
    {
        if (freezeInput)
            TopDown.GameManager.Instance.StopSequence();
    }

    private void PDirector_played(PlayableDirector obj)
    {
        if(freezeInput)
        {
            TopDown.GameManager.Instance.StartSequence();
        }
    }
}

using UnityEngine;

public class MusicRoom : MonoBehaviour
{
    [SerializeField] string bgName;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out TopDown.PlayerMovement playerMovement))
        {
            print("roomplaying " + bgName);
            AudioManager.Instance.PlayBGM(bgName);
            AudioManager.Instance.ResumeBGM();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out TopDown.PlayerMovement playerMovement))
        {
            print("stopping " + bgName);
            AudioManager.Instance.StopBGM();
        }
    }
}

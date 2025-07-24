using UnityEngine;
using DialogueEditor;
using System.Collections;
using TMPro;

public class Race : MonoBehaviour
{
    Inventory inv;
    [SerializeField] NPCConversation winConversation;
    [SerializeField] NPCConversation lossConversation;
    [SerializeField] NPCMovement npc;
    [SerializeField] TextMeshProUGUI countdownText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inv = FindFirstObjectByType<Inventory>();
    }

    public void StartRace()
    {
        StartCoroutine(RaceCountdown());
    }

    public void FinishRace(bool win)
    {
        if (win)
        {
            inv.SetItemAsOwned("Athletic");
            winConversation.StartConversation();
        }
        else
        {
            lossConversation.StartConversation();
        }
    }

    private IEnumerator RaceCountdown()
    {
        TopDown.GameManager.Instance.StartSequence();

        countdownText.text = "3";
        yield return new WaitForSeconds(1f);
        countdownText.text = "2";
        yield return new WaitForSeconds(1f);
        countdownText.text = "1";
        yield return new WaitForSeconds(1f);
        countdownText.text = "START!";
        yield return new WaitForSeconds(1f);
        countdownText.text = "";

        npc.StartMovement();
        TopDown.GameManager.Instance.StopSequence();

    }
}

using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class StockRoom : MonoBehaviour
{
    [SerializeField] private int foldersRequired;
    private int foldersAcquired;
    public UnityEvent OnAllFoldersAcquired;

    [SerializeField] TextMeshProUGUI count;
    public void GetFolder()
    {
        foldersAcquired++;
        if (foldersAcquired >= foldersRequired)
            OnAllFoldersAcquired?.Invoke();

        count.text = foldersAcquired + "/" + foldersRequired;
    }
}

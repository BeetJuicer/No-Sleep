using UnityEngine;

public class Transitioner : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform[] positions;

    public void Transfer(int i)
    {
        if (i >= positions.Length)
        {
            Debug.LogError(i + " not in array!");
            return;
        }

        playerTransform.position = positions[i].position;
    }
}

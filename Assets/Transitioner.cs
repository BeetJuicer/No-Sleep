using UnityEngine;
using TopDown;
public class Transitioner : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform[] positions;

    private void Start()
    {
        playerTransform = FindFirstObjectByType<PlayerMovement>().transform;
    }

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

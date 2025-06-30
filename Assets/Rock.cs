using System.Runtime.CompilerServices;
using UnityEngine;

public class Rock : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float distance = 1f;
    [SerializeField] private float duration;
    private float distanceMoved;
    private float startTime;

    private void Start()
    {
        startTime = Time.time;
    }

    void Update()
    {
        if(Time.time > startTime + duration)
            Destroy(gameObject);

        if (distanceMoved < distance)
        {
            float moveAmount = speed * Time.deltaTime;

            // Don't exceed the distance limit
            if (distanceMoved + moveAmount > distance)
            {
                moveAmount = distance - distanceMoved;
            }

            transform.Translate(Vector3.up * moveAmount);
            distanceMoved += moveAmount;
        }
    }
}

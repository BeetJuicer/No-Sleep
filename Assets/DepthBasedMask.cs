using UnityEngine;

public class DepthBasedMask : MonoBehaviour
{
    Transform player;
    SpriteMask mask;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindFirstObjectByType<TopDown.PlayerMovement>().transform;
        mask = GetComponent<SpriteMask>();
    }

    // Update is called once per frame
    void Update()
    {
        mask.enabled = player.position.y > transform.position.y;
    }
}

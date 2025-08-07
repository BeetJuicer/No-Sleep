using UnityEngine;

public class LoopingRoom : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sRenderer;
    [SerializeField] private Transform player;
    [SerializeField] private float buffer;

    float leftX, leftWithBuffer;
    float rightX, rightWithBuffer;
    float topY, topWithBuffer;
    float bottomY, bottomWithBuffer;

    bool needsPositionUpdate;
    Vector3 pendingPlayerPosition;

    public bool isLooping;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        float spriteWidth = sRenderer.sprite.bounds.size.x * transform.lossyScale.x;
        float spriteHeight = sRenderer.sprite.bounds.size.y * transform.lossyScale.y;

        leftX = transform.position.x - (spriteWidth / 2);
        rightX = transform.position.x + (spriteWidth / 2);
        topY =  transform.position.y + (spriteHeight / 2);
        bottomY = transform.position.y - (spriteHeight / 2);

        // we add or subtract buffers because we want to spawn riiight before the boundary. To prevent infinite loops.
        // keep the buffer as small as possible to prevent jitters.
        leftWithBuffer = leftX + buffer;
        rightWithBuffer = rightX - buffer;
        topWithBuffer = topY - buffer;
        bottomWithBuffer = bottomY + buffer;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLooping) return;

        CheckForLooping();
    }

    private void LateUpdate()
    {
        if (!isLooping) return;

        // check for looping update for accuracy, but apply it in late update to prevent flickers from camera.
        ApplyLooping();
    }

    void CheckForLooping()
    {
        Vector3 playerPos = player.position;
        bool positionChanged = false;
        
        //horizontal
        if (playerPos.x > rightX)
        {
            playerPos.x = leftX + (playerPos.x - rightX);
            positionChanged = true;
        }
        else if (playerPos.x < leftX)
        {
            playerPos.x = rightX - (leftX - playerPos.x);
            positionChanged = true;
        }

        //vertical
        if (playerPos.y > topY)
        {
            playerPos.y = bottomY + (playerPos.y - topY);
            positionChanged = true;
        }
        else if (playerPos.y < bottomY)
        {
            playerPos.y = topY - (bottomY - playerPos.y);
            positionChanged = true;
        }

        // Store the position change for LateUpdate
        if (positionChanged)
        {
            pendingPlayerPosition = playerPos;
            needsPositionUpdate = true;
        }
    }

    void ApplyLooping()
    {
        if (needsPositionUpdate)
        {
            player.position = pendingPlayerPosition;
            needsPositionUpdate = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out TopDown.PlayerMovement mv))
        {
            EnterRoom();
        }
    }

    public void EnterRoom()
    {
        isLooping = true;
    }

    public void ExitRoom()
    {
        isLooping = false;
    }
}

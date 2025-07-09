using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class InteractableTrigger : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private string playerLayerName = "Player";
    [SerializeField] private bool oneTimeUse = false;
    [SerializeField] private float interactionCooldown = 0.5f;

    [Header("UI Settings")]
    [SerializeField] private GameObject promptUI;
    [SerializeField] private string promptText = "Press E to interact";

    [Header("Events")]
    public UnityEvent OnInteract;
    public UnityEvent OnPlayerEnter;
    public UnityEvent OnPlayerExit;

    private HashSet<GameObject> playersInTrigger = new HashSet<GameObject>();
    private bool hasBeenUsed = false;
    private float lastInteractionTime = 0f;
    private int playerLayerMask;

    void Start()
    {
        // Get the layer mask for the player layer
        playerLayerMask = 1 << LayerMask.NameToLayer(playerLayerName);

        // Ensure we have a trigger collider
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        if (boxCollider == null)
        {
            Debug.LogError("InteractableTrigger requires a BoxCollider component!");
            return;
        }

        if (!boxCollider.isTrigger)
        {
            Debug.LogWarning("BoxCollider should be set as trigger for InteractableTrigger to work properly.");
            boxCollider.isTrigger = true;
        }

        // Hide prompt UI initially
        if (promptUI != null)
        {
            promptUI.SetActive(false);
        }
    }

    void Update()
    {
        // Check for interaction input when player is in trigger
        if (CanInteract() && Input.GetKeyDown(interactKey))
        {
            Interact();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object is on the player layer
        if (IsPlayerLayer(collision.gameObject))
        {
            playersInTrigger.Add(collision.gameObject);

            // Show prompt UI
            if (promptUI != null && CanShowPrompt())
            {
                promptUI.SetActive(true);
            }

            // Invoke enter event
            OnPlayerEnter?.Invoke();

            Debug.Log($"Player {collision.gameObject.name} entered interaction zone");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Check if the object is on the player layer
        if (IsPlayerLayer(collision.gameObject))
        {
            playersInTrigger.Remove(collision.gameObject);

            // Hide prompt UI if no players are in trigger
            if (playersInTrigger.Count == 0 && promptUI != null)
            {
                promptUI.SetActive(false);
            }

            // Invoke exit event
            OnPlayerExit?.Invoke();

            Debug.Log($"Player {collision.gameObject.name} left interaction zone");
        }
    }

    private bool IsPlayerLayer(GameObject obj)
    {
        return (playerLayerMask & (1 << obj.layer)) != 0;
    }

    private bool CanInteract()
    {
        return playersInTrigger.Count > 0 &&
               !hasBeenUsed &&
               Time.time - lastInteractionTime >= interactionCooldown;
    }

    private bool CanShowPrompt()
    {
        return !hasBeenUsed;
    }

    public void Interact()
    {
        if (!CanInteract()) return;

        // Record interaction time
        lastInteractionTime = Time.time;

        // Mark as used if one-time use
        if (oneTimeUse)
        {
            hasBeenUsed = true;
            if (promptUI != null)
            {
                promptUI.SetActive(false);
            }
        }

        // Invoke the interaction event
        OnInteract?.Invoke();

        Debug.Log($"Interacted with {gameObject.name}");
    }

    // Public methods for external control
    public void ResetInteractable()
    {
        hasBeenUsed = false;
        lastInteractionTime = 0f;

        if (promptUI != null && playersInTrigger.Count > 0)
        {
            promptUI.SetActive(true);
        }
    }

    public void SetInteractKey(KeyCode newKey)
    {
        interactKey = newKey;
    }

    public void SetPromptText(string newText)
    {
        promptText = newText;
    }

    public bool IsPlayerInRange()
    {
        return playersInTrigger.Count > 0;
    }

    public bool HasBeenUsed()
    {
        return hasBeenUsed;
    }

    // Force interaction (useful for debugging or external triggers)
    public void ForceInteract()
    {
        if (hasBeenUsed && oneTimeUse) return;

        lastInteractionTime = Time.time;

        if (oneTimeUse)
        {
            hasBeenUsed = true;
            if (promptUI != null)
            {
                promptUI.SetActive(false);
            }
        }

        OnInteract?.Invoke();
        Debug.Log($"Force interacted with {gameObject.name}");
    }
}
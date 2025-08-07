using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueController : MonoBehaviour
{
    [SerializeField] private string[] dialogueStrings;
    [SerializeField] private TextMeshProUGUI dialogueText; // Assign your TextMeshPro UI component
    [SerializeField] private float typeSpeed = 0.05f; // Time between each letter
    [SerializeField] private float displayDuration = 2f; // How long text stays before fading after typing is complete
    [SerializeField] private float fadeDuration = 1f; // How long the fade effect takes
    [SerializeField] private float fadeDistance = 100f; // How far left the text moves while fading

    private bool isDialogueActive = false;

    [NaughtyAttributes.Button]
    public void StartDialogue()
    {
        TopDown.GameManager.Instance.StartSequence();
        if (isDialogueActive || dialogueStrings.Length == 0)
            return;

        StartCoroutine(DialogueSequence());
    }

    private IEnumerator DialogueSequence()
    {
        isDialogueActive = true;

        // Make sure the text component is active
        if (dialogueText != null)
            dialogueText.gameObject.SetActive(true);

        for (int i = 0; i < dialogueStrings.Length; i++)
        {
            yield return StartCoroutine(DisplayText(dialogueStrings[i]));
        }

        // Hide text after all dialogue is complete
        if (dialogueText != null)
            dialogueText.gameObject.SetActive(false);

        isDialogueActive = false;
        TopDown.GameManager.Instance.StopSequence();
    }

    private IEnumerator DisplayText(string text)
    {
        if (dialogueText == null)
            yield break;

        // Reset position and alpha
        RectTransform rectTransform = dialogueText.GetComponent<RectTransform>();
        Vector3 originalPosition = rectTransform.anchoredPosition;
        Color originalColor = dialogueText.color;

        // Make sure text is fully visible and reset position
        dialogueText.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);
        rectTransform.anchoredPosition = originalPosition;

        // Clear text and start typing effect
        dialogueText.text = "";

        // Type out each character
        for (int i = 0; i <= text.Length; i++)
        {
            dialogueText.text = text.Substring(0, i);
            yield return new WaitForSeconds(typeSpeed);
        }

        // Wait for the display duration after typing is complete
        yield return new WaitForSeconds(displayDuration);

        // Fade out while moving left
        float elapsedTime = 0f;
        Vector3 startPosition = rectTransform.anchoredPosition;
        Vector3 endPosition = startPosition + Vector3.left * fadeDistance;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeDuration;

            // Smooth fade curve
            float fadeAlpha = Mathf.Lerp(1f, 0f, EaseOutCubic(t));
            float positionT = EaseOutCubic(t);

            // Apply fade and movement
            dialogueText.color = new Color(originalColor.r, originalColor.g, originalColor.b, fadeAlpha);
            rectTransform.anchoredPosition = Vector3.Lerp(startPosition, endPosition, positionT);

            yield return null;
        }

        // Ensure final state
        dialogueText.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        rectTransform.anchoredPosition = endPosition;

        // Reset position for next text
        rectTransform.anchoredPosition = originalPosition;
    }

    // Smooth easing function for more natural animation
    private float EaseOutCubic(float t)
    {
        return 1f - Mathf.Pow(1f - t, 3f);
    }

    // Optional: Method to stop dialogue early
    public void StopDialogue()
    {
        if (isDialogueActive)
        {
            StopAllCoroutines();
            isDialogueActive = false;

            if (dialogueText != null)
            {
                dialogueText.gameObject.SetActive(false);
            }
        }
    }

    // Optional: Check if dialogue is currently running
    public bool IsDialogueActive()
    {
        return isDialogueActive;
    }

    // Optional: Skip current typing animation and show full text
    public void SkipTyping()
    {
        // This would need additional state tracking to implement properly
        // Left as an exercise for more advanced usage
    }
}
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Transition : MonoBehaviour
{
    public static Transition Instance { get; private set; }
    Image image;
    [SerializeField] private float inDurationDefault = 1f;
    [SerializeField] private float outDurationDefault = 1f;
    [SerializeField] private float holdTimeDefault;
    private float currentInDuration = 0.5f;
    private float currentOutDuration;
    private bool isTransitioning;
    private bool isFadingIn;
    private float transitionTimer;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject); // Destroy the gameObject, not just the component
    }

    private void Start()
    {
        image = GetComponent<Image>();
        // Start with transparent
        Color color = image.color;
        color.a = 0f;
        image.color = color;
    }

    private void Update()
    {
        if (isTransitioning)
        {
            transitionTimer += Time.deltaTime;

            if (isFadingIn)
            {
                // Fade in (transparent to opaque)
                float progress = transitionTimer / currentInDuration;
                float alpha = Mathf.Lerp(0f, 1f, progress);

                Color color = image.color;
                color.a = alpha;
                image.color = color;

                // Check if fade in is complete
                if (progress >= 1f)
                {
                    isFadingIn = false;
                    transitionTimer = 0f;
                    // Start fade out immediately, or you could add a delay here
                }
            }
            else
            {
                // Fade out (opaque to transparent)
                float progress = transitionTimer / currentOutDuration;
                float alpha = Mathf.Lerp(1f, 0f, progress);

                Color color = image.color;
                color.a = alpha;
                image.color = color;

                // Check if fade out is complete
                if (progress >= 1f)
                {
                    isTransitioning = false;
                    transitionTimer = 0f;
                }
            }
        }
    }

    public void StartTransition()
    {
        StartTransition(inDurationDefault, outDurationDefault);
    }

    public void StartTransition(float inDuration, float outDuration)
    {
        if (!isTransitioning) // Prevent overlapping transitions
        {
            isTransitioning = true;
            isFadingIn = true;
            currentInDuration = inDuration;
            currentOutDuration = outDuration;
            transitionTimer = 0f;

            // Ensure we start from transparent
            Color color = image.color;
            color.a = 0f;
            image.color = color;
        }
    }

    public void TransitionWithAction(System.Action action)
    {
        StartCoroutine(TransitionWithActionCoroutine(action, inDurationDefault, outDurationDefault, holdTimeDefault));
    }

    public void TransitionWithAction(System.Action action, float inD, float outD, float holdT)
    {
        StartCoroutine(TransitionWithActionCoroutine(action, inD, outD, holdT));
    }

    private IEnumerator TransitionWithActionCoroutine(System.Action action, float inDuration, float outDuration, float holdTime)
    {
        TopDown.GameManager.Instance.StartSequence();

        isTransitioning = true;

        // Fade In
        Color color = image.color;
        color.a = 0f;
        image.color = color;

        float timer = 0f;
        while (timer < inDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / inDuration;
            color.a = Mathf.Lerp(0f, 1f, progress);
            image.color = color;
            yield return null;
        }

        // Ensure fully opaque
        color.a = 1f;
        image.color = color;

        // Execute the action
        action?.Invoke();

        // Hold time
        if (holdTime > 0f)
        {
            yield return new WaitForSeconds(holdTime);
        }

        // Fade Out
        timer = 0f;
        while (timer < outDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / outDuration;
            color.a = Mathf.Lerp(1f, 0f, progress);
            image.color = color;
            yield return null;
        }

        // Ensure fully transparent
        color.a = 0f;
        image.color = color;

        isTransitioning = false;
        TopDown.GameManager.Instance.StopSequence();
    }
}

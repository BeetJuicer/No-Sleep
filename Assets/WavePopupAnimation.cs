using UnityEngine;
using System.Collections;

public class WavePopupAnimation : MonoBehaviour
{
    [Header("Animation Settings")]
    public float animationDuration = 1f;
    public AnimationCurve easeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Scale Settings")]
    public Vector3 targetScale = Vector3.one;

    [Header("Axis Targeting")]
    public bool animateX = true;
    public bool animateY = true;
    public bool animateZ = true;

    [Header("Show Settings")]
    public float showDuration = 2f;
    public bool showOnStart;

    private Vector3 originalScale;
    private Vector3 startScale;

    void Start()
    {
        originalScale = transform.localScale;

        if (showOnStart)
            StartCoroutine(PopupCoroutine());
    }

    /// <summary>
    /// Complete show sequence: popup, wait, popdown
    /// </summary>
    [NaughtyAttributes.Button]
    public void Show()
    {
        print("showing..");
        StartCoroutine(ShowSequence());
    }

    private IEnumerator ShowSequence()
    {
        yield return StartCoroutine(PopupCoroutine());
        yield return new WaitForSeconds(showDuration);
        yield return StartCoroutine(PopdownCoroutine());
    }

    private IEnumerator PopupCoroutine()
    {
        startScale = transform.localScale;
        float elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            float normalizedTime = elapsedTime / animationDuration;
            float progress = easeCurve.Evaluate(normalizedTime);

            Vector3 currentScale = Vector3.Lerp(startScale, targetScale, progress);

            // Apply axis targeting
            Vector3 finalScale = transform.localScale;
            if (animateX) finalScale.x = currentScale.x;
            if (animateY) finalScale.y = currentScale.y;
            if (animateZ) finalScale.z = currentScale.z;

            transform.localScale = finalScale;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure final scale is exactly the target on animated axes
        Vector3 endScale = transform.localScale;
        if (animateX) endScale.x = targetScale.x;
        if (animateY) endScale.y = targetScale.y;
        if (animateZ) endScale.z = targetScale.z;
        transform.localScale = endScale;
    }

    private IEnumerator PopdownCoroutine()
    {
        Vector3 currentScale = transform.localScale;
        float elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            float normalizedTime = elapsedTime / animationDuration;
            float progress = easeCurve.Evaluate(normalizedTime);

            Vector3 targetZero = Vector3.zero;
            Vector3 lerpedScale = Vector3.Lerp(currentScale, targetZero, progress);

            // Apply axis targeting
            Vector3 finalScale = transform.localScale;
            if (animateX) finalScale.x = lerpedScale.x;
            if (animateY) finalScale.y = lerpedScale.y;
            if (animateZ) finalScale.z = lerpedScale.z;

            transform.localScale = finalScale;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure final scale is zero on animated axes
        Vector3 endScale = transform.localScale;
        if (animateX) endScale.x = 0f;
        if (animateY) endScale.y = 0f;
        if (animateZ) endScale.z = 0f;
        transform.localScale = endScale;
    }
}
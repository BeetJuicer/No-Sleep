using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class FadeInUIImage : MonoBehaviour
{
    [Header("Fade Settings")]
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float startAlpha = 0f;
    [SerializeField] private float targetAlpha = 1f;
    [SerializeField] private AnimationCurve fadeCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    [Header("Options")]
    [SerializeField] private bool fadeOnEnable = true;
    [SerializeField] private bool resetAlphaOnDisable = true;
    [SerializeField] private float delay = 0f;

    private Image image;
    private Color originalColor;
    private Coroutine fadeCoroutine;

    void Awake()
    {
        image = GetComponent<Image>();
        originalColor = image.color;
    }

    void OnEnable()
    {
        if (fadeOnEnable)
        {
            FadeIn();
        }
    }

    void OnDisable()
    {
        if (resetAlphaOnDisable)
        {
            ResetAlpha();
        }

        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
            fadeCoroutine = null;
        }
    }

    /// <summary>
    /// Starts the fade in effect
    /// </summary>
    public void FadeIn()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        fadeCoroutine = StartCoroutine(FadeInCoroutine());
    }

    /// <summary>
    /// Starts fade in with custom duration
    /// </summary>
    public void FadeIn(float customDuration)
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        fadeCoroutine = StartCoroutine(FadeInCoroutine(customDuration));
    }

    /// <summary>
    /// Starts fade in with custom parameters
    /// </summary>
    public void FadeIn(float customDuration, float customStartAlpha, float customTargetAlpha)
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        fadeCoroutine = StartCoroutine(FadeInCoroutine(customDuration, customStartAlpha, customTargetAlpha));
    }

    /// <summary>
    /// Immediately sets alpha to start value
    /// </summary>
    public void ResetAlpha()
    {
        SetAlpha(startAlpha);
    }

    /// <summary>
    /// Immediately sets alpha to target value
    /// </summary>
    public void SetToTargetAlpha()
    {
        SetAlpha(targetAlpha);
    }

    /// <summary>
    /// Sets the alpha value immediately
    /// </summary>
    public void SetAlpha(float alpha)
    {
        Color color = image.color;
        color.a = Mathf.Clamp01(alpha);
        image.color = color;
    }

    /// <summary>
    /// Stops the current fade if running
    /// </summary>
    public void StopFade()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
            fadeCoroutine = null;
        }
    }

    /// <summary>
    /// Gets the current alpha value
    /// </summary>
    public float GetCurrentAlpha()
    {
        return image.color.a;
    }

    /// <summary>
    /// Checks if currently fading
    /// </summary>
    public bool IsFading()
    {
        return fadeCoroutine != null;
    }

    private IEnumerator FadeInCoroutine()
    {
        yield return FadeInCoroutine(fadeDuration, startAlpha, targetAlpha);
    }

    private IEnumerator FadeInCoroutine(float duration)
    {
        yield return FadeInCoroutine(duration, startAlpha, targetAlpha);
    }

    private IEnumerator FadeInCoroutine(float duration, float fromAlpha, float toAlpha)
    {
        // Set initial alpha
        SetAlpha(fromAlpha);

        // Wait for delay if specified
        if (delay > 0f)
        {
            yield return new WaitForSeconds(delay);
        }

        float elapsed = 0f;
        Color color = image.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / duration;

            // Apply animation curve
            float curveValue = fadeCurve.Evaluate(progress);

            // Interpolate alpha
            float currentAlpha = Mathf.Lerp(fromAlpha, toAlpha, curveValue);

            // Apply to image
            color.a = currentAlpha;
            image.color = color;

            yield return null;
        }

        // Ensure final alpha is set
        color.a = toAlpha;
        image.color = color;

        fadeCoroutine = null;
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        if (fadeDuration < 0f) fadeDuration = 0f;
        if (delay < 0f) delay = 0f;

        startAlpha = Mathf.Clamp01(startAlpha);
        targetAlpha = Mathf.Clamp01(targetAlpha);
    }

    // Preview in editor
    [ContextMenu("Preview Fade In")]
    void PreviewFadeIn()
    {
        if (Application.isPlaying)
        {
            FadeIn();
        }
    }

    [ContextMenu("Reset to Start Alpha")]
    void PreviewResetAlpha()
    {
        if (image == null) image = GetComponent<Image>();
        SetAlpha(startAlpha);
    }

    [ContextMenu("Set to Target Alpha")]
    void PreviewTargetAlpha()
    {
        if (image == null) image = GetComponent<Image>();
        SetAlpha(targetAlpha);
    }
#endif
}
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal; // For URP
// using UnityEngine.Rendering.HighDefinition; // For HDRP

public class VignetteController : MonoBehaviour
{
    [Header("Vignette Settings")]
    [SerializeField] private Volume postProcessVolume;
    [SerializeField] private bool animateVignette = false;

    [Header("Animation Settings")]
    [SerializeField] private float animationSpeed = 1f;
    [SerializeField] private float minIntensity = 0f;
    [SerializeField] private float maxIntensity = 1f;

    private Vignette vignette;
    private float animationTime = 0f;

    void Start()
    {
        // Get the volume component
        if (postProcessVolume == null)
        {
            postProcessVolume = GetComponent<Volume>();
        }

        if (postProcessVolume == null)
        {
            Debug.LogError("No Volume component found! Please assign a Volume or add one to this GameObject.");
            return;
        }

        // Get the vignette effect from the volume profile
        if (postProcessVolume.profile.TryGet<Vignette>(out vignette))
        {
            Debug.Log("Vignette effect found and ready for runtime control.");
        }
        else
        {
            Debug.LogError("Vignette effect not found in the Volume profile!");
        }
    }

    void Update()
    {
        if (animateVignette && vignette != null)
        {
            AnimateVignette();
        }
    }

    private void AnimateVignette()
    {
        animationTime += Time.deltaTime * animationSpeed;
        float intensity = Mathf.Lerp(minIntensity, maxIntensity, (Mathf.Sin(animationTime) + 1f) / 2f);
        SetVignetteIntensity(intensity);
    }

    // Public methods for controlling vignette properties
    public void SetVignetteIntensity(float intensity)
    {
        if (vignette != null)
        {
            vignette.intensity.value = Mathf.Clamp01(intensity);
        }
    }

    public void SetVignetteSmoothness(float smoothness)
    {
        if (vignette != null)
        {
            vignette.smoothness.value = Mathf.Clamp01(smoothness);
        }
    }


    public void SetVignetteRounded(bool rounded)
    {
        if (vignette != null)
        {
            vignette.rounded.value = rounded;
        }
    }

    public void SetVignetteColor(Color color)
    {
        if (vignette != null)
        {
            vignette.color.value = color;
        }
    }

    public void SetVignetteCenter(Vector2 center)
    {
        if (vignette != null)
        {
            vignette.center.value = center;
        }
    }

    public void SetMinIntensity(float intensity)
    {
        minIntensity = intensity;
    }

    public void SetMaxIntensity(float intensity)
    {
        maxIntensity = intensity;
    }

    // Utility methods
    public void EnableVignette()
    {
        if (vignette != null)
        {
            vignette.active = true;
        }
    }

    public void DisableVignette()
    {
        if (vignette != null)
        {
            vignette.active = false;
        }
    }

    public void ToggleVignette()
    {
        if (vignette != null)
        {
            vignette.active = !vignette.active;
        }
    }

    // Smooth transition methods
    public void FadeVignetteIn(float duration)
    {
        if (vignette != null)
        {
            StartCoroutine(FadeVignette(0f, 1f, duration));
        }
    }

    public void FadeVignetteOut(float duration)
    {
        if (vignette != null)
        {
            StartCoroutine(FadeVignette(vignette.intensity.value, 0f, duration));
        }
    }

    public void FadeVignetteTo(float targetIntensity, float duration)
    {
        if (vignette != null)
        {
            StartCoroutine(FadeVignette(vignette.intensity.value, targetIntensity, duration));
        }
    }

    private System.Collections.IEnumerator FadeVignette(float startIntensity, float endIntensity, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / duration;
            float currentIntensity = Mathf.Lerp(startIntensity, endIntensity, progress);
            SetVignetteIntensity(currentIntensity);
            yield return null;
        }

        SetVignetteIntensity(endIntensity);
    }

    // Preset configurations
    public void ApplyHealthDamageVignette()
    {
        if (vignette != null)
        {
            SetVignetteColor(Color.red);
            SetVignetteIntensity(0.5f);
            SetVignetteSmoothness(0.3f);
        }
    }

    public void ApplyFocusVignette()
    {
        if (vignette != null)
        {
            SetVignetteColor(Color.black);
            SetVignetteIntensity(0.3f);
            SetVignetteSmoothness(0.5f);
        }
    }

    public void ApplyDreamVignette()
    {
        if (vignette != null)
        {
            SetVignetteColor(new Color(0.8f, 0.9f, 1f, 1f)); // Light blue
            SetVignetteIntensity(0.4f);
            SetVignetteSmoothness(0.8f);
        }
    }

    public void ResetVignette()
    {
        if (vignette != null)
        {
            SetVignetteIntensity(0f);
            SetVignetteSmoothness(0.2f);
            SetVignetteColor(Color.black);
            SetVignetteCenter(Vector2.one * 0.5f);
            SetVignetteRounded(false);
        }
    }

    // Get current values
    public float GetVignetteIntensity()
    {
        return vignette?.intensity.value ?? 0f;
    }

    public float GetVignetteSmoothness()
    {
        return vignette?.smoothness.value ?? 0f;
    }


    public Color GetVignetteColor()
    {
        return vignette?.color.value ?? Color.black;
    }
}
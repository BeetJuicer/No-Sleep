using UnityEngine;

public class SpritePaintMonitor : MonoBehaviour
{
    [Header("Sprite Settings")]
    public SpriteRenderer spriteRenderer;
    public Texture2D spriteTexture;

    [Header("Monitoring Settings")]
    [Range(0f, 1f)]
    public float completionThreshold = 0.95f; // 95% painted = "fully painted"
    public bool checkOnStart = true;
    public bool continuousCheck = false;
    public float checkInterval = 1f; // seconds between checks if continuous

    [Header("Debug Info")]
    public int initialTransparentCount = 0;
    public int currentTransparentCount = 0;
    public float paintProgress = 0f;

    private Texture2D workingTexture;
    private Color[] initialPixels;
    private bool hasBeenFullyPainted = false;
    private float lastCheckTime = 0f;

    Inventory inventory;

    void Start()
    {
        InitializeSprite();
        inventory = FindFirstObjectByType<Inventory>();


        if (checkOnStart)
        {
            CheckPaintProgress();
        }
    }

    void Update()
    {
        if (continuousCheck && Time.time - lastCheckTime >= checkInterval)
        {
            CheckPaintProgress();
            lastCheckTime = Time.time;
        }
    }

    void InitializeSprite()
    {
        // Get the sprite texture
        if (spriteRenderer != null && spriteRenderer.sprite != null)
        {
            spriteTexture = spriteRenderer.sprite.texture;
        }

        if (spriteTexture == null)
        {
            Debug.LogError("No sprite texture found!");
            return;
        }

        // Create a working copy of the texture
        workingTexture = new Texture2D(spriteTexture.width, spriteTexture.height, TextureFormat.RGBA32, false);

        // Make sure the texture is readable
        if (!spriteTexture.isReadable)
        {
            Debug.LogWarning("Sprite texture is not readable! You may need to change import settings.");
        }

        // Copy pixels and count initial transparent pixels
        try
        {
            initialPixels = spriteTexture.GetPixels();
            workingTexture.SetPixels(initialPixels);
            workingTexture.Apply();

            CountInitialTransparentPixels();
        }
        catch (UnityException e)
        {
            Debug.LogError($"Could not read texture pixels: {e.Message}");
        }
    }

    void CountInitialTransparentPixels()
    {
        initialTransparentCount = 0;

        foreach (Color pixel in initialPixels)
        {
            if (pixel.a < 0.01f) // Consider nearly transparent as transparent
            {
                initialTransparentCount++;
            }
        }

        currentTransparentCount = initialTransparentCount;

        Debug.Log($"Initial transparent pixels: {initialTransparentCount} out of {initialPixels.Length} total pixels");
    }

    public void CheckPaintProgress()
    {
        if (spriteTexture == null || hasBeenFullyPainted)
            return;

        Color[] currentPixels = spriteTexture.GetPixels();
        int transparentCount = 0;

        // Count current transparent pixels
        foreach (Color pixel in currentPixels)
        {
            if (pixel.a < 0.01f)
            {
                transparentCount++;
            }
        }

        currentTransparentCount = transparentCount;

        // Calculate progress
        if (initialTransparentCount > 0)
        {
            paintProgress = 1f - ((float)currentTransparentCount / initialTransparentCount);
        }
        else
        {
            paintProgress = 1f; // No transparent pixels to begin with
        }

        // Check if fully painted
        if (paintProgress >= completionThreshold && !hasBeenFullyPainted)
        {
            hasBeenFullyPainted = true;
            OnFullyPainted();
        }

        // Debug info
        Debug.Log($"Paint Progress: {paintProgress:P1} ({initialTransparentCount - currentTransparentCount}/{initialTransparentCount} pixels painted)");
    }

    void OnFullyPainted()
    {
        Debug.Log("FULLY PAINTED!");

        // You can add additional effects here:
        // - Play a sound
        // - Show particles
        // - Trigger an animation
        // - Call other game events

        GrantArtistEffect();

        // Example: Change sprite color to indicate completion
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.green;
        }
    }

    private void GrantArtistEffect()
    {
        inventory.SetItemAsOwned("Artist");
    }

    // Public methods for external triggering
    public void ForceCheck()
    {
        CheckPaintProgress();
    }

    public void ResetMonitoring()
    {
        hasBeenFullyPainted = false;
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.white;
        }
        InitializeSprite();
    }

    // Method to simulate painting (for testing)
    [ContextMenu("Simulate Paint Progress")]
    public void SimulatePaintProgress()
    {
        if (workingTexture == null) return;

        Color[] pixels = workingTexture.GetPixels();
        int pixelsToPaint = Mathf.RoundToInt(pixels.Length * 0.1f); // Paint 10% of pixels

        for (int i = 0; i < pixelsToPaint && i < pixels.Length; i++)
        {
            if (pixels[i].a < 0.01f) // If transparent
            {
                pixels[i] = Color.red; // Paint it red
            }
        }

        workingTexture.SetPixels(pixels);
        workingTexture.Apply();

        // Update the sprite renderer if it's using this texture
        if (spriteRenderer != null && spriteRenderer.sprite != null)
        {
            Sprite newSprite = Sprite.Create(workingTexture, spriteRenderer.sprite.rect, spriteRenderer.sprite.pivot);
            spriteRenderer.sprite = newSprite;
        }

        CheckPaintProgress();
    }

    void OnDestroy()
    {
        if (workingTexture != null)
        {
            DestroyImmediate(workingTexture);
        }
    }
}
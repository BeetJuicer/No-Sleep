using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialSwitcher : MonoBehaviour
{
    [Header("Materials")]
    [SerializeField] private List<Material> materials = new List<Material>();
    
    [Header("Duration Settings")]
    [SerializeField] private float minDuration = 1f;
    [SerializeField] private float maxDuration = 3f;
    
    [Header("Settings")]
    [SerializeField] private bool startOnAwake = true;
    [SerializeField] private bool allowSameMaterial = false;
    
    private Renderer objectRenderer;
    private Coroutine switchingCoroutine;
    private int lastMaterialIndex = -1;
    
    void Awake()
    {
        objectRenderer = GetComponent<Renderer>();
        
        if (objectRenderer == null)
        {
            Debug.LogError("MaterialSwitcher: No Renderer component found on " + gameObject.name);
            enabled = false;
            return;
        }
        
        if (materials.Count == 0)
        {
            Debug.LogWarning("MaterialSwitcher: No materials assigned to " + gameObject.name);
            enabled = false;
            return;
        }
    }
    
    void Start()
    {
        if (startOnAwake)
        {
            StartSwitching();
        }
    }
    
    /// <summary>
    /// Starts the material switching coroutine
    /// </summary>
    public void StartSwitching()
    {
        if (switchingCoroutine != null)
        {
            StopCoroutine(switchingCoroutine);
        }
        
        switchingCoroutine = StartCoroutine(SwitchMaterialsCoroutine());
    }
    
    /// <summary>
    /// Stops the material switching
    /// </summary>
    public void StopSwitching()
    {
        if (switchingCoroutine != null)
        {
            StopCoroutine(switchingCoroutine);
            switchingCoroutine = null;
        }
    }
    
    /// <summary>
    /// Switches to a random material immediately
    /// </summary>
    public void SwitchToRandomMaterial()
    {
        if (materials.Count == 0) return;
        
        int randomIndex = GetRandomMaterialIndex();
        objectRenderer.material = materials[randomIndex];
        lastMaterialIndex = randomIndex;
    }
    
    /// <summary>
    /// Switches to a specific material by index
    /// </summary>
    public void SwitchToMaterial(int index)
    {
        if (index >= 0 && index < materials.Count)
        {
            objectRenderer.material = materials[index];
            lastMaterialIndex = index;
        }
        else
        {
            Debug.LogWarning("MaterialSwitcher: Invalid material index " + index);
        }
    }
    
    /// <summary>
    /// Adds a new material to the list
    /// </summary>
    public void AddMaterial(Material material)
    {
        if (material != null && !materials.Contains(material))
        {
            materials.Add(material);
        }
    }
    
    /// <summary>
    /// Removes a material from the list
    /// </summary>
    public void RemoveMaterial(Material material)
    {
        materials.Remove(material);
    }
    
    private IEnumerator SwitchMaterialsCoroutine()
    {
        while (true)
        {
            // Wait for random duration
            float waitTime = Random.Range(minDuration, maxDuration);
            yield return new WaitForSeconds(waitTime);
            
            // Switch to random material
            SwitchToRandomMaterial();
        }
    }
    
    private int GetRandomMaterialIndex()
    {
        if (materials.Count == 1)
        {
            return 0;
        }
        
        int randomIndex;
        
        if (allowSameMaterial)
        {
            randomIndex = Random.Range(0, materials.Count);
        }
        else
        {
            // Ensure we don't pick the same material twice in a row
            do
            {
                randomIndex = Random.Range(0, materials.Count);
            }
            while (randomIndex == lastMaterialIndex && materials.Count > 1);
        }
        
        return randomIndex;
    }
    
    void OnDestroy()
    {
        StopSwitching();
    }
    
    #if UNITY_EDITOR
    void OnValidate()
    {
        // Ensure min duration is not negative
        if (minDuration < 0f)
        {
            minDuration = 0f;
        }
        
        // Ensure max duration is not less than min duration
        if (maxDuration < minDuration)
        {
            maxDuration = minDuration;
        }
    }
    #endif
}
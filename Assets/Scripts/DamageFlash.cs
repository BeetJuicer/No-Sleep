using System.Collections;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    [ColorUsage(true, true)]
    [SerializeField] private Color flashColor = Color.white;
    [SerializeField] private float flashTime = 0.25f;
    [SerializeField] private AnimationCurve flashCurve;
    private SpriteRenderer[] spriteRenderers;
    private Material[] materials;

    private Coroutine damageFlashCoroutine;
    private Combat combat;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        Init();
    }

    private void Start()
    {
        if(GetComponentInChildren<Combat>())
        {
            combat = GetComponentInChildren<Combat>();
            combat.onDamaged += CallDamageFlash;
        }
    }

    private void OnDestroy()
    {
        combat.onDamaged -= CallDamageFlash;
    }

    private void Init()
    {
        materials = new Material[spriteRenderers.Length];
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            materials[i] = spriteRenderers[i].material; 
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void CallDamageFlash()
    {
        damageFlashCoroutine = StartCoroutine(Damageflasher());   
    }

    private IEnumerator Damageflasher()
    {
        SetFlashColor();

        //lerp the flash amount
        float currentFlashAmount = 0f;
        float elapsedTime = 0f;

        while (elapsedTime < flashTime)
        {
            //iterate elapsed time.
            elapsedTime += Time.deltaTime;
            //lerp flash amount
            currentFlashAmount = Mathf.Lerp(1f, flashCurve.Evaluate(elapsedTime), (elapsedTime / flashTime));
            SetFlashAmount(currentFlashAmount);
            
            yield return null;
        }

    }
    private void SetFlashColor()
    {
        //set the color
        foreach (var material in materials)
        {
            print("setting flash color");
            material.SetColor("_FlashColor", flashColor);
        }
    }

    private void SetFlashAmount(float amount)
    {
        print("setting flash amount");

        foreach (var material in materials)
        {
            material.SetFloat("_FlashAmount", amount);
        }   
    }
}

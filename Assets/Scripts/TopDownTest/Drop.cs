using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public enum DropType
{
    Health,
    Territory,
    Weapon,
    Ammo,
    Currency,
    Powerup,
    Custom
}

[System.Serializable]
public class DropEffect
{
    [Header("Effect Settings")]
    public DropType dropType = DropType.Health;
    public float value = 1f;
    public string customEffectName = "";

    [Header("Territory Specific")]
    public float territoryExpandAmount = 0.5f;
    public float territoryExpandDuration = 2f;

    [Header("Weapon Specific")]
    public GameObject weaponPrefab;
    public float weaponDamageMultiplier = 1f;
    public float weaponDuration = 10f;

    [Header("Powerup Specific")]
    public float powerupDuration = 5f;
    public float speedMultiplier = 1.5f;
    public float damageMultiplier = 2f;
    public bool invincibility = false;
}

namespace TopDown
{



public class Drop : MonoBehaviour
{
    [Header("Drop Configuration")]
    [SerializeField] private DropEffect dropEffect;
    [SerializeField] private string playerTag = "Player";

    [Header("Visual & Audio")]
    [SerializeField] private GameObject pickupVFX;
    [SerializeField] private AudioClip pickupSound;
    [SerializeField] private float floatAmplitude = 0.5f;
    [SerializeField] private float floatSpeed = 2f;
    [SerializeField] private bool rotateOverTime = true;
    [SerializeField] private float rotationSpeed = 90f;

    [Header("Behavior")]
    [SerializeField] private bool destroyOnPickup = true;
    [SerializeField] private float lifeTime = 30f; // Auto-destroy after time
    [SerializeField] private bool magneticToPlayer = false;
    [SerializeField] private float magnetRange = 3f;
    [SerializeField] private float magnetStrength = 5f;

    [Header("Events")]
    public UnityEvent<GameObject> OnPickedUp;
    public UnityEvent OnExpired;

    private Vector3 startPosition;
    private float timeAlive = 0f;
    private Transform playerTransform;
    private AudioSource audioSource;
    private Rigidbody2D rb2D;

    void Start()
    {
        startPosition = transform.position;
        audioSource = GetComponent<AudioSource>();
        rb2D = GetComponent<Rigidbody2D>();

        // Find player for magnetic effect
        if (magneticToPlayer)
        {
            GameObject player = GameObject.FindGameObjectWithTag(playerTag);
            if (player != null)
                playerTransform = player.transform;
        }

        // Auto-destroy after lifetime
        if (lifeTime > 0)
        {
            Invoke(nameof(ExpireDrop), lifeTime);
        }
    }

    void Update()
    {
        timeAlive += Time.deltaTime;

        // Floating animation
        if (floatAmplitude > 0)
        {
            Vector3 pos = startPosition;
            pos.y += Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
            transform.position = pos;
        }

        // Rotation animation
        if (rotateOverTime)
        {
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        }

        // Magnetic effect
        if (magneticToPlayer && playerTransform != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
            if (distanceToPlayer <= magnetRange)
            {
                Vector3 direction = (playerTransform.position - transform.position).normalized;
                if (rb2D != null)
                {
                    rb2D.AddForce(direction * magnetStrength);
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position,
                        playerTransform.position, magnetStrength * Time.deltaTime);
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            PickupDrop(other.gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            PickupDrop(other.gameObject);
        }
    }

    private void PickupDrop(GameObject player)
    {
        // Play pickup effects
        PlayPickupEffects();

        // Apply the drop effect
        ApplyDropEffect(player);

        // Trigger events
        OnPickedUp?.Invoke(player);

        // Destroy or disable the drop
        if (destroyOnPickup)
        {
            Destroy(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void ApplyDropEffect(GameObject player)
    {
        switch (dropEffect.dropType)
        {
            case DropType.Health:
                ApplyHealthEffect(player);
                break;

            case DropType.Territory:
                ApplyTerritoryEffect(player);
                break;

            case DropType.Weapon:
                ApplyWeaponEffect(player);
                break;

            case DropType.Ammo:
                ApplyAmmoEffect(player);
                break;

            case DropType.Currency:
                ApplyCurrencyEffect(player);
                break;

            case DropType.Powerup:
                ApplyPowerupEffect(player);
                break;

            case DropType.Custom:
                ApplyCustomEffect(player);
                break;
        }
    }

    private void ApplyHealthEffect(GameObject player)
    {
        //// Try different health component names
        //var health = player.GetComponent<Health>() ??
        //            player.GetComponent<PlayerHealth>() ??
        //            player.GetComponent<HealthSystem>();

        //if (health != null)
        //{
        //    // Use reflection to call common heal methods
        //    var healMethod = health.GetType().GetMethod("Heal") ??
        //                   health.GetType().GetMethod("AddHealth") ??
        //                   health.GetType().GetMethod("RestoreHealth");

        //    healMethod?.Invoke(health, new object[] { dropEffect.value });
        //}

        //Debug.Log($"Player healed for {dropEffect.value} health");
    }

    private void ApplyTerritoryEffect(GameObject player)
    {
            // Find territory system
        var territory = FindFirstObjectByType<TerritoryShrink>();

        if (territory != null)
        {
                // Try to expand territory
            territory.ExpandTerritory(dropEffect.territoryExpandAmount);
        }

        Debug.Log($"Territory expanded by {dropEffect.territoryExpandAmount}");
    }

    private void ApplyWeaponEffect(GameObject player)
    {
        //if (dropEffect.weaponPrefab != null)
        //{
        //    // Spawn weapon or give to player
        //    var weaponSystem = player.GetComponent<WeaponSystem>() ??
        //                      player.GetComponent<PlayerWeapons>();

        //    if (weaponSystem != null)
        //    {
        //        var giveWeaponMethod = weaponSystem.GetType().GetMethod("GiveWeapon") ??
        //                             weaponSystem.GetType().GetMethod("AddWeapon");

        //        giveWeaponMethod?.Invoke(weaponSystem, new object[] { dropEffect.weaponPrefab });
        //    }
        //}

        //Debug.Log($"Weapon pickup applied");
    }

    private void ApplyAmmoEffect(GameObject player)
    {
        //var weaponSystem = player.GetComponent<WeaponSystem>() ??
        //                  player.GetComponent<PlayerWeapons>();

        //if (weaponSystem != null)
        //{
        //    var addAmmoMethod = weaponSystem.GetType().GetMethod("AddAmmo") ??
        //                      weaponSystem.GetType().GetMethod("GiveAmmo");

        //    addAmmoMethod?.Invoke(weaponSystem, new object[] { (int)dropEffect.value });
        //}

        //Debug.Log($"Added {dropEffect.value} ammo");
    }

    private void ApplyCurrencyEffect(GameObject player)
    {
        //var currency = player.GetComponent<Currency>() ??
        //              player.GetComponent<PlayerCurrency>() ??
        //              FindObjectOfType<GameManager>();

        //if (currency != null)
        //{
        //    var addMoneyMethod = currency.GetType().GetMethod("AddMoney") ??
        //                       currency.GetType().GetMethod("AddCurrency") ??
        //                       currency.GetType().GetMethod("AddCoins");

        //    addMoneyMethod?.Invoke(currency, new object[] { (int)dropEffect.value });
        //}

        //Debug.Log($"Added {dropEffect.value} currency");
    }

    private void ApplyPowerupEffect(GameObject player)
    {
        //var powerupSystem = player.GetComponent<PowerupSystem>() ??
        //                   player.GetComponent<PlayerPowerups>();

        //if (powerupSystem != null)
        //{
        //    var applyPowerupMethod = powerupSystem.GetType().GetMethod("ApplyPowerup");
        //    applyPowerupMethod?.Invoke(powerupSystem, new object[] { dropEffect });
        //}
        //else
        //{
        //    // Create a simple powerup coroutine
        //    StartCoroutine(ApplyTempPowerup(player));
        //}

        //Debug.Log($"Powerup applied for {dropEffect.powerupDuration} seconds");
    }

    private System.Collections.IEnumerator ApplyTempPowerup(GameObject player)
    {
        // This is a basic implementation - you'd want to integrate with your player systems
        Debug.Log($"Powerup active: Speed x{dropEffect.speedMultiplier}, Damage x{dropEffect.damageMultiplier}");

        yield return new WaitForSeconds(dropEffect.powerupDuration);

        Debug.Log("Powerup expired");
    }

    private void ApplyCustomEffect(GameObject player)
    {
        // Send a message for custom handling
        player.SendMessage("OnCustomDropPickup", dropEffect.customEffectName, SendMessageOptions.DontRequireReceiver);

        Debug.Log($"Custom effect applied: {dropEffect.customEffectName}");
    }

    private void PlayPickupEffects()
    {
        // Spawn VFX
        if (pickupVFX != null)
        {
            Instantiate(pickupVFX, transform.position, transform.rotation);
        }

        // Play sound
        if (pickupSound != null)
        {
            if (audioSource != null)
            {
                audioSource.PlayOneShot(pickupSound);
            }
            else
            {
                AudioSource.PlayClipAtPoint(pickupSound, transform.position);
            }
        }
    }

    private void ExpireDrop()
    {
        OnExpired?.Invoke();

        if (destroyOnPickup)
        {
            Destroy(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    // Public methods for external control
    public void SetDropEffect(DropEffect newEffect)
    {
        dropEffect = newEffect;
    }

    public void SetLifetime(float newLifetime)
    {
        lifeTime = newLifetime;
        CancelInvoke(nameof(ExpireDrop));
        if (lifeTime > 0)
        {
            Invoke(nameof(ExpireDrop), lifeTime);
        }
    }

    // Gizmos for debugging
    void OnDrawGizmosSelected()
    {
        if (magneticToPlayer)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, magnetRange);
        }
    }
}
}
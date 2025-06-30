using UnityEngine;

namespace TopDown
{
    public class GunController : MonoBehaviour
    {
        [Header("Gun Settings")]
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private Transform firePoint;
        [SerializeField] private Transform visualGun; // The visual gun object to rotate

        [Header("Gun Stats")]
        [SerializeField] private float projectileSpeed = 10f;
        [SerializeField] private float projectileDistance = 20f;
        [SerializeField] private float damage = 25f;
        [SerializeField] private float fireRate = 0.2f; // Time between shots

        [Header("Aiming Settings")]
        [SerializeField] private bool smoothRotation = true;
        [SerializeField] private float rotationSpeed = 10f;
        [SerializeField] private Vector3 rotationOffset = new Vector3(0, 0, -90); // Adjust based on sprite orientation

        private Camera playerCamera;
        private float timeLastShot;
        private Vector3 mouseWorldPosition;
        private Vector3 aimDirection;
        private float targetAngle;

        public Vector3 AimDirection => aimDirection;
        public bool CanShoot => Time.time >= timeLastShot + fireRate;

        private void Awake()
        {
            // Get the camera (usually main camera)
            playerCamera = Camera.main;
            if (playerCamera == null)
                playerCamera = FindObjectOfType<Camera>();

            // If no fire point is assigned, use this transform
            if (firePoint == null)
                firePoint = transform;

            // If no visual gun is assigned, use this transform
            if (visualGun == null)
                visualGun = transform;
        }

        private void Update()
        {
            HandleAiming();
            HandleShooting();
        }

        private void HandleAiming()
        {
            // Get mouse position in world space
            Vector3 mouseScreenPosition = Input.mousePosition;
            mouseWorldPosition = playerCamera.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, playerCamera.nearClipPlane));
            mouseWorldPosition.z = transform.position.z; // Keep same Z level

            // Calculate aim direction
            aimDirection = (mouseWorldPosition - transform.position).normalized;

            // Calculate rotation angle
            targetAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

            // Apply rotation to visual gun
            RotateGun();
        }

        private void RotateGun()
        {
            Quaternion targetRotation = Quaternion.Euler(rotationOffset + new Vector3(0, 0, targetAngle));

            if (smoothRotation)
            {
                visualGun.rotation = Quaternion.Lerp(visualGun.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
            else
            {
                visualGun.rotation = targetRotation;
            }
        }

        private void HandleShooting()
        {
            if (Input.GetMouseButton(0) && CanShoot)
            {
                FireProjectile();
            }
        }

        private void FireProjectile()
        {
            if (bulletPrefab == null)
            {
                Debug.LogWarning("Bullet prefab is not assigned!");
                return;
            }

            // Calculate projectile rotation (Z rotation for 2D)
            Vector3 projectileRot = new Vector3(0, 0, targetAngle);

            // Instantiate bullet at fire point with proper rotation
            var bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.Euler(projectileRot));
            timeLastShot = Time.time;

            // Configure projectile if it has the Projectile component
            if (bullet.TryGetComponent(out Projectile projectile))
            {
                projectile.FireProjectile(projectileSpeed, projectileDistance, damage);
            }

            // Optional: Add recoil or muzzle flash effects here
            OnProjectileFired();
        }

        private void OnProjectileFired()
        {
            // Override this or add events for muzzle flash, sound effects, etc.
            // Example: Play sound, spawn muzzle flash, add screen shake
        }

        // Public methods for external control
        public void SetFireRate(float newFireRate)
        {
            fireRate = newFireRate;
        }

        public void SetDamage(float newDamage)
        {
            damage = newDamage;
        }

        public void SetProjectileSpeed(float newSpeed)
        {
            projectileSpeed = newSpeed;
        }

        // Manual fire method for other scripts
        public void Fire()
        {
            if (CanShoot)
            {
                FireProjectile();
            }
        }
    }

}
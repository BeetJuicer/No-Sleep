using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    private enum AimDirection
    {
        Up,
        Right,
        Diagonal,
        Down
    }

    [Header("External")]
    [SerializeField] private Movement playerMovement;
    Camera cam;
    
    [Header("Shooting")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float shotsPerSecond;
    [SerializeField] private int projectileSpeed;
    [SerializeField] private int damage;
    [SerializeField] private int projectileDistance;
    private float timeLastShot;
    private float cooldownPerShot;
    AimDirection aimDir = AimDirection.Right;

    [Header("Rotations")]
    [SerializeField] private Vector3 rotationUp;
    [SerializeField] private Vector3 rotationDown;
    [SerializeField] private Vector3 rotationRight;
    [SerializeField] private Vector3 rotationLeft;
    [SerializeField] private Vector3 rotationDiagonal;

    private Dictionary<AimDirection, Vector3> rotations = new Dictionary<AimDirection, Vector3> {
        { AimDirection.Up,       new Vector3(0f,0f,90f) },
        { AimDirection.Down,     new Vector3(0f,0f,-90f) },
        { AimDirection.Right,    new Vector3(0f,0f,0f) },
        { AimDirection.Diagonal, new Vector3(0f,0f,45f) },
    };


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = FindFirstObjectByType<Camera>();
        cooldownPerShot = 1f / shotsPerSecond;

    }

    // Update is called once per frame
    void Update()
    {
        HandleRotation();
        HandleShooting();
    }

    void HandleRotation()
    {
        if (Input.GetKey(KeyCode.W))
        {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                transform.SetLocalPositionAndRotation(transform.localPosition, Quaternion.Euler(rotations[AimDirection.Diagonal]));
                aimDir = AimDirection.Diagonal;
            }
            else
            {
                transform.SetLocalPositionAndRotation(transform.localPosition, Quaternion.Euler(rotations[AimDirection.Up]));
                aimDir = AimDirection.Up;
            }
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.SetLocalPositionAndRotation(transform.localPosition, Quaternion.Euler(rotations[AimDirection.Down]));
            aimDir = AimDirection.Down;
        }
        else
        {
            transform.SetLocalPositionAndRotation(transform.localPosition, Quaternion.Euler(rotations[AimDirection.Right]));
            aimDir = AimDirection.Right;
        }
    }
    void HandleShooting()
    {
        if (timeLastShot + cooldownPerShot >= Time.time)
            return;

        if(Input.GetKey(KeyCode.J))
        {
            Vector3 projectileRot = rotations[aimDir];
            float yrot = (playerMovement.FacingDirection == 1) ? 0 : 180f; //rotate face according to faceDirection
            projectileRot.y = yrot;

            print(projectileRot);

            var bullet = Instantiate(bulletPrefab, transform.position, Quaternion.Euler(projectileRot));
            timeLastShot = Time.time;
            
            if(bullet.TryGetComponent(out Projectile projectile))
            {
                projectile.FireProjectile(projectileSpeed, projectileDistance, damage);
            }
        }
    }
}

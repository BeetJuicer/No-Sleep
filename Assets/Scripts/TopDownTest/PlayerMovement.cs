using UnityEngine;
namespace TopDown
{

    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float acceleration = 10f;
        [SerializeField] private float deceleration = 10f;

        [Header("Optional Settings")]
        [SerializeField] private bool usePhysicsMovement = true;
        [SerializeField] private bool smoothMovement = true;

        private Rigidbody2D rb;
        private PlayerInput playerInput;
        private Vector2 currentVelocity;
        private Vector2 targetVelocity;

        public Vector2 CurrentVelocity => currentVelocity;
        public bool IsMoving => currentVelocity.magnitude > 0.1f;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            playerInput = GetComponent<PlayerInput>();

            // Configure Rigidbody2D for top-down movement
            rb.gravityScale = 0f;
            rb.linearDamping = 0f;
            rb.angularDamping = 0f;
            rb.freezeRotation = true;
        }

        private void FixedUpdate()
        {
            if (GameManager.Instance.CurrentGameState != GameManager.GameState.Gameplay)
            {
                rb.linearVelocity = Vector2.zero;
                return;
            }

            if (playerInput != null)
            {
                HandleMovement(playerInput.MovementInput);
            }
        }

        private void HandleMovement(Vector2 input)
        {
            targetVelocity = input * moveSpeed;

            if (smoothMovement)
            {
                SmoothMovement();
            }
            else
            {
                DirectMovement();
            }

            if (usePhysicsMovement)
            {
                rb.linearVelocity = currentVelocity;
            }
            else
            {
                transform.position += (Vector3)currentVelocity * Time.fixedDeltaTime;
            }
        }

        private void SmoothMovement()
        {
            float accelRate = targetVelocity.magnitude > 0 ? acceleration : deceleration;
            currentVelocity = Vector2.MoveTowards(currentVelocity, targetVelocity, accelRate * Time.fixedDeltaTime);
        }

        private void DirectMovement()
        {
            currentVelocity = targetVelocity;
        }

        public void SetMoveSpeed(float newSpeed)
        {
            moveSpeed = newSpeed;
        }

        public void Stop()
        {
            currentVelocity = Vector2.zero;
            targetVelocity = Vector2.zero;
            if (usePhysicsMovement)
            {
                rb.linearVelocity = Vector2.zero;
            }
        }
    }
}
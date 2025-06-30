using UnityEngine;
namespace TopDown
{

    [RequireComponent(typeof(Animator))]
    public class PlayerAnimation : MonoBehaviour
    {
        [Header("Animation Parameters")]
        [SerializeField] private string isMovingParam = "IsMoving";
        [SerializeField] private string horizontalParam = "Horizontal";
        [SerializeField] private string verticalParam = "Vertical";
        [SerializeField] private string lastHorizontalParam = "LastHorizontal";
        [SerializeField] private string lastVerticalParam = "LastVertical";

        private Animator animator;
        private PlayerInput playerInput;
        private PlayerMovement playerMovement;

        private Vector2 lastMovementDirection = Vector2.down; // Default facing direction

        private void Awake()
        {
            animator = GetComponent<Animator>();
            playerInput = GetComponent<PlayerInput>();
            playerMovement = GetComponent<PlayerMovement>();
        }

        private void Update()
        {
            UpdateAnimations();
        }

        private void UpdateAnimations()
        {
            if (playerInput == null || animator == null) return;

            Vector2 input = playerInput.MovementInput;
            bool isMoving = playerInput.IsMoving;

            // Set movement state
            animator.SetBool(isMovingParam, isMoving);

            // Set current movement direction
            animator.SetFloat(horizontalParam, input.x);
            animator.SetFloat(verticalParam, input.y);

            // Update last movement direction for idle animations
            if (isMoving)
            {
                lastMovementDirection = input;
            }

            animator.SetFloat(lastHorizontalParam, lastMovementDirection.x);
            animator.SetFloat(lastVerticalParam, lastMovementDirection.y);
        }

        public void SetFacingDirection(Vector2 direction)
        {
            lastMovementDirection = direction.normalized;
            animator.SetFloat(lastHorizontalParam, lastMovementDirection.x);
            animator.SetFloat(lastVerticalParam, lastMovementDirection.y);
        }
    }
}
using UnityEngine;
namespace TopDown
{

    [RequireComponent(typeof(Animator))]
    public class PlayerAnimation : MonoBehaviour
    {
        [Header("Animation Parameters")]
        [SerializeField] private string horizontalParam = "horizontalMovement";
        [SerializeField] private string verticalParam = "verticalMovement";
        [SerializeField] private string isMovingParam = "isMoving";

        private Animator animator;
        private PlayerInput playerInput;
        private PlayerMovement playerMovement;
        SpriteRenderer spRenderer;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            playerInput = GetComponent<PlayerInput>();
            playerMovement = GetComponent<PlayerMovement>();
            spRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            UpdateAnimations();
        }

        private void UpdateAnimations()
        {
            if (playerInput == null || animator == null) return;

            Vector2 input = playerInput.MovementInput;
            bool isMoving = (Mathf.Abs(input.x) >= 0.1f) || (Mathf.Abs(input.y) >= 0.1f);


            // Set current movement direction
            animator.SetFloat(horizontalParam, input.x);
            animator.SetFloat(verticalParam, input.y);
            animator.SetBool(isMovingParam, isMoving);

            spRenderer.flipX = Mathf.Sign(input.x) < 0;
        }
    }
}
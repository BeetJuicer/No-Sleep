using UnityEngine;
using AYellowpaper.SerializedCollections;
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
        private PlayerMovement playerMovement;
        SpriteRenderer spRenderer;

        [SerializedDictionary]

        private void Awake()
        {
            animator = GetComponent<Animator>();
            playerMovement = GetComponent<PlayerMovement>();
            spRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            UpdateAnimations();

        }

        private void UpdateAnimations()
        {
            if (playerMovement == null || animator == null) return;

            Vector2 movement = playerMovement.CurrentVelocity;
            bool isMoving = (Mathf.Abs(movement.x) >= 0.1f) || (Mathf.Abs(movement.y) >= 0.1f);


            // Set current movement direction
            animator.SetFloat(horizontalParam, movement.x);
            animator.SetFloat(verticalParam, movement.y);
            animator.SetBool(isMovingParam, isMoving);

            spRenderer.flipX = Mathf.Sign(movement.x) < 0;
        }
    }
}
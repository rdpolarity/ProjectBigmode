using UnityEngine;
using UnityEngine.InputSystem;

namespace Bigmode
{
    public class PlayerController : Singleton<PlayerController>, Controls.IPlayerActions
    {
        // Configuration
        [SerializeField] private float force;
        [SerializeField] private float walkingThreshold = 0.2f;

        // Working vars
        public Animator walkIdleAnimator;
        private new Rigidbody2D rigidbody;
        private Controls.PlayerActions playerActions;
        private Vector2 movementInput;

        // Constants
        private static readonly int IsWalking = Animator.StringToHash("IsWalking");

        // Properties
        public Vector3Int Cell => transform.position.Cell();

        protected override void Awake()
        {
            rigidbody = GetComponent<Rigidbody2D>();
        }

        private void OnEnable()
        {
            playerActions = new Controls.PlayerActions(Settings.InputActions);
            playerActions.SetCallbacks(this);
            playerActions.Enable();
        }

        private void OnDisable()
        {
            playerActions.Disable();
        }

        private void Update()
        {
            var desired = movementInput * force;
            var delta = desired - rigidbody.velocity;

            rigidbody.AddForce(delta);

            var isMoving = rigidbody.velocity.magnitude > walkingThreshold;
            walkIdleAnimator.SetBool(IsWalking, isMoving);

            float speed = rigidbody.velocity.magnitude;
            if (isMoving) walkIdleAnimator.speed = speed / 2;
            else walkIdleAnimator.speed = 1;
        }

        // IPlayerActions

        public void OnMovement(InputAction.CallbackContext context)
        {
            movementInput = context.ReadValue<Vector2>();
        }
    }
}

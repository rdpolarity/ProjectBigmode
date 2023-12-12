using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace Bigmode
{
    public class PlayerController : Singleton<PlayerController>, Controls.IPlayerActions
    {
        // Configuration
        [SerializeField] private float force;

        // Working vars
        public Animator walkIdleAnimator;
        private new Rigidbody2D rigidbody;

        private Controls.PlayerActions playerActions;
        private Vector2 movementInput;

        // Constants

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

            rigidbody.AddForce(delta * Time.deltaTime);
        }

        // IPlayerActions

        public void OnMovement(InputAction.CallbackContext context)
        {
            movementInput = context.ReadValue<Vector2>();
        }

        public void OnSpawnMinion(InputAction.CallbackContext context)
        {
            if (context.phase != InputActionPhase.Performed) return;
            if (TryGetComponent<Biggie>(out var biggie))
            {
                biggie.SpawnSelectedMinion();
            }
        }

        public void OnNumberKeyPressed(InputAction.CallbackContext context)
        {
            if (context.phase != InputActionPhase.Performed) return;
            KeyControl keyControl = (KeyControl)context.control;
            string keycode = keyControl.name;
            int number = int.Parse(keycode);

            if (TryGetComponent<Biggie>(out var biggie))
            {
                biggie.SelectMinion(number - 1);
            }
        }

        public void OnTongue(InputAction.CallbackContext context)
        {
            TryGetComponent<Biggie>(out var biggie);
            if (biggie == null) return;

            // Holding down the tongue button
            if (context.phase == InputActionPhase.Started)
            {
                biggie.TongueGrab();
            }

            // Releasing the tongue button
            if (context.phase == InputActionPhase.Canceled)
            {
                biggie.TongueRelease();
            }
        }
    }
}

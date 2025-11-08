using UnityEngine;

namespace EclipseRealms.Core.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class TopDownMover : MonoBehaviour
    {
        [SerializeField] private float acceleration = 20f;
        [SerializeField] private float maxSpeed = 6f;
        [SerializeField] private float deceleration = 25f;
        [SerializeField] private float turnSpeedDegrees = 720f;
        [SerializeField] private float gravity = -30f;
        [SerializeField] private Transform facingProxy;

        private CharacterController controller;
        private Vector3 velocity;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            if (facingProxy == null)
            {
                facingProxy = transform;
            }
        }

        private void Update()
        {
            Vector2 moveInput = new(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            Vector3 desiredXZ = new(moveInput.x, 0f, moveInput.y);
            desiredXZ = Vector3.ClampMagnitude(desiredXZ, 1f);

            Vector3 currentXZ = new(velocity.x, 0f, velocity.z);

            if (desiredXZ.sqrMagnitude > 0.01f)
            {
                currentXZ = Vector3.MoveTowards(currentXZ, desiredXZ * maxSpeed, acceleration * Time.deltaTime);
                FaceDirection(desiredXZ);
            }
            else
            {
                currentXZ = Vector3.MoveTowards(currentXZ, Vector3.zero, deceleration * Time.deltaTime);
            }

            velocity.x = currentXZ.x;
            velocity.z = currentXZ.z;
            velocity.y += gravity * Time.deltaTime;

            controller.Move(velocity * Time.deltaTime);

            if (controller.isGrounded && velocity.y < 0f)
            {
                velocity.y = -2f; // keeps grounded
            }
        }

        private void FaceDirection(Vector3 direction)
        {
            Vector3 lookTarget = direction.normalized;
            if (lookTarget.sqrMagnitude < 0.0001f)
            {
                return;
            }

            Quaternion targetRotation = Quaternion.LookRotation(lookTarget, Vector3.up);
            facingProxy.rotation = Quaternion.RotateTowards(facingProxy.rotation, targetRotation, turnSpeedDegrees * Time.deltaTime);
        }
    }
}

using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Main.Scripts.Locomotion
{
    [RequireComponent(typeof(Rigidbody))]
    public class LocomotionMotor : MonoBehaviour
    {
        [SerializeField] private Transform rotationPivot;
        [Range(1, 20)]
        [SerializeField] private float movementSpeed = 5f;
        [Range(1, 20)]
        [SerializeField] private float rotationSpeed = 10;
        
        private Rigidbody _rigidbody;
        private Transform _cameraTransform;
        private Vector3 _movementDirection;
        public float Velocity => _rigidbody.velocity.magnitude;

        protected virtual void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        protected virtual void Start()
        {
            _rigidbody.useGravity = false;
            _rigidbody.freezeRotation = true;
        }

        protected void Update()
        {
            
        }

        protected virtual void FixedUpdate()
        {
            MoveCharacter();
        }

        public void HandleMovement(Vector2 direction)
        {
            if(_cameraTransform == null) return;
            
            var inputDirection = new Vector3(direction.x, 0,direction.y).normalized;
            
            if (inputDirection.magnitude >= 0.1f)
            {
                // Rotate player to face movement direction
                if (_movementDirection != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(_movementDirection);
                    rotationPivot.rotation = Quaternion.Slerp(rotationPivot.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                }
            }
            
            Vector3 camForward = _cameraTransform.forward;
            camForward.y = 0f;
            camForward.Normalize();

            Vector3 camRight = _cameraTransform.right;
            camRight.y = 0f;
            camRight.Normalize();
            
            _movementDirection = (camForward * inputDirection.z + camRight * inputDirection.x).normalized;
        }

        private void MoveCharacter()
        {
            _rigidbody.velocity = new Vector3(
                _movementDirection.x * movementSpeed, 
                _rigidbody.velocity.y, 
                _movementDirection.z * movementSpeed);
        }

        public void SetCamera(Transform cameraTransform)
        {
            _cameraTransform = cameraTransform;
        }
    }
}
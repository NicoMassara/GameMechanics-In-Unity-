using System;
using _Main.Scripts.Locomotion;
using UnityEngine;

namespace _Main.Scripts.AstroneerCrafting.Character
{
    public class CharacterInputs : MonoBehaviour
    {
        [SerializeField] private CharacterCamera characterCamera;
        private LocomotionMotor _motor;
        private AC_CharacterInputActions _inputs;

        private void Awake()
        {
            _motor = GetComponent<LocomotionMotor>();
            _inputs = new AC_CharacterInputActions();
        }

        private void Start()
        {
            _inputs.Default.Enable();
        }

        private void FixedUpdate()
        {
            var movementAxis = _inputs.Default.MovementAxis.ReadValue<Vector2>();
            _motor.HandleMovement(movementAxis);
        }

        private void LateUpdate()
        {
            var cameraAxis = _inputs.Default.CameraAxis.ReadValue<Vector2>();
            characterCamera.HandleMovement(cameraAxis);
        }
    }
}
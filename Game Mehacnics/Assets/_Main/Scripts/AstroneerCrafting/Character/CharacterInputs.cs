using System;
using _Main.Scripts.AstroneerCrafting.Managers;
using _Main.Scripts.Locomotion;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Main.Scripts.AstroneerCrafting.Character
{
    public class CharacterInputs : MonoBehaviour
    {
        [SerializeField] private CharacterCamera characterCamera;
        private AstroneerCharacterMotor _motor;
        private AC_CharacterInputActions _inputs;

        private void Awake()
        {
            _motor = GetComponent<AstroneerCharacterMotor>();
            _inputs = new AC_CharacterInputActions();

            _motor.OnInteract += OnInteractHandler;
        }

        private void Start()
        {
            _inputs.Default.Interact.performed += IA_Default_Interact_PerformedHandler;
            
            EnableInputs();
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
        
        private void EnableInputs()
        {
            GameManager.Instance.CameraManager.SetActiveCamera(characterCamera.gameObject.GetComponent<Camera>());
            _inputs.Default.Enable();
        }

        private void DisableInputs()
        {
            _inputs.Default.Disable();
        }

        private void OnInteractHandler(bool hasStarted)
        {
            if (hasStarted)
            {
                DisableInputs();
            }
            else
            {
                EnableInputs();
            }
        }
        
        private void IA_Default_Interact_PerformedHandler(InputAction.CallbackContext obj)
        {
            _motor.StartInteraction();
        }
    }
}
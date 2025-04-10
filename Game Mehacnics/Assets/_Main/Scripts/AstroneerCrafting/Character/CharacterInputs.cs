using System.Collections.Generic;
using _Main.Scripts.AstroneerCrafting.Managers;
using _Main.Scripts.AstroneerCrafting.ScreenMouse;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Main.Scripts.AstroneerCrafting.Character
{
    public class CharacterInputs : MonoBehaviour
    {
        [SerializeField] private CharacterCamera characterCamera;
        [SerializeField] private VirtualMouse virtualMouse;
        private AstroneerCharacterMotor _motor;
        private AC_CharacterInputActions _inputs;
        private Dictionary<CharacterInputType, InputData> _inputDic = new Dictionary<CharacterInputType, InputData>();
        private InputData _currentInput;

        private void Awake()
        {
            _motor = GetComponent<AstroneerCharacterMotor>();
            _inputs = new AC_CharacterInputActions();
            _motor.OnHandEnable += Motor_OnHandEnableHandler;
        }

        private void Start()
        {
            _motor.OnInteract += OnInteractHandler;
            _inputs.Default.Interact.performed += IA_Default_Interact_PerformedHandler;
            _inputs.Default.StartHand.performed += IA_Default_StartHand_PerformedHandler;
            _inputs.Hand.Leave.performed += IA_Hand_Leave_PerformedHandler;
            _inputs.Hand.Grab.performed += IA_Hand_Grab_PerformedHandler;
            
            _inputDic.Add(CharacterInputType.Default, new InputData(CharacterInputType.Default, _inputs.Default.Enable, _inputs.Default.Disable));
            _inputDic.Add(CharacterInputType.Interaction, new InputData(CharacterInputType.Interaction, _inputs.Printer.Enable, _inputs.Printer.Disable));
            _inputDic.Add(CharacterInputType.Hand, new InputData(CharacterInputType.Hand, _inputs.Hand.Enable, _inputs.Hand.Disable));

            ChangeInput(CharacterInputType.Default);
            virtualMouse.SetEnable(false);
        }

        private void IA_Hand_Grab_PerformedHandler(InputAction.CallbackContext obj)
        {
            _motor.GrabHand();
        }

        private void IA_Hand_Leave_PerformedHandler(InputAction.CallbackContext obj)
        {
            _motor.ToggleHand();
        }

        private void IA_Default_StartHand_PerformedHandler(InputAction.CallbackContext obj)
        {
            _motor.ToggleHand();
        }


        private void FixedUpdate()
        {
            var movementAxis = _inputs.Default.MovementAxis.ReadValue<Vector2>();
            _motor.HandleMovement(movementAxis);
        }

        private void LateUpdate()
        {
            if (_currentInput.InputType == CharacterInputType.Default)
            {
                var cameraAxis = _inputs.Default.CameraAxis.ReadValue<Vector2>();
                characterCamera.HandleMovement(cameraAxis);
            }
            else if (_currentInput.InputType == CharacterInputType.Hand)
            {
                var mouseDelta = _inputs.Hand.HandAxis.ReadValue<Vector2>();
                virtualMouse.Move(mouseDelta);
                _motor.CalculateMouseInWorld(virtualMouse.GetOffsetScreenPosition());
                
                var distanceAxis = _inputs.Hand.DistanceAxis.ReadValue<Vector2>();
                _motor.MoveHand(distanceAxis.y);
            }
        }

        private void ChangeInput(CharacterInputType inputType)
        {
            if (_currentInput != null)
            {
                _currentInput.DisableInput();
            }

            _currentInput = _inputDic[inputType];
            _currentInput.EnableInput();
        }

        private void OnInteractHandler(bool hasStarted)
        {
            var newInput = hasStarted ? CharacterInputType.Interaction : CharacterInputType.Default;

            if (hasStarted == false)
            {
                GameManager.Instance.CameraManager.SetActiveCamera(characterCamera.SelfCamera);
            }

            ChangeInput(newInput);
        }
        
        private void IA_Default_Interact_PerformedHandler(InputAction.CallbackContext obj)
        {
            _motor.StartInteraction();
        }
        
        private void Motor_OnHandEnableHandler(bool isEnable)
        {
            var newInput = isEnable ? CharacterInputType.Hand : CharacterInputType.Default;
            ChangeInput(newInput);
            virtualMouse.SetEnable(isEnable);
        }
    }

    public class InputData
    {
        public CharacterInputType InputType { get; private set; }

        public delegate void InputDelegate();

        public InputDelegate EnableInput { get; private set;}
        public InputDelegate DisableInput { get; private set; }

        public InputData(CharacterInputType inputType, InputDelegate enableInput, InputDelegate disableInput)
        {
            EnableInput = enableInput;
            DisableInput = disableInput;
            InputType = inputType;
        }
    }

    public enum CharacterInputType
    {
        Default,
        Interaction,
        Hand
    }
}
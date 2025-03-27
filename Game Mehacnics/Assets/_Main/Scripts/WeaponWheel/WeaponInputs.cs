using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Main.Scripts.WeaponWheel
{
    public class WeaponInputs : MonoBehaviour
    {
        [SerializeField] private bool isGamepad = true;
        [SerializeField] private WeaponStorage storage;
        public WeaponWheelInputActions _inputs;
        
        private Vector2 _lastMousePosition;
        private Vector2 _lastJoystickDirection;

        private void Awake()
        {
            _inputs = new WeaponWheelInputActions();
        }

        private void Start()
        {
            _inputs.DefaultActionMap.Enable();
            _inputs.DefaultActionMap.SlotAxis.started += Inputs_SlotAxis_StartedHandler;
            _inputs.DefaultActionMap.Select.started += Inputs_Select_StartedHandler;
        }

        private void Update()
        {
            if (isGamepad)
            {
                HandleJoystickPosition();
            }
            else
            {
                HandleMousePosition();
            }
        }

        private void HandleMousePosition()
        {
            Vector2 mousePosition = _inputs.DefaultActionMap.MousePosition.ReadValue<Vector2>();

            if (_lastMousePosition != mousePosition)
            {
                var screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
                var distance = Vector2.Distance(mousePosition, screenCenter);
                var direction = Vector2.zero;
                if (distance > 100)
                {
                    direction = screenCenter - mousePosition;
                }
                
                storage.CalculateAngle(direction);
                
                _lastMousePosition = mousePosition;
            }
        }

        private void HandleJoystickPosition()
        {
            var direction = _inputs.DefaultActionMap.LeftStickDirection.ReadValue<Vector2>();

            direction *= -1;
            
            storage.CalculateAngle(direction);
        }
        
        private void Inputs_Select_StartedHandler(InputAction.CallbackContext obj)
        {
            storage.SelectHighlightedSlot();
        }

        private void Inputs_SlotAxis_StartedHandler(InputAction.CallbackContext obj)
        {
            var value = obj.ReadValue<float>();
            if (value > 0.1f)
            {
                storage.ChangeIndexInSlot(doesIncrease: true);
            }
            else if (value < -0.1f)
            {
                storage.ChangeIndexInSlot(doesIncrease: false);
            }
        }
    }
}
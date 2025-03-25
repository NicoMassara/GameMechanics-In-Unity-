using System;
using TMPro;
using UnityEngine;

namespace _Main.Scripts.WeaponWheel
{
    public class Inputs : MonoBehaviour
    {
        [SerializeField] private WeaponStorage storage;
        [SerializeField] private WeaponWheelUI ui;
        
        private Vector3 _lastMousePosition;
        private Vector2 _lastJoystickDirection;

        private void Update()
        {
            HandleMousePosition();
            HandleJoystickPosition();
            CheckWheelInputs();
        }

        private void CheckWheelInputs()
        {
            if (!Input.anyKeyDown) return;
            
            int selectedIndex = -1;

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                selectedIndex = 0;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                selectedIndex = 1;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                selectedIndex = 2;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                selectedIndex = 3;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                selectedIndex = 4;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                selectedIndex = 5;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                selectedIndex = 6;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                selectedIndex = 7;
            }
            
            if(selectedIndex > -1)
            {
                storage.SelectWeaponByIndex(selectedIndex);
            }

            if (Input.GetButtonDown("Accept"))
            {
                ui.SelectSlot();
            }
        }

        private void HandleMousePosition()
        {
            var mousePosition = Input.mousePosition;

            if (_lastMousePosition == mousePosition)
            {
                ui.ClearCheckDelay();
                return;
            }

            var direction =  ui.CenterPosition - mousePosition;
            
            ui.CalculateAngle(direction);
            _lastMousePosition = mousePosition;
        }

        private void HandleJoystickPosition()
        {
            var direction = new Vector2(
                Input.GetAxis("ControlHorizontal"), 
                Input.GetAxis("ControlVertical")).normalized;

            direction *= -1;
            
            if (_lastJoystickDirection == direction)
            {
                ui.ClearCheckDelay();
                return;
            }
            
            ui.CalculateAngle(direction);
            _lastJoystickDirection = direction;
        }
    }
}
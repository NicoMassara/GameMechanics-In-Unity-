using System;
using _Main.Scripts.AstroneerCrafting.Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Main.Scripts.AstroneerCrafting.Printer
{
    public class PrinterInputs : MonoBehaviour
    {
        [SerializeField] private Camera printerCamera;
        private PrinterModel _model;
        private AC_CharacterInputActions _inputs;
        private float _startInputDelay = 0.25f;
        private float _startInputDelayTimer;
        
        private void Awake()
        {
            _model = GetComponent<PrinterModel>();
            _inputs = new AC_CharacterInputActions();
            _model.OnInteract += OnInteractHandler;
            
            printerCamera.gameObject.SetActive(false);
        }
        
        private void Start()
        {
            _inputs.Printer.CycleRecipe.performed += IA_Printer_CycleRecipe_PerformedHandler;
            _inputs.Printer.Print.performed += IA_Printer_Print_PerformedHandler;
            _inputs.Printer.Leave.performed += IA_Printer_Leave_PerformedHandler;
        }

        private void Update()
        {
            if (_startInputDelayTimer > 0)
            {
                _startInputDelayTimer -= Time.deltaTime;
                if (_startInputDelayTimer <= 0)
                {
                    _inputs.Printer.Enable();
                }
            }
        }

        private void ResetTimer()
        {
            _startInputDelayTimer = _startInputDelay;
        }

        private void EnableInput()
        {
            GameManager.Instance.CameraManager.SetActiveCamera(printerCamera);
            ResetTimer();
        }

        private void DisableInput()
        {
            _inputs.Printer.Disable();
        }
        
        private void OnInteractHandler(bool hasStarted)
        {
            if (hasStarted)
            {
                EnableInput();
            }
            else
            {
                DisableInput();
            }
        }
        
        private void IA_Printer_Leave_PerformedHandler(InputAction.CallbackContext obj)
        {
            _model.StopInteraction();
        }

        private void IA_Printer_Print_PerformedHandler(InputAction.CallbackContext obj)
        {
            _model.Print();
        }

        private void IA_Printer_CycleRecipe_PerformedHandler(InputAction.CallbackContext obj)
        {
            var value = obj.ReadValue<float>();

            if (value >= 0.1f)
            {
                _model.CycleRecipes(true);
            }
            else if (value <= -0.1f)
            {
                _model.CycleRecipes(false);
            }
        }
    }
}
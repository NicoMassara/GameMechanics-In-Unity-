using System;
using _Main.Scripts.AstroneerCrafting.Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Main.Scripts.AstroneerCrafting.Printer
{
    public class PrinterInputs : MonoBehaviour
    {
        [SerializeField] private Camera printerCamera;
        private PrinterModel _printerModel;
        private AC_CharacterInputActions _inputs;

        private void Awake()
        {
            _printerModel = GetComponent<PrinterModel>();
            _inputs = new AC_CharacterInputActions();
            _printerModel.OnInteract += OnInteractHandler;
            
            printerCamera.gameObject.SetActive(false);
        }

        private void Start()
        {
            _inputs.Printer.CycleRecipe.performed += IA_Printer_CycleRecipe_PerformedHandler;
            _inputs.Printer.Print.performed += IA_Printer_Print_PerformedHandler;
            _inputs.Printer.Leave.performed += IA_Printer_Leave_PerformedHandler;
        }

        private void EnableInput()
        {
            GameManager.Instance.CameraManager.SetActiveCamera(printerCamera);
            _inputs.Printer.Enable();
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
            _printerModel.StopInteraction();
        }

        private void IA_Printer_Print_PerformedHandler(InputAction.CallbackContext obj)
        {
            _printerModel.StartPrinting();
        }

        private void IA_Printer_CycleRecipe_PerformedHandler(InputAction.CallbackContext obj)
        {
            var value = obj.ReadValue<float>();

            if (value >= 0.1f)
            {
                _printerModel.CycleRecipes(true);
            }
            else if (value <= -0.1f)
            {
                _printerModel.CycleRecipes(false);
            }
        }
    }
}
using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Main.Scripts.AstroneerCrafting.Printer
{
    public class PrinterView : MonoBehaviour
    {
        private PrinterModel _printerModel;
        [SerializeField] private MeshRenderer lightMesh;
        [SerializeField] private Color idleColor = Color.red;
        [SerializeField] private Color readyColor = Color.green;
        [SerializeField] private Color printingColor = Color.yellow;
        [SerializeField] private Color printedColor = Color.white;

        private void Awake()
        {
            _printerModel = GetComponent<PrinterModel>();
            
            _printerModel.OnReadyToPrint += OnReadyToPrintHandler;
            _printerModel.OnPrint += OnPrintHandler;
            _printerModel.OnPrintEnd += OnPrintEndHandler;
        }

        private void OnReadyToPrintHandler(bool isReady)
        {
            lightMesh.material.color = isReady ? readyColor : idleColor;
        }

        private void OnPrintHandler()
        {
            lightMesh.material.color = printingColor;
        }
        
        private void OnPrintEndHandler()
        {
            lightMesh.material.color = printedColor;
        }
    }
}
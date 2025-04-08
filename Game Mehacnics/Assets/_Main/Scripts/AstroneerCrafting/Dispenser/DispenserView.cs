using System;
using _Main.Scripts.AstroneerCrafting.Managers;
using UnityEngine;

namespace _Main.Scripts.AstroneerCrafting.Dispenser
{
    public class DispenserView : MonoBehaviour
    {
        private DispenserModel _model;
        [Header("Lights")]
        [SerializeField] private MeshRenderer lightMesh;
        [SerializeField] private Color idleColor = Color.red;
        [SerializeField] private Color printingColor = Color.yellow;
        [SerializeField] private Color printedColor = Color.green;
        [SerializeField] private SpriteRenderer colorMesh;
        
        private void Awake()
        {
            _model = GetComponent<DispenserModel>();
            _model.OnPrinted += OnPrintedHandler;
            _model.OnPrinting += OnPrintingHandler;
            _model.OnReadyToPrint += OnReadyToPrintHandler;
        }

        private void Start()
        {
            colorMesh.material.color = GameManager.Instance.MaterialManager.GetMaterialDataByEnum(_model.MaterialEnum).SelfColor;
            SetLightColor(idleColor);
        }

        private void SetLightColor(Color color)
        {
            lightMesh.material.color = color;
        }

        private void OnPrintingHandler()
        {
            SetLightColor(printingColor);
        }

        private void OnPrintedHandler()
        {
            SetLightColor(printedColor);
        }
        
        private void OnReadyToPrintHandler()
        {
            SetLightColor(idleColor);
        }
    }
}
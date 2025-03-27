using System;
using _Main.Scripts.AstroneerCrafting.Managers;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Main.Scripts.AstroneerCrafting.Printer
{
    public class PrinterView : MonoBehaviour
    {
        private PrinterModel _printerModel;
        [Header("Lights")]
        [SerializeField] private MeshRenderer lightMesh;
        [SerializeField] private Color idleColor = Color.red;
        [SerializeField] private Color readyColor = Color.green;
        [SerializeField] private Color printingColor = Color.yellow;
        [SerializeField] private Color printedColor = Color.white;
        [Header("Recipe Visuals")]
        [SerializeField] private SpriteRenderer[] slotVisual;
        [SerializeField] private SpriteRenderer slotOutVisual;

        private void Awake()
        {
            _printerModel = GetComponent<PrinterModel>();
            
            _printerModel.OnReadyToPrint += OnReadyToPrintHandler;
            _printerModel.OnPrint += OnPrintHandler;
            _printerModel.OnPrintEnd += OnPrintEndHandler;
            _printerModel.OnRecipeChange += OnRecipeChangeHandler;
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
        
        private void OnRecipeChangeHandler(RecipeEnum recipeEnum)
        {
            if(recipeEnum == RecipeEnum.None) return;
            
            var gameManager = GameManager.Instance;
            
            var recipeData = gameManager.RecipeManager.GetRecipeDataByEnum(recipeEnum);

            var materialsNeeded = recipeData.MaterialNeeded;
            var materialManager = gameManager.MaterialManager;
            var materialCount = recipeData.MaterialNeededCount;

            ResetSlotsColor();
            
            if (materialCount > slotVisual.Length)
            {
                Debug.Log("This Recipe is too large to be printed");
                return;
            }
            

            for (int i = 0; i < materialCount; i++)
            {
                var materialData = materialManager.GetMaterialDataByEnum(materialsNeeded[i]);
                slotVisual[i].color = materialData.SelfColor;
            }
            
            slotOutVisual.color = materialManager.GetMaterialDataByEnum(recipeData.MaterialGotten).SelfColor;
        }

        private void ResetSlotsColor()
        {
            foreach (var slot in slotVisual)
            {
                slot.color = Color.clear;
            }
        }
    }
}
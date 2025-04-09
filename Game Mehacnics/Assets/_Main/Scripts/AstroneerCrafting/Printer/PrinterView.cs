using _Main.Scripts.AstroneerCrafting.Managers;
using UnityEngine;

namespace _Main.Scripts.AstroneerCrafting.Printer
{
    public class PrinterView : MonoBehaviour
    {
        private PrinterModel _model;
        [Header("Lights")]
        [SerializeField] private MeshRenderer lightMesh;
        [SerializeField] private Color idleColor = Color.red;
        [SerializeField] private Color readyColor = Color.green;
        [SerializeField] private Color printingColor = Color.yellow;
        [SerializeField] private Color printedColor = Color.white;
        [Header("Recipe Visuals")]
        [SerializeField] private SpriteRenderer[] slotVisual;
        [SerializeField] private SpriteRenderer slotOutVisual;
        
        private MaterialManager _materialManager;
        private RecipeManager _recipeManager;
        
        private void Awake()
        {
            _model = GetComponent<PrinterModel>();
            
            _model.OnReadyToPrint += OnReadyToPrintHandler;
            _model.OnPrint += OnPrintHandler;
            _model.OnPrintEnd += OnPrintEndHandler;
            _model.OnRecipeChange += OnRecipeChangeHandler;

            _materialManager = GameManager.Instance.MaterialManager;
            _recipeManager = GameManager.Instance.RecipeManager;
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
        
        private void OnRecipeChangeHandler(MaterialEnum recipeEnum)
        {
            if(recipeEnum == MaterialEnum.None) return;
            
            var recipeData = _recipeManager.GetRecipeDataByEnum(recipeEnum);
            var materialCount = recipeData.MaterialNeededCount;
            
            if (slotVisual.Length >= materialCount)
            {
                var materialsNeeded = recipeData.MaterialNeeded;

                ResetSlotsColor();
                
                for (int i = 0; i < materialCount; i++)
                {
                    var materialData = _materialManager.GetMaterialDataByEnum(materialsNeeded[i]);
                    slotVisual[i].color = materialData.SelfColor;
                }
            }
            
            slotOutVisual.color = _materialManager.GetMaterialDataByEnum(recipeData.MaterialGotten).SelfColor;
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
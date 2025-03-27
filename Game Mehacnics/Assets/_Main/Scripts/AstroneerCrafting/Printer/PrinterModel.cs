using System.Collections.Generic;
using _Main.Scripts.AstroneerCrafting.Managers;
using _Main.Scripts.AstroneerCrafting.Materials;
using UnityEngine;
using UnityEngine.Events;

namespace _Main.Scripts.AstroneerCrafting.Printer
{
    public class PrinterModel : MonoBehaviour
    {
        [Header("Values")]
        [Space]
        [Range(1, 15)]
        [SerializeField] private int timeToPrint;
        [Range(0,1)]
        [SerializeField] private float printDelay = 0.5f;
        [SerializeField] private RecipeEnum currentRecipe;
        [Header("Objects")]
        [Space] 
        [SerializeField] private PrinterSlot[] materialSlot;
        [SerializeField] private PrinterSlot outSlot;

        public RecipeEnum[] AvailableRecipes { get; protected set; }

        private List<PrinterSlot> _slotToGetMaterial = new List<PrinterSlot>();
        private GameManager _gameManager;
        private bool _canPrint = false;
        private bool _isPrinting = false;
        private float _printTimer = 0f;
        private MaterialEnum _itemToPrint;
        private float _printDelayTimer = 0f;
        
        public UnityAction<bool> OnReadyToPrint;
        public UnityAction OnPrint;
        public UnityAction OnPrintEnd;
        public UnityAction OnRecipeChange;

        private void Start()
        {
            _gameManager = GameManager.Instance;

            foreach (var slot in materialSlot)
            {
                slot.OnMaterialAttached += Slot_OnMaterialAttachedHandler;
                slot.OnMaterialDetached += Slot_OnMaterialDetachedHandler;
            }
            
            outSlot.OnMaterialDetached += OutSlot_OnMaterialDetachedHandler;

            OnRecipeChange += OnRecipeChangeHandler;
            OnReadyToPrint?.Invoke(false);
        }

        private void Update()
        {
            if (_isPrinting)
            {
                HandlePrinting();
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                StartPrinting();
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                OnRecipeChange.Invoke();
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                outSlot.DetachMaterial();
            }
        }

        private void StartPrinting()
        {
            if (_canPrint)
            {
                _isPrinting = true;
                _printTimer = timeToPrint + printDelay;
                
                var shrinkData = new ScaleData(0f, 1f, timeToPrint);
                
                foreach (PrinterSlot slot in _slotToGetMaterial)
                {
                    slot.ChangeMaterialScale(shrinkData);
                }
                
                _printDelayTimer = printDelay;
                OnPrint?.Invoke();
            }
            else
            {
                Debug.Log("Can't print the recipe!");
            }
        }

        private void CalculateRecipe()
        {
            if(outSlot.HasMaterialAttached) return;
            
            var recipeData = _gameManager.RecipeManager.GetRecipeDataByEnum(currentRecipe);
            var ingredientsCount = recipeData.MaterialNeededCount;
            var ingredientsCheck = 0; //Increase by one every time finds a correct ingredient
            
            _slotToGetMaterial.Clear();

            for (int i = 0; i < ingredientsCount; i++)
            {
                for (int j = 0; j < materialSlot.Length; j++)
                {
                    if (recipeData.MaterialNeeded[i] == materialSlot[j].GetAttachedMaterial())
                    {
                        ingredientsCheck++;
                        _slotToGetMaterial.Add(materialSlot[j]);
                    }
                }
            }
            

            if (ingredientsCheck == ingredientsCount)
            {
                _canPrint = true;
                _itemToPrint = recipeData.MaterialGotten;
            }
            else
            {
                _canPrint = false;
                _itemToPrint = MaterialEnum.None;
            }
            
            OnReadyToPrint?.Invoke(_canPrint);
        }
        
        private void HandlePrinting()
        {
            if (_printTimer > 0)
            {
                _printTimer -= Time.deltaTime;
                
                if (_printTimer <= 0)
                {
                    foreach (PrinterSlot slot in _slotToGetMaterial)
                    {
                        slot.DetachMaterial();
                    }
                    
                    _slotToGetMaterial.Clear();

                    OnPrintEnd?.Invoke();
                    _isPrinting = false;
                }
                
                if (_printDelayTimer > 0)
                {
                    _printDelayTimer -= Time.deltaTime;
                    if (_printDelayTimer <= 0)
                    {
                        PrintOutMaterial();
                    }
                }
            }
        }

        private void PrintOutMaterial()
        {
            var expandData = new ScaleData(1f, 0f, timeToPrint);
            var newMaterial = _gameManager.CreateMaterialObject();
            newMaterial.SetMaterial(_itemToPrint);
            outSlot.AttachMaterial(newMaterial);
            outSlot.SetMaterialScale(0f);
            outSlot.ChangeMaterialScale(expandData);
        }

        #region Handlers

        private void Slot_OnMaterialAttachedHandler()
        {
            CalculateRecipe();
        }
        
        private void Slot_OnMaterialDetachedHandler()
        {
            CalculateRecipe();
        }
        
        private void OnRecipeChangeHandler()
        {
            CalculateRecipe();
        }
        
        private void OutSlot_OnMaterialDetachedHandler()
        {
            CalculateRecipe();
        }

        #endregion
    }
}
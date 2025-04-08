using System;
using System.Collections.Generic;
using System.Transactions;
using _Main.Scripts.AstroneerCrafting.Managers;
using _Main.Scripts.AstroneerCrafting.Materials;
using UnityEngine;
using UnityEngine.Events;

namespace _Main.Scripts.AstroneerCrafting.Printer
{
    public abstract class PrinterModel : MonoBehaviour, IInteractable
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
        private MaterialEnum _itemToPrint;
        private bool _canPrint = false;
        private bool _isPrinting = false;
        private float _printTimer = 0f;
        private float _printDelayTimer = 0f;
        private int _currentRecipeIndex;
        private readonly float _cycleDelay = 0.75f;
        private float _cycleDelayTimer;


        public bool IsQuick { get; private set; } = true;
        public UnityAction<bool> OnReadyToPrint;
        public UnityAction OnPrint;
        public UnityAction OnPrintEnd;
        public UnityAction<RecipeEnum> OnRecipeChange;
        public Transform SelfTransform => transform;
        public event Action<bool> OnInteract;


        private void Awake()
        {
            _gameManager = GameManager.Instance;

            foreach (var slot in materialSlot)
            {
                slot.OnMaterialAttached += Slot_OnMaterialAttachedHandler;
                slot.OnMaterialDetached += Slot_OnMaterialDetachedHandler;
            }
            
            outSlot.OnMaterialDetached += OutSlot_OnMaterialDetachedHandler;

            OnRecipeChange += OnRecipeChangeHandler;
            
            SetRecipeList();
            SetCurrentRecipe(AvailableRecipes[0]);
        }

        private void Start()
        {
            OnReadyToPrint?.Invoke(false);
            OnRecipeChange.Invoke(currentRecipe);
        }

        protected virtual void Update()
        {
            if (_cycleDelayTimer > 0)
            {
                _cycleDelayTimer -= Time.deltaTime;
            }

            if (_isPrinting)
            {
                HandlePrinting();
            }
        }
        
        public void Interact()
        {
            OnInteract?.Invoke(true);
        }

        protected abstract void SetRecipeList();

        public void CycleRecipes(bool isPositive)
        {
            if(_isPrinting) return;
            if(AvailableRecipes == null) return;
            if(AvailableRecipes.Length <= 1) return;
            if(_cycleDelayTimer > 0) return;
            
            
            var recipeCount = AvailableRecipes.Length;
            
            _currentRecipeIndex = isPositive ? _currentRecipeIndex + 1 : _currentRecipeIndex - 1;


            if (_currentRecipeIndex >= recipeCount)
            {
                _currentRecipeIndex = 0;
            }
            else if (_currentRecipeIndex <= -1)
            {
                _currentRecipeIndex = recipeCount - 1;
            }
            
            SetCurrentRecipe(AvailableRecipes[_currentRecipeIndex]);
            _cycleDelayTimer = _cycleDelay;
        }

        protected void SetCurrentRecipe(RecipeEnum recipeEnum)
        {
            currentRecipe = recipeEnum;
            OnRecipeChange.Invoke(currentRecipe);
        }

        public void StartPrinting()
        {
            if(_isPrinting) return;
            if(outSlot.HasMaterialAttached) return;
            
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
            if(outSlot.HasMaterialAttached ||
               currentRecipe == RecipeEnum.None) return;
            
            var recipeData = _gameManager.RecipeManager.GetRecipeDataByEnum(currentRecipe);
            var ingredientsCount = recipeData.MaterialNeededCount;
            var ingredientsCheck = 0; //Increase by one every time finds a correct ingredient
            
            _slotToGetMaterial.Clear();

            for (int i = 0; i < ingredientsCount; i++)
            {
                for (int j = 0; j < materialSlot.Length; j++)
                {
                    //Debug.Log($"Material in Slot {j+1}: {materialSlot[j].GetAttachedMaterial()}");
                    
                    if (recipeData.MaterialNeeded[i] == materialSlot[j].GetAttachedMaterial())
                    {
                        ingredientsCheck++;
                        _slotToGetMaterial.Add(materialSlot[j]);
                    }
                }
            }
            
            //Debug.Log($"Needed: {ingredientsCount}, Check: {ingredientsCheck}");

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
                        slot.RemoveMaterial();
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
        
        private void OnRecipeChangeHandler(RecipeEnum recipeData)
        {
            CalculateRecipe();
        }
        
        private void OutSlot_OnMaterialDetachedHandler()
        {
            CalculateRecipe();
        }

        #endregion

        public void StopInteraction()
        {
            OnInteract?.Invoke(false);
        }
    }
}
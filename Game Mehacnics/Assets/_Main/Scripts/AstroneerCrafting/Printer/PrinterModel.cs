using System;
using _Main.Scripts.AstroneerCrafting.Managers;
using _Main.Scripts.AstroneerCrafting.Materials;
using UnityEngine;
using UnityEngine.Events;

namespace _Main.Scripts.AstroneerCrafting.Printer
{
    public class PrinterModel : MonoBehaviour, IInteractable
    {
        [Header("Values")]
        [SerializeField] private PrinterDataSo printerData;
        [Header("Objects")]
        [Space] 
        [SerializeField] private MaterialSlot[] inMaterialSlot;
        [SerializeField] private MaterialSlot outMaterialSlot;

        private PrinterRecipeController _recipeController;
        private PrintController _printController;
        
        private MaterialEnum _currentRecipe;
        private RecipeManager _recipeManager;
        private bool _canPrint;
        private MaterialSlot[] _slotsToUse;
        private bool _isPrinting;
        
        public Transform SelfTransform => transform;
        
        public event Action<bool> OnInteract;
        public UnityAction<bool> OnReadyToPrint;
        public UnityAction OnPrint;
        public UnityAction OnPrintEnd;
        public UnityAction<MaterialEnum> OnRecipeChange;

        private void Awake()
        {
            _recipeController = new PrinterRecipeController(printerData.AvailableRecipes);
            _printController = new PrintController(outMaterialSlot, GameManager.Instance.MaterialManager);
            
            _recipeController.OnRecipeChange += RecipeController_OnRecipeChangeHandler;
            _printController.OnPrinting += PrintController_OnPrintHandler;
            _printController.OnPrintEnd += PrintController_OnPrintEnd;

            foreach (var slot in inMaterialSlot)
            {
                slot.OnMaterialAttached += InMaterialSlot_OnMaterialAttachedHandler;
                slot.OnMaterialDetached += InMaterialSlot_OnMaterialDetachedHandler;
            }
            
            outMaterialSlot.OnMaterialDetached += OutMaterialSlot_OnMaterialDetachedHandler;

        }

        private void Start()
        {
            _recipeManager = GameManager.Instance.RecipeManager;
            _recipeController.Initialize();
            _printController.Initialize();
        }

        private void Update()
        {
            _recipeController.Execute();
            _printController.Execute();
        }

        public void Interact()
        {
            OnInteract?.Invoke(true);
        }
        
        public void StopInteraction()
        {
            OnInteract?.Invoke(false);
        }

        #region Recipe

        public void CycleRecipes(bool isForward)
        {
            if(_isPrinting) return;
            
            _recipeController.CycleRecipes(isForward);
        }

        private void CalculateRecipe()
        {
            if (outMaterialSlot.HasMaterialAttached)
            {
                return;
            }
            
            var tempTuple = _recipeController.CalculateRecipe(inMaterialSlot, _recipeManager.GetRecipeDataByEnum(_currentRecipe));
            _canPrint = tempTuple.Item1;
            _slotsToUse = tempTuple.Item2;

            OnReadyToPrint?.Invoke(_canPrint);
        }

        #endregion

        #region Printing

        public void Print()
        {
            if(_isPrinting == true) return;
            if(outMaterialSlot.HasMaterialAttached) return;
            if (_canPrint == false)
            {
                Debug.Log("Can't print the recipe!");
                return;
            }

            var recipeData = _recipeManager.GetRecipeDataByEnum(_currentRecipe);
            _printController.StartPrint(recipeData, _slotsToUse, printerData.TimeToPrint);
            _isPrinting = true;
            _canPrint = false;
        }

        #endregion


        #region Handlers

        private void InMaterialSlot_OnMaterialDetachedHandler()
        {
            CalculateRecipe();
        }
        
        private void InMaterialSlot_OnMaterialAttachedHandler()
        {
            
        }

        private void OutMaterialSlot_OnMaterialDetachedHandler()
        {
            CalculateRecipe();
        }

        private void RecipeController_OnRecipeChangeHandler(MaterialEnum recipe)
        {
            _currentRecipe = recipe;
            CalculateRecipe();
            OnRecipeChange?.Invoke(recipe);
        }
        
        private void PrintController_OnPrintEnd()
        {
            OnPrintEnd?.Invoke();

            if (_slotsToUse != null)
            {
                foreach (var slot in _slotsToUse)
                {
                    slot.RemoveMaterial();
                }
            }
        }

        private void PrintController_OnPrintHandler()
        {
            OnPrint?.Invoke();
        }

        #endregion
    }
    
    public class PrintController
    {
        private MaterialSlot _outMaterialSlot;
        private MaterialManager _manager;
        private RecipeData _recipeToPrint;
        private float _printTime;
        private float _timer;

        public UnityAction OnPrinting;
        public UnityAction OnPrintEnd;

        public PrintController(MaterialSlot outMaterialSlot, MaterialManager manager)
        {
            _outMaterialSlot = outMaterialSlot;
            _manager = manager;
        }

        public void Initialize()
        {
            
        }

        public void Execute()
        {
            if (_timer > 0)
            {
                _timer -= Time.deltaTime;
                
                if (_timer <= 0)
                {
                    OnPrintEnd?.Invoke();
                }
            }
        }

        public void StartPrint(RecipeData recipeData, MaterialSlot[] materialSlots, float printTime = 5)
        {
            _recipeToPrint = recipeData;
            _printTime = printTime;
            _timer = _printTime;

            if (materialSlots != null)
            {
                foreach (var slot in materialSlots)
                {
                    var shrinkData = new ScaleData(0f, 1f, _printTime);
                    slot.ChangeMaterialScale(shrinkData);
                }
            }

            CreateOutMaterial();
            
            OnPrinting?.Invoke();
        }

        private void CreateOutMaterial()
        {
            var expandData = new ScaleData(1f, 0f, _printTime);
            var newMaterial = _manager.GetItem(_recipeToPrint.MaterialGotten);
            _outMaterialSlot.AttachMaterial(newMaterial);
            _outMaterialSlot.SetMaterialScale(0f);
            _outMaterialSlot.ChangeMaterialScale(expandData);
        }
    }

    public class PrinterRecipeController
    {
        public MaterialEnum[] AvailableRecipes { get; private set; }

        private bool _hasRecipes => AvailableRecipes.Length > 0;
        private int _recipeCount => AvailableRecipes.Length;
        private float _cycleTimer;
        private readonly float _cycleDelay = 0.75f;
        private int _currentIndex;
        
        public UnityAction<MaterialEnum> OnRecipeChange;

        public PrinterRecipeController(MaterialEnum[] availableRecipes)
        {
            AvailableRecipes = availableRecipes;
        }

        public void Initialize()
        {
            _currentIndex = 0;
            if (AvailableRecipes.Length > 0)
            {
                OnRecipeChange?.Invoke(AvailableRecipes[0]);
            }
        }

        public void Execute()
        {
            if(_cycleTimer > 0)
            {
                _cycleTimer -= Time.deltaTime;
            }
        }

        public void CycleRecipes(bool isForward)
        {
            if(_hasRecipes == false) return;
            if(_cycleTimer > 0) return;

            _currentIndex = isForward ? _currentIndex + 1 : _currentIndex - 1;

            if (_currentIndex >= _recipeCount)
            {
                _currentIndex = 0;
            }
            else if (_currentIndex <= -1)
            {
                _currentIndex = _recipeCount - 1;
            }
            
            OnRecipeChange?.Invoke(AvailableRecipes[_currentIndex]);

            _cycleTimer = _cycleDelay;
        }

        public Tuple<bool, MaterialSlot[]> CalculateRecipe(MaterialSlot[] materialSlots, RecipeData recipeData)
        {
            var ingredientsCount = recipeData.MaterialNeededCount;

            //Skips if it is an item dispenser
            if (ingredientsCount == 1 && recipeData.MaterialNeeded[0] == MaterialEnum.None)
            {
                return new Tuple<bool, MaterialSlot[]>(true, null);
            }

            var ingredientsCheck = 0;
            MaterialSlot[] slotsToUse = new MaterialSlot[ingredientsCount];
            bool canPrint = false;
            
            for (int i = 0; i < ingredientsCount; i++)
            {
                for (int j = 0; j < materialSlots.Length; j++)
                {
                    if (recipeData.MaterialNeeded[i] == materialSlots[j].GetAttachedMaterial())
                    {
                        ingredientsCheck++;
                        slotsToUse[i] = materialSlots[j];
                    }
                }
            }

            if (ingredientsCheck == ingredientsCount)
            {
                canPrint = true;
            }
                
            return new Tuple<bool, MaterialSlot[]>(canPrint, slotsToUse);
        }
    }
}
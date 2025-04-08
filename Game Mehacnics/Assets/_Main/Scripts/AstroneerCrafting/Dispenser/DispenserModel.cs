using System;
using _Main.Scripts.AstroneerCrafting.Managers;
using _Main.Scripts.AstroneerCrafting.Materials;
using UnityEngine;
using UnityEngine.Events;

namespace _Main.Scripts.AstroneerCrafting.Dispenser
{
    [RequireComponent(typeof(DispenserView))]
    public class DispenserModel : MonoBehaviour, IInteractable
    {
        [SerializeField] private MaterialSlot materialSlot;
        [SerializeField] private MaterialEnum materialEnum;
        
        private MaterialManager _materialManager;
        private readonly float _dispenseDelay = 1;
        private float _dispenseDelayTimer;
        private bool _canDispense;
        
        public Transform SelfTransform => transform;
        public MaterialEnum MaterialEnum => materialEnum;
        public bool IsQuick { get; private set; } = false;

        public event Action<bool> OnInteract;

        public UnityAction OnPrinting;
        public UnityAction OnPrinted;
        public UnityAction OnReadyToPrint;

        private void Start()
        {
            materialSlot.OnMaterialDetached += OnMaterialAttachedHandler;
            _materialManager = GameManager.Instance.MaterialManager;
            _canDispense = true;
        }

        private void Update()
        {
            if (_dispenseDelayTimer > 0)
            {
                _dispenseDelayTimer -= Time.deltaTime;
                if (_dispenseDelayTimer <= 0)
                {
                    DispenseItem();
                    _canDispense = true;
                    OnPrinted?.Invoke();
                }
            }
        }

        private void DispenseItem()
        {
            var newMaterial = _materialManager.GetItem(materialEnum);
            materialSlot.AttachMaterial(newMaterial);
        }

        private void ResetTimer()
        {
            _dispenseDelayTimer = _dispenseDelay;
        }

        public void Interact()
        {
            if (materialSlot == null)
            {
                Debug.LogWarning("Material Slot not assigned");
                return;
            }

            if (materialEnum == MaterialEnum.None)
            {
                Debug.LogWarning("No material selected");
                return;
            }

            if(_canDispense == false) return;

            _canDispense = false;
            
            ResetTimer();
            
            OnPrinting?.Invoke();
        }
        
        private void OnMaterialAttachedHandler()
        {
            OnReadyToPrint?.Invoke();
        }
    }
}
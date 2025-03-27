using System;
using _Main.Scripts.AstroneerCrafting.Managers;
using _Main.Scripts.AstroneerCrafting.Materials;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace _Main.Scripts.AstroneerCrafting.Printer
{
    public class PrinterSlot : MonoBehaviour
    {
        [SerializeField] private MaterialObject materialAttached;
        [SerializeField] private Transform attachPoint;
        
        public bool _canAttach = true;
        private float _canAttachTimer;
        private float _canAttachDelay = 0.5f;

        public bool HasMaterialAttached { get; private set; }

        public UnityAction OnMaterialAttached;
        public UnityAction OnMaterialDetached;

        private void Update()
        {
            if (_canAttachTimer > 0)
            {
                _canAttachTimer -= Time.deltaTime;
                if (_canAttachTimer <= 0)
                {
                    _canAttach = true;
                }
            }
        }

        private void ResetCanAttachTimer()
        {
            _canAttachTimer = _canAttachDelay;
        }

        public MaterialEnum GetAttachedMaterial()
        {
            return materialAttached != null ? materialAttached.MaterialType : MaterialEnum.None;
        }

        public void AttachMaterial(MaterialObject materialObject)
        {
            materialObject.AttachToSlot(attachPoint);
            materialAttached = materialObject;
            HasMaterialAttached = true;
            _canAttach = false;
            OnMaterialAttached?.Invoke();
        }

        public void DetachMaterial()
        {
            if(materialAttached == null) return;
            
            materialAttached.DetachFromSlot();
            materialAttached = null;
            HasMaterialAttached = false;
            ResetCanAttachTimer();
            OnMaterialDetached?.Invoke();
        }

        public void ChangeMaterialScale(ScaleData scaleData)
        {
            materialAttached.ChangeScale(scaleData);
        }

        public void SetMaterialScale(float newScale)
        {
            materialAttached.SetScale(newScale);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if(HasMaterialAttached || !_canAttach) return;
            
            if (other.gameObject.TryGetComponent<MaterialObject>(out var materialObject))
            {
                AttachMaterial(materialObject);
            }
        }
    }
}
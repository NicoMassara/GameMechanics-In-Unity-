using _Main.Scripts.AstroneerCrafting.Materials;
using UnityEngine;
using UnityEngine.Events;

namespace _Main.Scripts.AstroneerCrafting.Printer
{
    public class MaterialSlot : MonoBehaviour
    {
        [SerializeField] private MaterialObject materialAttached;
        [SerializeField] private Transform attachPoint;
        [SerializeField] private bool isAttachable;
        
        private bool _canAttach = true;
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
        
        protected void ResetCanAttachTimer()
        {
            _canAttachTimer = _canAttachDelay;
        }
        
        public MaterialEnum GetAttachedMaterial()
        {
            return materialAttached != null ? materialAttached.MaterialType : MaterialEnum.None;
        }
        
        public void DetachMaterial()
        {
            if(!HasMaterialAttached) return;
            
            materialAttached.DetachFromSlot();
            ResetAttachValues();
        }
        
        public void RemoveMaterial()
        {
            if(!HasMaterialAttached) return;
            
            materialAttached.Disable();
            ResetAttachValues();
        }
        
        public void ChangeMaterialScale(ScaleData scaleData)
        {
            materialAttached.ChangeScale(scaleData);
        }

        public void SetMaterialScale(float newScale)
        {
            materialAttached.SetScale(newScale);
        }

        protected void ResetAttachValues()
        {
            materialAttached = null;
            HasMaterialAttached = false;
            ResetCanAttachTimer();
            OnMaterialDetached?.Invoke();
        }

        public void AttachMaterial(MaterialObject materialObject)
        {
            if (materialObject != null)
            {
                materialAttached = materialObject;
                materialAttached.AttachToSlot(attachPoint);
                HasMaterialAttached = true;
                _canAttach = false;
                OnMaterialAttached?.Invoke();
            }
            else
            {
                Debug.Log("Material attached is null");
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if(!isAttachable) return;
            if(HasMaterialAttached || !_canAttach) return;
            
            if (other.gameObject.TryGetComponent<MaterialObject>(out var materialObject))
            {
                AttachMaterial(materialObject);
            }
        }
    }
}
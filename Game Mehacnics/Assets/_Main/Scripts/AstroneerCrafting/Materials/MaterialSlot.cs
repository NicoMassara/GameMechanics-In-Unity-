using UnityEngine;
using UnityEngine.Events;

namespace _Main.Scripts.AstroneerCrafting.Materials
{
    public class MaterialSlot : MonoBehaviour
    {
        [SerializeField] private MaterialObject materialAttached;
        [SerializeField] private Transform attachPoint;
        
        public bool _canAttach = true;
        private float _canAttachTimer;
        private float _canAttachDelay = 0.5f;
        
        protected MaterialObject MaterialAttached => materialAttached;
        
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
            return MaterialAttached != null ? MaterialAttached.MaterialType : MaterialEnum.None;
        }
        
        public void DetachMaterial()
        {
            if(!HasMaterialAttached) return;
            
            materialAttached.DetachFromSlot();
            ResetAttachValues();
        }
        
        public void ChangeMaterialScale(ScaleData scaleData)
        {
            MaterialAttached.ChangeScale(scaleData);
        }

        public void SetMaterialScale(float newScale)
        {
            MaterialAttached.SetScale(newScale);
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
            HasMaterialAttached = materialObject != null;
            
            if (HasMaterialAttached)
            {
                materialObject.AttachToSlot(attachPoint);
                materialAttached = materialObject;
                _canAttach = false;
                OnMaterialAttached?.Invoke();
            }
            else
            {
                Debug.Log("No Material Attached To Slot");
            }
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
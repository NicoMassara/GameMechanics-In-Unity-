using System;
using _Main.Scripts.AstroneerCrafting.FloatingHand;
using _Main.Scripts.AstroneerCrafting.Managers;
using UnityEngine;
using UnityEngine.Events;
using _Main.Custom.Pool;

namespace _Main.Scripts.AstroneerCrafting.Materials
{
    public class MaterialObject : MonoBehaviour, IHandable, IPoolable<MaterialObject>
    {
        [SerializeField] private MaterialEnum materialType;
        [SerializeField] private MeshRenderer meshRenderer;
        
        private Rigidbody _rigidbody;
        public Transform SelfTransform => transform;
        public MaterialEnum MaterialType => materialType;
        private readonly ScaleModifier _scaleModifier = new ScaleModifier();
        public bool IsAttached { get; private set; }
        
        public bool CanBeGrabbed { get; private set; }
        public event Action<IHandable> OnForcedRelease;
        public event UnityAction<MaterialObject> OnRecycle;
        
        public UnityAction OnDetachedFromSlot;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            SetMaterial(materialType);
        }

        private void Update()
        {
            if (_scaleModifier != null && _scaleModifier.IsChanging)
            {
                _scaleModifier.ChangeScale();
                transform.localScale = _scaleModifier.GetNewScale();
            }
        }

        public void SetMaterial(MaterialEnum type)
        {
            if (type != MaterialEnum.None)
            {
                var materialData = GameManager.Instance.MaterialManager.GetMaterialDataByEnum(type);
                UpdateMaterialColor(materialData.SelfColor, type); 
            }
        }

        public void UpdateMaterialColor(Color color, MaterialEnum typeEnum)
        {
            materialType = typeEnum;
            meshRenderer.material.color = color;
        }

        public void AttachToSlot(Transform slotTransform)
        {
            TriggerRelease();
            
            _rigidbody.isKinematic = true;
            transform.SetParent(slotTransform);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            
            IsAttached = true;
        }

        public void DetachFromSlot()
        {
            _rigidbody.isKinematic = false;
            transform.parent = null;
            IsAttached = false;
            OnDetachedFromSlot?.Invoke();
        }

        public void ChangeScale(ScaleData newScaleData)
        {
            if (_scaleModifier != null)
            {
                _scaleModifier.SetScaleData(newScaleData);
            }
        }

        public void SetScale(float newScale)
        {
            transform.localScale = newScale * Vector3.one;
        }

        public void SetCanBeGrabbed(bool canBeGrabbed)
        {
            CanBeGrabbed = canBeGrabbed;
        }

        public void Recycle()
        {
            IsAttached = false;
            transform.SetParent(null);
            OnRecycle?.Invoke(this);
            gameObject.SetActive(false);
        }   

        public void Reset()
        {
            gameObject.SetActive(true);
            _rigidbody.isKinematic = false;
        }
        
        public void Grab(Transform newParent)
        {
            if (IsAttached)
            {
               DetachFromSlot();
            }
            
            _rigidbody.isKinematic = true;
            transform.SetParent(newParent);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }

        public void UnGrab()
        {
            transform.parent = null;
            _rigidbody.isKinematic = false;
        }
        

        public void TriggerRelease()
        {
            OnForcedRelease?.Invoke(this);
        }
    }
}
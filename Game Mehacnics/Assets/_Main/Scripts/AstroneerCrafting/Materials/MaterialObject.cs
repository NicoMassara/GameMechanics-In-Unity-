using System;
using _Main.Scripts.AstroneerCrafting.Managers;
using _Tools.SingleHandledPool;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

namespace _Main.Scripts.AstroneerCrafting.Materials
{
    public class MaterialObject : MonoBehaviour, IPoolable<MaterialObject>
    {
        [SerializeField] private MaterialEnum materialType;
        [SerializeField] private MeshRenderer meshRenderer;
        
        private Rigidbody _rigidbody;

        public MaterialEnum MaterialType { get; private set; }

        public bool IsAttached { get; private set; }
        
        private readonly ScaleModifier _scaleModifier = new ScaleModifier();
        
        public event Action<MaterialObject> OnDisable;

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
                UpdateMaterialColor(materialData.SelfColor); 
            }
        }

        public void UpdateMaterialColor(Color color)
        {
            meshRenderer.material.color = color;
        }

        public void AttachToSlot(Transform slotTransform)
        {
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
        
        public void Reset()
        {
            _rigidbody.isKinematic = false;
            transform.parent = null;
            IsAttached = false;
            gameObject.SetActive(false);
        }   

        public void Enable()
        {
            gameObject.SetActive(true);
        }
    }
}
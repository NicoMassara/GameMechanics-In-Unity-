using System;
using _Main.Scripts.AstroneerCrafting.Managers;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

namespace _Main.Scripts.AstroneerCrafting.Materials
{
    public class MaterialObject : MonoBehaviour
    {
        [SerializeField] private MaterialEnum materialType;
        [SerializeField] private MeshRenderer meshRenderer;
        
        private Rigidbody _rigidbody;

        public MaterialEnum MaterialType { get; private set; }

        public bool IsAttached { get; private set; }
        
        private readonly ScaleModifier _scaleModifier = new ScaleModifier();

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
                MaterialType = type;
                UpdateMaterialColor(); 
            }
        }

        private void UpdateMaterialColor()
        {
            if (MaterialType != MaterialEnum.None)
            {
                var materialData = GameManager.Instance.MaterialManager.GetMaterialDataByEnum(MaterialType);
                meshRenderer.material.color = materialData.SelfColor;
            }
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
    }
}
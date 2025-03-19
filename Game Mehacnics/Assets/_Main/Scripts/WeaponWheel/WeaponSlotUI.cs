using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _Main.Scripts.WeaponWheel
{
    public class WeaponSlotUI : MonoBehaviour
    {
        [SerializeField] private WeaponStorage storage;
        [SerializeField] private TMP_Text slotText;
        [Range(0,7)]
        [SerializeField] private int slotIndex;

        private Image _slotImage;
        private Color _currentColor;
        private Color _selectedColor = Color.black;
        private Color _defaultColor;
        
        private bool _isSelected;

        private void Awake()
        {
            storage.OnWeaponChange += OnWeaponChangeHandler;
            _slotImage = GetComponent<Image>();
            _defaultColor = _slotImage.color;
            _currentColor = _defaultColor;
        }

        public void Initialize()
        {
            var slot = storage.GetSlotByIndex(slotIndex);
            SetSlotText(slot.SelfName, slot.SelfColor);
        }

        public void SetHighlight(bool bIsHighlighted)
        {
            if (_isSelected)
            {
                return;
            }

            _slotImage.color = bIsHighlighted ? Color.white : _currentColor;
        }

        private void SetSlotText(string weaponName, Color slotColor)
        {
            slotText.text = weaponName;
            slotText.color = slotColor;
        }

        public void SetSelected(bool bIsSelected)
        {
            _isSelected = bIsSelected;
            _slotImage.color = _isSelected ? _selectedColor : _defaultColor;
            
        }

        private void OnWeaponChangeHandler(string weaponName, Color slotColor)
        {

        }

        public int GetWeaponIndex()
        {
            return slotIndex;
        }
    }
}
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _Main.Scripts.WeaponWheel
{
    public class WeaponSlotUI : MonoBehaviour
    {
        [Header("UI")] 
        [SerializeField] private Image backgroundColor;
        [SerializeField] private Image selectedIndicator;
        [SerializeField] private TMP_Text slotText;

        [Header("Values")] 
        [SerializeField] private WeaponStorage storage;
        [SerializeField] private Color selectedColor;
        [SerializeField] private Color highlightColor;
        [Range(0,7)]
        [SerializeField] private int slotIndex;
        
        private Color _defaultColor;
        
        private bool _isSelected;

        private void Awake()
        {
            _defaultColor = backgroundColor.color;
            selectedIndicator.color = Color.clear;
        }

        public void Initialize()
        {
            var slot = storage.GetSlotByIndex(slotIndex);
            var weaponSlot = slot.SelectCurrentWeapon();
            SetSlotText(weaponSlot.SelfName, weaponSlot.SelfColor);
            slot.OnWeaponChange += OnWeaponChangeHandler;
            slot.OnSlotHighlighted += OnSlotHighlightedHandler;
            slot.OnSlotSelected += OnSlotSelectedHandler;
        }

        private void OnSlotSelectedHandler(bool isSelected)
        {
            SetSelected(isSelected);
        }

        private void OnSlotHighlightedHandler(bool isHighlighted)
        {
            SetHighlight(isHighlighted);
        }

        private void OnWeaponChangeHandler(string weaponName, Color weaponColor)
        {
            SetSlotText(weaponName, weaponColor);
        }

        private void SetHighlight(bool bIsHighlighted)
        {
            backgroundColor.color = bIsHighlighted ? highlightColor : _defaultColor;
        }

        private void SetSlotText(string weaponName, Color slotColor)
        {
            slotText.text = weaponName;
            slotText.color = slotColor;
        }

        private void SetSelected(bool bIsSelected)
        {
            _isSelected = bIsSelected;
            selectedIndicator.color = _isSelected ?  selectedColor : Color.clear;
        }

        public int GetWeaponIndex()
        {
            return slotIndex;
        }
    }
}
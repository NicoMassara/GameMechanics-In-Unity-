using System;
using TMPro;
using UnityEngine;

namespace _Main.Scripts.WeaponWheel
{
    public class WeaponWheelUI : MonoBehaviour
    {
        [SerializeField] private WeaponStorage storage;
        [SerializeField] private TMP_Text currentWeaponText;
        [SerializeField] private RectTransform centerPosition;
        [SerializeField] private TMP_Text angleText;
        
        [SerializeField] private WeaponSlotUI[] slots;
        

        private const int AngleStartOffset = 22;
        private const int SlotAngleSize = 45;
        private int _slotDebugDistance = 350;
        private int _lastIndex;
        private float _lastAngle;
        private int _indexSlot;
        private WeaponSlotUI _currentSlotSelected;
        private WeaponSlotUI _currentHightlightedSlot;
        private float _checkAngleDelay = 0.1f;
        private float _currentCheckDelay;
        
        public Vector3 CenterPosition => centerPosition.position;

        private void Awake()
        {
            storage.OnWeaponChange += OnWeaponChangeHandler;
        }

        public void Initialize()
        {
            foreach (var slot in slots)
            {
                slot.Initialize();
            }

            // Forcing the Upper Middle Slot to be selected
            _indexSlot = 1;
            SelectSlot();
        }

        public void ClearCheckDelay()
        {
            _currentCheckDelay = 0;
        }

        public void CalculateAngle(Vector2 direction)
        {
            if (_currentCheckDelay > 0)
            {
                _currentCheckDelay -= Time.deltaTime;
                return;
            }

            if (direction == Vector2.zero)
            {
                StopHighlightSlot();
                return;
            }

            float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) + (180 - AngleStartOffset);
            if (angle < 0)
            {
                angle += 360;
            }
            
            if (Math.Abs(_lastAngle - angle) > 1)
            {
                angleText.text = $"Mouse Angle: {(int)angle}";
            
                _lastAngle = angle;
                HighlightSlot();
            }
            
            _currentCheckDelay = _checkAngleDelay;
        }

        private void HighlightSlot()
        {
            _indexSlot = Mathf.FloorToInt(_lastAngle / SlotAngleSize);
            var slot = slots[_indexSlot];
            slot.SetHighlight(true);

            if (_currentHightlightedSlot != slot)
            {
                StopHighlightSlot();    
                _currentHightlightedSlot = slot;
            }
        }

        private void StopHighlightSlot()
        {
            if (_currentHightlightedSlot != null)
            {
                _currentHightlightedSlot.SetHighlight(false);
                _currentHightlightedSlot = null;
            }
        }

        public void SelectSlot()
        {
            if(_currentHightlightedSlot == null) return;
            
            if(_currentSlotSelected != null)
            {
                _currentSlotSelected.SetSelected(false);
            }

            _currentSlotSelected = _currentHightlightedSlot;
            _currentHightlightedSlot = null;
            
            _currentSlotSelected.SetSelected(true);
            
            storage.SelectWeaponByIndex(_currentSlotSelected.GetWeaponIndex());
        }

        private void SetWeaponText(string weaponName,Color weaponColor)
        {
            currentWeaponText.text = weaponName;
            currentWeaponText.color = weaponColor;
        }

        private void OnWeaponChangeHandler(string weaponName, Color weaponColor)
        {
            SetWeaponText(weaponName,weaponColor);
        }
    }
}
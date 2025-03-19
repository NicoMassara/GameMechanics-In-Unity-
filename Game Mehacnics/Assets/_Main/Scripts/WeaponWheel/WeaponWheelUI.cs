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
        private float _checkAngleDelay = 0.1f;
        private float _currentCheckDelay;

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

        private void Update()
        {
            CalculateMouseAngle();
            HighlightSlot();
        }

        private void CalculateMouseAngle()
        {
            var mousePosition = Input.mousePosition;
            var origin = centerPosition.position;
            
            if (_currentCheckDelay <= 0)
            {
                var direction = origin - mousePosition;
            
                float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) + (180 - AngleStartOffset);
                if (angle < 0)
                {
                    angle += 360;
                }
            
                if (Math.Abs(_lastAngle - angle) > 1)
                {
                    angleText.text = $"Mouse Angle: {(int)angle}";
            
                    _lastAngle = angle;
                }
            
                _currentCheckDelay = _checkAngleDelay;
            }
            
            _currentCheckDelay -= Time.deltaTime;
            
            Debug.DrawLine(origin, mousePosition, Color.red);

            for (int i = 0; i < 8; i++)
            {
                float baseAngle = AngleStartOffset + (i * SlotAngleSize);
                float angleRad = baseAngle * Mathf.Deg2Rad;
                Vector2 angleVector = new Vector2(
                    origin.x + Mathf.Cos(angleRad) * _slotDebugDistance, 
                    origin.y + Mathf.Sin(angleRad) * _slotDebugDistance
                );
                Debug.DrawLine(origin, angleVector, i > 0 ? Color.green : Color.magenta);
            }
        }

        private void HighlightSlot()
        {
            _indexSlot = Mathf.FloorToInt(_lastAngle / SlotAngleSize);
            slots[_indexSlot].SetHighlight(true);

            if (_lastIndex != _indexSlot)
            {
                slots[_lastIndex].SetHighlight(false);
                _lastIndex = _indexSlot;
            }
        }

        public void SelectSlot()
        {
            if(_currentSlotSelected != null)
            {
                _currentSlotSelected.SetSelected(false);
            }

            _currentSlotSelected = slots[_indexSlot];
            
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
using System;
using UnityEngine;
using UnityEngine.Events;

namespace _Main.Scripts.WeaponWheel
{
    public class WeaponStorage : MonoBehaviour
    {
        private WeaponData _currentWeapon;
        private WeaponSlot[] _slots;
        private WeaponSlot _currentSlot;
        
        //Slot Values
        private float _checkAngleTimer;
        private float _checkAngleDelay = 0.1f;
        private const int AngleStartOffset = 22;
        private const int SlotAngleSize = 45;
        private float _lastAngle;
        private WeaponSlot _currentHighlightedSlot;

        public UnityAction<string, Color> OnSlotChange;
        public UnityAction<WeaponData> OnWeaponChange;

        private void Awake()
        {
            SetSlotsData();
        }

        private void Start()
        {
            
        }

        private void Update()
        {
            if (_checkAngleTimer > 0)
            {
                _checkAngleTimer -= Time.deltaTime;
            }
        }

        #region Weapon 
        
        public void SelectSlotWeapon()
        {
            if (_currentSlot == null) return;

            _currentWeapon = _currentSlot.SelectCurrentWeapon();
            OnWeaponChange?.Invoke(_currentWeapon);
        }

        #endregion

        #region Slot
        
        public void ChangeIndexInSlot(bool doesIncrease)
        {
            if (_currentHighlightedSlot == null) return;

            if (doesIncrease)
            {
                _currentHighlightedSlot.IncreaseIndex();
            }
            else
            {
                _currentHighlightedSlot.DecreaseIndex();
            }
        }
        
        private void HighlightSlot()
        {
            var indexSlot = Mathf.FloorToInt(_lastAngle / SlotAngleSize);
            var slot = _slots[indexSlot];
            slot.SetHighlighted(true);

            if (_currentHighlightedSlot != slot)
            {
                StopHighlightSlot();    
                _currentHighlightedSlot = slot;
            }
        }
        
        private void StopHighlightSlot()
        {
            if (_currentHighlightedSlot != null)
            {
                _currentHighlightedSlot.SetHighlighted(false);
                _currentHighlightedSlot = null;
            }
        }

        #endregion

        #region Data Getters
        public WeaponData GetCurrentWeapon()
        {
            return _currentWeapon;
        }

        public WeaponSlot GetCurrentSlot()
        {
            return _currentSlot;
        }
        
        public WeaponSlot GetSlotByIndex(int index)
        {
            index = Mathf.Clamp(index, 0, _slots.Length - 1);
            return _slots[index];
        }
        
        public WeaponData GetWeaponBySlotIndex(int index)
        {
            index = Mathf.Clamp(index, 0, _slots.Length - 1);
            return _slots[index].SelectCurrentWeapon();
        }

        #endregion

        #region Inputs

        public void CalculateAngle(Vector2 direction)
        {
            if(_checkAngleTimer > 0) return;

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
                _lastAngle = angle;
                HighlightSlot();
            }
            
            _checkAngleTimer = _checkAngleDelay;
        }

        public void SelectHighlightedSlot()
        {
            if (_currentHighlightedSlot == null) return;
            
            if (_currentHighlightedSlot == _currentSlot)
            {
                if (_currentSlot.GetCurrentWeapon() == _currentWeapon)
                {
                    return;
                }
            }
            else if (_currentSlot != null)
            {
                //
                _currentSlot.ClearSelectedWeapon();
                _currentSlot.SetSelected(false);
                _currentSlot.SetHighlighted(false);
                _currentSlot = null;
            }
            
            _currentSlot = _currentHighlightedSlot;
            _currentSlot.SetSelected(true);
            SelectSlotWeapon();
        }

        #endregion


        private void SetSlotsData()
        {
            var pistols = new WeaponData[]
            {
                new WeaponData("Beretta", 11),
                new WeaponData("Glock", 12),
                new WeaponData("Dessert", 8),
            };

            var smg = new WeaponData[]
            {
                new WeaponData("MP5", 30),
                new WeaponData("UZI", 30),
                new WeaponData("Vector", 30),
            };

            var rifle = new WeaponData[]
            {
                new WeaponData("AK-47", 20),
                new WeaponData("M4", 20),
                new WeaponData("Famas", 20),
            };

            var sniper = new WeaponData[]
            {
                new WeaponData("Cal. 50", 5),
                new WeaponData("Scar-H", 5),
                new WeaponData("Teco", 5),
            };

            var fist = new WeaponData[]
            {
                new WeaponData("Fist", -1),
                new WeaponData("Knife", -1),
                new WeaponData("Nuckles", -1),
            };

            var shotgun = new WeaponData[]
            {
                new WeaponData("Spas-20", 8),
                new WeaponData("D.Barrel", 8),
                new WeaponData("Bullpup", 8),
            };
            var rpg = new WeaponData[]
            {
                new WeaponData("RPG", 1),
                new WeaponData("Homing", 1),
                new WeaponData("Grenade L.", 5),
            };
            var explosives = new WeaponData[]
            {
                new WeaponData("Grenade", 5),
                new WeaponData("C4", 5),
                new WeaponData("Molotov", 5),
            };

            _slots = new WeaponSlot[]
            {
                new WeaponSlot("SMG", smg),
                new WeaponSlot("Pistols", pistols),
                new WeaponSlot("Explosives", explosives),
                new WeaponSlot("RPG", rpg),
                new WeaponSlot("Shotgun", shotgun),
                new WeaponSlot("Fist", fist),
                new WeaponSlot("Sniper", sniper),
                new WeaponSlot("Rifle", rifle)
            };
        }
    }

    public class WeaponSlot
    {
        public string SlotName = "Slot";
        public WeaponData[] WeaponsStored;
        
        public bool HasWeapons => WeaponsStored.Length > 0;
        public int WeaponCount => WeaponsStored.Length - 1;

        public int CurrentIndex { get; private set; }
        
        public bool IsSelected { get; private set; }
        public bool IsHighlighted { get; private set; }

        public WeaponData SelectedWeapon;
        
        public UnityAction<string, Color> OnWeaponChange;
        public UnityAction<bool> OnSlotSelected;
        public UnityAction<bool> OnSlotHighlighted;

        public WeaponSlot(string slotName, WeaponData[] weaponsStored)
        {
            SlotName = slotName;
            WeaponsStored = weaponsStored;
            CurrentIndex = 0;
        }

        public WeaponData SelectCurrentWeapon()
        {
            var weapon = WeaponsStored[CurrentIndex];
            SelectedWeapon = weapon;
            return weapon;
        }

        public WeaponData GetCurrentWeapon()
        {
            return WeaponsStored[CurrentIndex];
        }

        public void ClearSelectedWeapon()
        {
            SelectedWeapon = null;
        }

        private void UpdateWeaponData()
        {
            var weapon = GetCurrentWeapon();
            
            SetSelected(weapon == SelectedWeapon);
            OnWeaponChange.Invoke(weapon.SelfName, weapon.SelfColor);
        }

        public void IncreaseIndex()
        {
            CurrentIndex++;
            if (CurrentIndex >= WeaponsStored.Length)
            {
                CurrentIndex = 0;
            }
            
            UpdateWeaponData();
        }

        public void DecreaseIndex()
        {
            CurrentIndex--;
            if (CurrentIndex <= -1)
            {
                CurrentIndex = WeaponCount;
            }
            
            
            UpdateWeaponData();
        }

        public void SetSelected(bool isSelected)
        {
            IsSelected = isSelected;
            OnSlotSelected?.Invoke(isSelected);
        }

        public void SetHighlighted(bool isHighlighted)
        {
            IsHighlighted = isHighlighted;
            OnSlotHighlighted?.Invoke(isHighlighted);
        }
    }

    public class WeaponData
    {
        public string SelfName;
        public int CurrentAmmo;
        public Color SelfColor;

        public WeaponData(string selfName, int currentAmmo)
        {
            SelfName = selfName;
            CurrentAmmo = currentAmmo;
            SelfColor = Color.white;
        }
    }
}
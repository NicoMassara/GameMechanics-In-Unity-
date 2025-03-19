using System;
using UnityEngine;
using UnityEngine.Events;

namespace _Main.Scripts.WeaponWheel
{
    public class WeaponStorage : MonoBehaviour
    {
        private WeaponData[] _weaponSlots;
        private WeaponData _currentWeapon;

        public UnityAction<string, Color> OnWeaponChange;

        private void Awake()
        {
            _weaponSlots = new WeaponData[]
            {
                new WeaponData("Pistol", 15, Color.red),
                new WeaponData("SMG", 15, Color.yellow),
                new WeaponData("Rifle", 15, Color.green),
                new WeaponData("Sniper", 15, Color.cyan),
                new WeaponData("Fist", 15, Color.white),
                new WeaponData("Shotgun", 15, Color.magenta),
                new WeaponData("RPG", 15, new Color(1, .5f, 0)),
                new WeaponData("C4", 15, new Color(.25f, 0, 1))
            };
        }

        public void Initialize()
        {
            SelectWeaponByIndex(0);
        }

        public WeaponData GetSlotByIndex(int index)
        {
            index = Mathf.Clamp(index, 0, _weaponSlots.Length - 1);
            return _weaponSlots[index];
        }

        public void SelectWeaponByIndex(int index)
        {
            var selectedSlot = _weaponSlots[index];
            _currentWeapon = selectedSlot;
            OnWeaponChange?.Invoke(_currentWeapon.SelfName, _currentWeapon.SelfColor);
        }

        public WeaponData GetCurrentWeapon()
        {
            return _currentWeapon;
        }
    }

    public class WeaponData
    {
        public string SelfName;
        public int CurrentAmmo;
        public Color SelfColor;

        public WeaponData(string selfName, int currentAmmo, Color selfColor)
        {
            SelfName = selfName;
            CurrentAmmo = currentAmmo;
            SelfColor = selfColor;
        }
    }
}
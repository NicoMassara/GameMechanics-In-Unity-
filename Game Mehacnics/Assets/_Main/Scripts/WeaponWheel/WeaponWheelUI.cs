using System;
using TMPro;
using UnityEngine;

namespace _Main.Scripts.WeaponWheel
{
    public class WeaponWheelUI : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private TMP_Text currentWeaponText;
        [SerializeField] private TMP_Text currentAmmoText;
        [SerializeField] private RectTransform centerPosition;
        [SerializeField] private WeaponSlotUI[] slots;
        [Header("Values")]
        [SerializeField] private WeaponStorage storage;

        private void Start()
        {
            foreach (var slot in slots)
            {
                slot.Initialize();
            }

            currentWeaponText.text = "---";
            currentAmmoText.text = "---";
            storage.OnWeaponChange += OnWeaponChangeHandler;
        }

        private void OnWeaponChangeHandler(WeaponData data)
        {
            currentWeaponText.text = data.SelfName;
            var ammo = data.CurrentAmmo;
            currentAmmoText.text = ammo == -1 ? " " : ammo.ToString();
        }
    }
}   
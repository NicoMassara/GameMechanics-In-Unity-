using System;
using UnityEngine;

namespace _Main.Scripts.WeaponWheel
{
    public class WeaponWheelDataControl : MonoBehaviour
    {
        [SerializeField] private WeaponStorage storage;
        [SerializeField] private WeaponWheelUI ui;

        private void Start()
        {
            ui.Initialize();
            storage.Initialize();
        }
    }
}
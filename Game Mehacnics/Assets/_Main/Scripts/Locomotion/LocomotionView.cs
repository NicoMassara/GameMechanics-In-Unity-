using System;
using UnityEngine;

namespace _Main.Scripts.Locomotion
{
    public class LocomotionView : MonoBehaviour
    {
        [SerializeField] private LocomotionMotor locomotionMotor;
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            _animator.SetFloat("Velocity", locomotionMotor.Velocity);
        }
    }
}
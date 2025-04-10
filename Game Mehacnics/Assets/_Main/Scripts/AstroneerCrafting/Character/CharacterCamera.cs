using System;
using _Main.Scripts.AstroneerCrafting.Managers;
using _Main.Scripts.Locomotion;
using UnityEngine;

namespace _Main.Scripts.AstroneerCrafting.Character
{
    public class CharacterCamera : MonoBehaviour
    {
        [SerializeField] private LocomotionMotor characterMotor;
        [SerializeField] private Vector3 viewOffset;
        [Range(1, 15)]
        [SerializeField] private float viewSpeed;
        [SerializeField] private Vector2 maxViewAngle;
        
        private Vector2 _lookDirection;
        private float _finalViewSpeed;
        private Transform _characterTransform;
        public Camera SelfCamera { get; private set; }

        private void Start()
        {
            characterMotor.SetCamera(transform);
            _characterTransform = characterMotor.transform;
            
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            
            var angles = transform.eulerAngles;
            _lookDirection.x = angles.y;
            _lookDirection.y = angles.x;
            
            _finalViewSpeed = viewSpeed * 10;
            
            SelfCamera = GetComponent<Camera>();
            
            GameManager.Instance.CameraManager.SetActiveCamera(SelfCamera);
        }

        public void HandleMovement(Vector2 viewDirection)
        {
            if(_characterTransform == null) return;
            
            _lookDirection.x += viewDirection.x * _finalViewSpeed * Time.deltaTime;
            _lookDirection.y -= viewDirection.y * _finalViewSpeed * Time.deltaTime;
            _lookDirection.y = Mathf.Clamp(_lookDirection.y, maxViewAngle.x, maxViewAngle.y);
            
            Quaternion rotation = Quaternion.Euler(_lookDirection.y, _lookDirection.x, 0f);
            Vector3 offset = rotation * viewOffset;
            transform.position = _characterTransform.position + offset;
            
            transform.LookAt(_characterTransform);
        }
    }
        
}
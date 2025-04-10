using System;
using _Main.Scripts.AstroneerCrafting.Managers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace _Main.Scripts.AstroneerCrafting.FloatingHand
{
    public class CharacterHand : MonoBehaviour
    {
        [SerializeField] private Transform handSlot;
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private float checkRadius = 3f;
        [Range(0,10)]
        [SerializeField] private float offsetSpeed = 2;
        [SerializeField] private Vector2 maxOffset = new Vector2(1, 8);
        private Collider[] _colliders;
        private float _cameraOffset;
        private bool _isEnable;
        private bool _hasGrabbed;
        private bool _hasHitObject;
        private IHandable _itemGrabbed;
        private Vector3 _mousePosInWorld;
        public UnityAction<bool> OnHandEnable;

        private void Start()
        {
            _isEnable = false;
            _hasGrabbed = false;
            
            _colliders = new Collider[10];
            _cameraOffset = 5;
        }

        public void ToggleHand()
        {
            _isEnable = !_isEnable;
            OnHandEnable?.Invoke(_isEnable);
            GameManager.Instance.SetEnableMouse(_isEnable);

            if(!_isEnable && _hasGrabbed)
            {
                ReleaseItem();
            }
        }

        public void MoveHand(float input)
        {
            _cameraOffset += (input * Time.deltaTime) * offsetSpeed;
            
            _cameraOffset = Mathf.Clamp(_cameraOffset, maxOffset.x, maxOffset.y);
        }

        public void TryGrab()
        {
            if (_hasGrabbed)
            {
                if (_itemGrabbed != null)
                {
                    ReleaseItem();
                }
            }
            else if(_hasHitObject)
            {
                int hitCount = Physics.OverlapSphereNonAlloc(_mousePosInWorld, checkRadius, _colliders, layerMask);
                
                for (int i = 0; i < hitCount; i++)
                {
                    if (_colliders[i].TryGetComponent(out IHandable handable))
                    {
                        if (handable.CanBeGrabbed)
                        {
                            AttachItemToHand(handable);
                            break;
                        }
                    }
                }
            }
        }

        private void ReleaseItem()
        {
            _itemGrabbed.UnGrab();
            _hasGrabbed = false;
            _itemGrabbed.OnForcedRelease -= OnForcedReleaseHandler;
        }

        private void AttachItemToHand(IHandable item)
        {
            _cameraOffset = maxOffset.y / 2;
            _hasGrabbed = true;
            _itemGrabbed = item;
            _itemGrabbed.Grab(handSlot);
            _itemGrabbed.OnForcedRelease += OnForcedReleaseHandler;
        }

        public void CalculateMouseInWorld(Vector2 mousePosition)
        {
            if (_hasGrabbed)
            {
                Vector3 temp = new Vector3(mousePosition.x, mousePosition.y, _cameraOffset);
                _mousePosInWorld = GameManager.Instance.CameraManager.GetActiveCamera().ScreenToWorldPoint(temp);
                handSlot.position = _mousePosInWorld;
            }
            else
            {
                var ray = GameManager.Instance.CameraManager.GetActiveCamera().ScreenPointToRay(mousePosition);
                
                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask))
                {
                    _hasHitObject = true;
                    _mousePosInWorld = hit.point;
                }
                else
                {
                    _hasHitObject = false;
                }
            }
        }
        
        private void OnForcedReleaseHandler(IHandable handable)
        {
            handable.UnGrab();
            handable.OnForcedRelease -= OnForcedReleaseHandler;
            _itemGrabbed = null;
            _hasGrabbed = false;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(_mousePosInWorld, checkRadius);
        }
    }
}
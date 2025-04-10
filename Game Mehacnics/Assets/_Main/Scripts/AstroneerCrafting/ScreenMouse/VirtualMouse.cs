using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Main.Scripts.AstroneerCrafting.ScreenMouse
{
    public class VirtualMouse : MonoBehaviour
    {
        [Range(0,10)]
        [SerializeField] private float sensitivity;
        [Space] 
        [SerializeField] private GameObject mousePointer;
        
        private int _windowWidth;
        private int _windowHeight;
        private bool _isEnable;
        public Vector2 ScreenPosition { get; private set; }

        private void Start()
        {
            _windowWidth = Screen.width;
            _windowHeight = Screen.height;
        }

        public void Move(Vector2 delta)
        {
            var deltaX = ScreenPosition.x + (delta.x * sensitivity);
            var deltaY = ScreenPosition.y + (delta.y * sensitivity);
            
            deltaX = Mathf.Clamp(deltaX, -_windowWidth/2, _windowWidth/2);
            deltaY = Mathf.Clamp(deltaY, -_windowHeight/2, _windowHeight/2);
            
            ScreenPosition = new Vector2(deltaX, deltaY);
            mousePointer.GetComponent<RectTransform>().anchoredPosition = ScreenPosition;
        }

        public void SetEnable(bool isEnable)
        {
            _isEnable = isEnable;
            mousePointer.SetActive(_isEnable);

            if (_isEnable)
            {
                ScreenPosition = Vector2.zero;
            }
        }

        public Vector2 GetOffsetScreenPosition()
        {
            var tempX = ScreenPosition.x + _windowWidth/2;
            var tempY = ScreenPosition.y + _windowHeight/2;
            
            return new Vector2(tempX, tempY);
        }
    }
}
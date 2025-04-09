using UnityEngine;

namespace _Main.Scripts.AstroneerCrafting.Managers
{
    public class CameraManager
    {
        private Camera _currentCamera;

        public Camera GetActiveCamera()
        {
            return _currentCamera;
        }

        public void SetActiveCamera(Camera newActiveCamera)
        {
            if (_currentCamera != null)
            {
                _currentCamera.gameObject.SetActive(false);
            }

            _currentCamera = newActiveCamera;
            _currentCamera.gameObject.SetActive(true);
        }
    }
}
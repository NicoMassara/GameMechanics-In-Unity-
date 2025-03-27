using UnityEngine;

namespace _Main.Scripts.AstroneerCrafting.Materials
{
    public class ScaleModifier
    {
        private Vector3 _currentScale;
        private Vector3 _targetScale;
        private Vector3 _startScale;
        private float _timeElapsed;
        private float _duration;

        public bool IsChanging { get; private set; }

        public void SetScaleData(ScaleData data)
        {
            _startScale = data.StartScale;
            _targetScale = data.TargetScale;
            _duration = data.Duration;
            _timeElapsed = 0;
            IsChanging = true;
        }

        public Vector3 GetNewScale()
        {
            return _currentScale;
        }

        public void ChangeScale()
        {
            if (_timeElapsed < _duration)
            {
                _timeElapsed += Time.deltaTime;
                float lerpFactor = _timeElapsed / _duration;
                _currentScale = Vector3.Lerp(_startScale, _targetScale, lerpFactor);

                if (_currentScale == _targetScale)
                {
                    IsChanging = false;
                }
            }
        }
    }

    public class ScaleData
    {
        public Vector3 TargetScale;
        public Vector3 StartScale;
        public float Duration;

        public ScaleData(float targetScale, float startScale, float duration)
        {
            TargetScale = targetScale * Vector3.one;
            StartScale = startScale * Vector3.one;
            Duration = duration;
        }
    }
}
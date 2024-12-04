using DG.Tweening;
using UnityEngine;

namespace Screens.Application.Views
{
    public class LoaderAnimation : MonoBehaviour
    {
        [SerializeField] private float _rotationAngle = 360f;
        [SerializeField] private float _duration = 2f;
        [SerializeField] private GameObject _loaderAnimationObject;
        
        private Tween _rotationTween;

        public void StartAnimation()
        {
            _loaderAnimationObject.SetActive(true);
            _rotationTween = _loaderAnimationObject.transform.DORotate(new Vector3(0, 0, _rotationAngle), _duration, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart);
        }

        public void StopAnimation()
        {
            if (_rotationTween != null)
            {
                _rotationTween.Kill();
            }
            
            _loaderAnimationObject.SetActive(false);
        }
    }
}
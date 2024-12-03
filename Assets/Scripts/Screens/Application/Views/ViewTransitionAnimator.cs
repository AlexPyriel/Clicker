using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Screens.Application.Views
{
    public class ViewTransitionAnimator : MonoBehaviour
    {
        private const float TargetAlpha = 1f;
        private const float InitialAlpha = 0f;
        
        [SerializeField] private Image _fadeImage;
        [SerializeField, Range(0, 1)] private float _fadeDuration = 0.25f;

        public void Animate(Action onFadeIn)
        {
            Sequence sequence = DOTween.Sequence();
            
            sequence.Append(_fadeImage.DOFade(TargetAlpha, _fadeDuration))
                .AppendCallback(() => onFadeIn?.Invoke())
                .Append(_fadeImage.DOFade(InitialAlpha, _fadeDuration));
        }
    }
}
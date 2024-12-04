using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Screens.Clicker.Views
{
    public class RewardAnimationView : MonoBehaviour
    {
        [SerializeField] private Image _iconImage;
        [SerializeField] private RectTransform _target;
        [SerializeField] private RectTransform _container;

        [SerializeField] private float _yOffset = 1.0f;
        [SerializeField] private float _randOffset = 1.0f;
        [SerializeField] private float _speed = 1.0f;
        [SerializeField] private float _minScale = 0.1f;
        [SerializeField] private float _randDelay = 0.44f;
        [SerializeField] private Vector3 _startScale = new Vector3(0.75f, 0.75f, 0.75f);

        public static RewardAnimationView Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        public void Play(Sprite iconSprite, Vector3 pos)
        {
            float halfSpeed = _speed * 0.5f;
            Image newIcon = Instantiate(_iconImage, _container);
            RectTransform newRect = newIcon.GetComponent<RectTransform>();
            newIcon.sprite = iconSprite;
            newRect.position = pos;

            Sequence sequence = DOTween.Sequence();

            Vector3 randOffset = Random.insideUnitCircle * _randOffset;
            randOffset.y += _yOffset;
            newRect.localScale = _startScale;
            newRect.gameObject.SetActive(true);

            sequence
                .Append(newRect.DOScale(1.0f, halfSpeed))
                .Join(newRect.DOMove(newRect.position + randOffset, halfSpeed)).SetEase(Ease.InSine)
                .AppendInterval(Random.value * _randDelay)
                .Append(newRect.DOMove(_target.position, _speed).SetEase(Ease.InSine))
                .Append(newRect.DOScale(1.1f, halfSpeed)).SetEase(Ease.InSine)
                .Append(newRect.DOScale(_minScale, _speed))
                .OnComplete(() =>
                {
                    sequence.Kill();

                    if (newRect != null)
                    {
                        Destroy(newRect.gameObject);
                    }
                });
        }
    }
}
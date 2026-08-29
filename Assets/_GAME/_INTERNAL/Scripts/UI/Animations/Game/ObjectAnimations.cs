using DG.Tweening;
using System;
using UnityEngine;

namespace UI.Animations.Game
{
    public abstract class ObjectAnimations : MonoBehaviour
    {
        [Header("Animations Duration Setup")]
        [SerializeField] private float _appearAnimationDuration = 0.35f;
        [SerializeField] private float _hoverAnimationDuration = 0.5f;
        [SerializeField] private float _pulseAnimationDuration = 1f;
        [SerializeField] private float _collaplseAnimationDuration = 0.55f;
        [SerializeField] private float _shakeScaleAnimationDuration = 0.15f;

        [Space(5), Header("Hover Animation Setup")]
        [SerializeField] private float _yMoveOffset = 1.0f;
        [SerializeField] private LoopType _hoverLoopType = LoopType.Yoyo;
        [SerializeField, Tooltip("Set -1 for infinite loops count")] private int _hoverLoopCount = -1;

        [Space(5), Header("Collapse Animation Setup")]
        [SerializeField] private float _shakeScaleStrength = 0.08f;
        [SerializeField] private Ease _collapseInEase = Ease.OutQuad;
        [SerializeField] private Ease _collapseOutEase = Ease.InBack;

        [Space(5), Header("Pulse Animation Setup")]
        [SerializeField] private float _pulseTargetScale = 0.9f;
        [SerializeField] private LoopType _pulseLoopType = LoopType.Yoyo;
        [SerializeField, Tooltip("Set -1 for infinite loops count")] private int _pulseLoopsCount = -1;

        [Space(5), Header("Animations Flags")]
        [SerializeField] private bool _hoverAnimationEnabled = false;
        [SerializeField] private bool _pulseAnimationEnabled = false;

        protected Vector3 _originalScale;

        private Tween _appearTween;
        private Tween _disappearTween;
        private Tween _hoverTween;
        private Tween _pulseTween;
        private Tween _shakeScaleTween;
        private Sequence _collapseSequence;

        private void OnEnable()
        {
            if (_hoverAnimationEnabled)
                HoverAnimation();

            if (_pulseAnimationEnabled)
                PulseAnimation();
        }

        private void OnDisable()
        {
            _hoverTween?.Kill();
            _appearTween?.Kill();
            _disappearTween?.Kill();
            _pulseTween?.Kill();
        }

        private void OnDestroy()
        {
            _hoverTween?.Kill();
            _appearTween?.Kill();
            _disappearTween?.Kill();
            _pulseTween?.Kill();
        }

        private void HoverAnimation()
        {
            _hoverTween?.Kill();

            Vector3 originalPosition = transform.localPosition;
            float targetY = transform.localPosition.y + _yMoveOffset;

            _hoverTween = transform
                .DOLocalMoveY(targetY, _hoverAnimationDuration)
                .SetEase(Ease.InOutSine)
                .SetLoops(_hoverLoopCount, _hoverLoopType)
                .OnKill(() => transform.localPosition = originalPosition);
        }

        private void PulseAnimation()
        {
            _pulseTween?.Kill();

            _originalScale = transform.localScale;

            _pulseTween = transform
                .DOScale(_pulseTargetScale, _pulseAnimationDuration)
                .SetEase(Ease.InOutSine)
                .SetLoops(_pulseLoopsCount, _pulseLoopType);
        }

        public void Appear(Vector3 originalScale, Action onComplete = null)
        {
            transform.localScale = Vector3.zero;
            gameObject.SetActive(true);
            _originalScale = originalScale;

            _appearTween?.Kill();
            _disappearTween?.Kill();

            _appearTween = transform
                .DOScale(originalScale, _appearAnimationDuration)
                .SetEase(Ease.InOutBounce)
                .OnComplete(() =>
                {
                    transform.localScale = _originalScale;
                    onComplete?.Invoke();
                });
        }

        public void Disappear(Action onComplete = null)
        {
            _appearTween?.Kill();
            _disappearTween?.Kill();

            float disappearAnimationDuration = _appearAnimationDuration * 0.25f;

            _disappearTween = transform
                .DOScale(Vector3.zero, disappearAnimationDuration)
                .SetEase(Ease.InOutBounce)
                .OnComplete(() =>
                {
                    transform.localScale = Vector3.zero;
                    onComplete?.Invoke();
                })
                .OnKill(() => transform.localScale = _originalScale);
        }

        public void CollapseAnimation(Action onComplete = null)
        {
            _collapseSequence?.Kill();
            _shakeScaleTween?.Kill();

            transform.localScale = _originalScale;

            _collapseSequence = DOTween.Sequence();

            float squashDuration = _collaplseAnimationDuration * 0.25f;
            float collapseDuration = _collaplseAnimationDuration * 0.75f;

            _shakeScaleTween = transform.DOShakeScale(_shakeScaleStrength, _shakeScaleAnimationDuration);

            _collapseSequence
                .Append(transform.DOScale(_originalScale * 1.1f, squashDuration))
                .SetEase(_collapseInEase);

            _collapseSequence
                .Append(transform.DOScale(Vector3.zero, squashDuration))
                .SetEase(_collapseOutEase);

            _collapseSequence.OnComplete(() =>
            {
                gameObject.SetActive(false);
                transform.localScale = _originalScale;
                onComplete?.Invoke();
            });
        }
    }
}
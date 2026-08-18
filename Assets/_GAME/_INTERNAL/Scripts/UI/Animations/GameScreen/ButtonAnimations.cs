using DG.Tweening;
using System;
using UnityEngine;

namespace UI.Animations.GameScreen
{
    [Serializable]
    public class ButtonAnimations
    {
        [Header("Click Animations Setup")]
        [SerializeField] private float _clickAnimationDuration = 0.25f;
        [SerializeField] private Vector2 _clickedScale = Vector2.one;

        [Space(5), Header("Hover Animations Setup")]
        [SerializeField] private bool _usingHoverAnimation = false;
        [SerializeField] private float _hoverAnimationDuration = 1.25f;
        [SerializeField] private Vector2 _hoverOffset = Vector2.zero;
        [SerializeField] private LoopType _hoverLoopType = LoopType.Yoyo;
        [SerializeField] private Ease _hoverEase = Ease.InOutSine;
        [SerializeField, Tooltip("Use -1 for cycling animation")] private int _hoverLoopsCount = -1;

        [Space(5), Header("Pulse Animation Setup")]
        [SerializeField] private bool _usingPulseAnimation = false;
        [SerializeField] private float _pulseAnimationDuration = 1.0f;
        [SerializeField] private Vector2 _pulseScaleFactor = Vector2.one;
        [SerializeField] private LoopType _pulseLoopType = LoopType.Yoyo;
        [SerializeField] private Ease _pulseEase = Ease.InOutSine;
        [SerializeField, Tooltip("Use -1 for cycling animation")] private int _pulseLoopsCount = -1;

        private RectTransform _rectTransform;

        private Vector2 _originalScale;
        private Vector2 _pulseTargetScale;

        private Tween _pulseTween;
        private Tween _hoverTween;

        private Sequence _clickSequence;

        public bool Initialized => _rectTransform != null;

        public void Init(RectTransform target)
        {
            _rectTransform = target;
            _originalScale = _rectTransform.localScale;

            _pulseTargetScale = _originalScale * _pulseScaleFactor;

            if (_usingPulseAnimation)
                PulseAnimation(_pulseLoopsCount, _pulseLoopType);

            if (_usingHoverAnimation)
                HoverAnimation();
        }

        public void StopAnimations() => _rectTransform.DOKill();

        public void HoverAnimation()
        {
            _hoverTween?.Kill();

            _hoverTween = _rectTransform
                .DOAnchorPos(_hoverOffset, _hoverAnimationDuration)
                .SetEase(_hoverEase)
                .SetLoops(_hoverLoopsCount, _hoverLoopType);
        }

        public void PulseAnimation(int loops = -1, LoopType loopType = LoopType.Yoyo)
        {
            _pulseTween?.Kill();

            _pulseTween = _rectTransform
                .DOScale(_pulseTargetScale, _pulseAnimationDuration)
                .SetEase(_pulseEase)
                .SetLoops(loops, loopType);
        }

        public void ButtonClickAnimation(Action onComplete = null)
        {
            _clickSequence?.Kill();

            _clickSequence = DOTween.Sequence();

            _clickSequence.Append(
                _rectTransform
                .DOScale(_clickedScale, _clickAnimationDuration)
                .SetEase(Ease.OutSine));

            _clickSequence.Append(_rectTransform
                .DOScale(Vector2.one, _clickAnimationDuration)
                .SetEase(Ease.InSine));

            _clickSequence.OnComplete(() => onComplete?.Invoke());
        }

        public Tween GetWaveTween(Vector2 targetScale, float duration)
        {
            _pulseTween?.Pause();
            _hoverTween?.Pause();

            _rectTransform.localScale = _originalScale;

            return _rectTransform
                .DOScale(targetScale, duration)
                .SetEase(Ease.InOutSine)
                .SetLoops(2, LoopType.Yoyo)
                .OnComplete(() =>
                {
                    _pulseTween?.Play();
                    _hoverTween?.Play();
                });
        }
    }
}
using Screen = UI.Other.Screen;
using UnityEngine;
using UI.Other;
using System;
using DG.Tweening;

namespace UI.Screens
{
    public class GameOverScreen : Screen
    {
        [Header("Refs")]
        [SerializeField] private RectTransform _panelRect;

        [Space(5), Header("Animation Setup")]
        [SerializeField] private float _animationDuration = 0.55f;
        [SerializeField] private float _targetY;

        [Space(5), Header("Buttons")]
        [SerializeField] private ActionButton _replayButton;
        [SerializeField] private ActionButton _homeButton;

        private Tween _openTween;

        public event Action OnHomeButtonClick;
        public event Action OnReplayButtonClick;

        private void Awake()
        {
            _replayButton.OnButtonClick += HandleReplayButtonClick;
            _homeButton.OnButtonClick += HandleHomeButtonClick;
        }

        private void OnDestroy()
        {
            _replayButton.OnButtonClick -= HandleReplayButtonClick;
            _homeButton.OnButtonClick -= HandleHomeButtonClick;
        }

        public override void Open()
        {
            base.Open();

            _openTween?.Kill();

            _openTween = _panelRect
                .DOAnchorPosY(_targetY, _animationDuration)
                .SetEase(Ease.InOutBack)
                .OnComplete(() => _panelRect.anchoredPosition = new(_panelRect.anchoredPosition.x, _targetY));
        }

        private void HandleReplayButtonClick()
        {
            OnReplayButtonClick?.Invoke();
        }

        private void HandleHomeButtonClick()
        {
            OnHomeButtonClick?.Invoke();
        }
    }
}
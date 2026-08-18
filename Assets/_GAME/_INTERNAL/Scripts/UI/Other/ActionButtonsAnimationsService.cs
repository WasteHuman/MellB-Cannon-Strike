using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Other
{
    public class ActionButtonsAnimationsService : MonoBehaviour
    {
        [Header("Buttons references")]
        [SerializeField] private List<ActionButton> _buttonsToWaveAnimation = new();

        [Space(5), Header("Wave Setup")]
        [SerializeField] private Vector2 _waveTargetScale;
        [SerializeField] private float _waveDelay = 0.1f;
        [SerializeField] private float _waveDuration = 0.5f;
        [SerializeField] private float _delayBetweenWaves = 1.25f;
        [SerializeField] private LoopType _waveLoopType = LoopType.Yoyo;
        [SerializeField, Tooltip("Use -1 for cycling animation")] private int _waveLoopsCount = -1;

        private Sequence _waveSequence;

        private void OnDestroy() => StopWaveAnimation();

        [ContextMenu("Force Start Wave Animation")]
        public void StartWaveAnimation()
        {
            _waveSequence?.Kill();

            _waveSequence = DOTween.Sequence();

            for (int i = 0; i < _buttonsToWaveAnimation.Count; i++)
            {
                ActionButton button = _buttonsToWaveAnimation[i];

                if (button == null || button.Animations == null)
                    continue;

                Tween buttonWave = button.Animations.GetWaveTween(_waveTargetScale, _waveDuration);

                _waveSequence.Insert(i * _waveDelay, buttonWave);
            }

            float lastButtonEndTime = (_buttonsToWaveAnimation.Count - 1) * _waveDelay + _waveDuration;

            float intervalDuration = lastButtonEndTime + _delayBetweenWaves + _waveSequence.Duration();
            _waveSequence.AppendInterval(intervalDuration);

            _waveSequence.SetLoops(_waveLoopsCount, _waveLoopType);
        }

        [ContextMenu("Force Stop Wave Animation")]
        public void ForceStopAnimationWithComplete()
        {
            StopWaveAnimation(true);
        }

        public void StopWaveAnimation(bool complete = false)
        {
            _waveSequence?.Kill(complete);
            _waveSequence = null;
        }
    }
}
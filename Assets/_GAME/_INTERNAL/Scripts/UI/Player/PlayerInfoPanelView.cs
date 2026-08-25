using Core.Services;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace UI.Player
{
    public class PlayerInfoPanelView : MonoBehaviour
    {
        [Header("Lables Setup")]
        [SerializeField] private TextMeshProUGUI _playerBalanceLabel;

        [Space(5), Header("Animation Durations Setup")]
        [SerializeField] private float _coinsBalanceChangedAnimationDuration = 0.5f;

        private float _displayedCoinsBalance;

        public void Init()
        {
            GameServices.EconomyService.OnCoinsBalanceChanged += HandleChangedCoinsBalance;
            _displayedCoinsBalance = GameServices.EconomyService.GetCoinsBalance();

            GameServices.EconomyService.RequestCoinsBalance();
        }

        public void Dispose() => GameServices.EconomyService.OnCoinsBalanceChanged -= HandleChangedCoinsBalance;

        private void HandleChangedCoinsBalance(float amount)
        {
            float oldValue = _displayedCoinsBalance;
            _displayedCoinsBalance = amount;

            DOVirtual.Float(oldValue, _displayedCoinsBalance, _coinsBalanceChangedAnimationDuration, value =>
            {
                _playerBalanceLabel.text = $"{value:N0}";
            });
        }
    }
}
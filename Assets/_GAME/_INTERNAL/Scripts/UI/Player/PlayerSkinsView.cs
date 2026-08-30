using Core.Services;
using Core.SO;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Player
{
    public class PlayerSkinsView : MonoBehaviour
    {
        [SerializeField] private Image _playerSkin;
        [SerializeField] private Image _playerBallSkin;

        private ShopEntitiesConfig _spriteConfig;

        void OnDestroy()
        {
            Dispose();
        }

        public void Init()
        {
            _spriteConfig = Resources.Load<ShopEntitiesConfig>("Configs/Shop/ShopEntitiesConfig");

            GameServices.PlayerService.OnPlayerSkinChanged += HandlePlayerSkinChanged;
            GameServices.PlayerService.OnPlayerBallSkinChanged += HandlePlayerBallSkinChanged;

            _playerSkin.sprite = _spriteConfig.GetPlayerSkinById(GameServices.PlayerService.CurrentPlayerSkinId);
            _playerBallSkin.sprite = _spriteConfig.GetPlayerBallSkinById(GameServices.PlayerService.CurrentPlayerBallSkinId);
        }

        public void Dispose()
        {
            GameServices.PlayerService.OnPlayerSkinChanged -= HandlePlayerSkinChanged;
            GameServices.PlayerService.OnPlayerBallSkinChanged -= HandlePlayerBallSkinChanged;
        }

        private void HandlePlayerSkinChanged(string id)
        {
            _playerSkin.sprite = _spriteConfig.GetPlayerSkinById(GameServices.PlayerService.CurrentPlayerSkinId);
            _playerBallSkin.sprite = _spriteConfig.GetPlayerBallSkinById(GameServices.PlayerService.CurrentPlayerBallSkinId);
        }

        private void HandlePlayerBallSkinChanged(string id)
        {
            _playerSkin.sprite = _spriteConfig.GetPlayerSkinById(GameServices.PlayerService.CurrentPlayerSkinId);
            _playerBallSkin.sprite = _spriteConfig.GetPlayerBallSkinById(GameServices.PlayerService.CurrentPlayerBallSkinId);
        }
    }
}
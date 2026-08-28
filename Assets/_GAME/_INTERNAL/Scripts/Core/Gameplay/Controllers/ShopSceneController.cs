using Core.Common;
using UI.Player;
using UnityEngine;

namespace Core.Gameplay.Controllers
{
    public class ShopSceneController : SceneController
    {
        [SerializeField] private PlayerInfoPanelView _playerInfoView;

        public override void Enter() {}
        
        public override void Initialize()
        {
            _playerInfoView.Init();
        }

        public override void Exit()
        {
            _playerInfoView.Dispose();
        }
    }
}
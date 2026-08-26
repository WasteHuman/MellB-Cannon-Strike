using Core.Common;
using Core.Gameplay.Game.Player;
using UI.Player;
using UnityEngine;

namespace Core.Gameplay.Controllers
{
    public class GameSceneController : SceneController
    {
        [SerializeField] private PlayerInfoPanelView _playerInfoPanelView;
        [SerializeField] private PlayerController _playerController;

        public override void Enter() { }

        public override void Initialize()
        {
            _playerInfoPanelView.Init();
            _playerController.Initialize();
        }

        public override void Exit()
        {
            _playerController.Dispose();
            _playerInfoPanelView.Dispose();
        }
    }
}
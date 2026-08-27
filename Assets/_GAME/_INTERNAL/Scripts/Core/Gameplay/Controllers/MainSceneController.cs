using Core.Common;
using UI.Player;
using UnityEngine;

namespace Core.Gameplay.Controllers
{
    public class MainSceneController : SceneController
    {
        [SerializeField] private PlayerInfoPanelView _playerInfoPanelView;

        public override void Enter() {}
        
        public override void Initialize()
        {
            _playerInfoPanelView.Init();
        }

        public override void Exit() {}
    }
}
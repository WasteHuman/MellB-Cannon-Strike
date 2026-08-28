using Core.Common;
using Core.Gameplay.Game.Player;
using Core.Gameplay.Game.TargetSystem;
using Core.UI.Controllers;
using UI.Player;
using UnityEngine;

namespace Core.Gameplay.Controllers
{
    public class GameSceneController : SceneController
    {
        [Header("Controllers (View & Gmae)")]
        [SerializeField] private PlayerInfoPanelView _playerInfoPanelView;
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private TargetSystemController _targetSystemController;

        [Space(5), Header("Screens")]
        [SerializeField] private GameSceneScreenController _screenController;

        public override void Enter()
        {
            _playerController.OnPlayerLose += HandlePlayerLose;
        }

        public override void Initialize()
        {
            _playerInfoPanelView.Init();
            _playerController.Initialize();
            _targetSystemController.Initialize();
        }

        public override void Exit()
        {
            _playerController.OnPlayerLose -= HandlePlayerLose;

            _playerController.Dispose();
            _playerInfoPanelView.Dispose();
        }

        private void HandlePlayerLose()
        {
            _targetSystemController.FreezeAllTargets();
            _screenController.OpenGameOverScreen();
        }
    }
}
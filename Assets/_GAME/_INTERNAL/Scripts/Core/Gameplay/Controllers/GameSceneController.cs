using Core.Common;
using Core.Data;
using Core.Gameplay.Game.Player;
using Core.Gameplay.Game.TargetSystem;
using Core.Services;
using Core.Services.Analytics;
using Core.UI.Controllers;
using UI.Other;
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
        [SerializeField] private GameplayTutorialView _gameplayTutorialView;

        [Space(5), Header("Screens")]
        [SerializeField] private GameSceneScreenController _screenController;

        public override void Enter()
        {
            GameServices.PlayerService.ResetSessionScore();
            GameServices.PlayerService.ResetEarnedSessionCoins();

            _playerController.OnPlayerLose += HandlePlayerLose;
            _gameplayTutorialView.OnTutorialSkipped += HandleGameplayStarted;
        }

        public override void Initialize()
        {
            _gameplayTutorialView.Initialize();

            _playerInfoPanelView.Init();
            _playerController.Initialize();
        }

        public override void Exit()
        {
            _playerController.OnPlayerLose -= HandlePlayerLose;
            _gameplayTutorialView.OnTutorialSkipped -= HandleGameplayStarted;

            _playerController.Dispose();
            _playerInfoPanelView.Dispose();
        }

        private void HandlePlayerLose()
        {
            var earnedCoins = GameServices.PlayerService.SessionEarnedCoins;
            var sessionScore = GameServices.PlayerService.SessionPlayerScore;

            GameResult sessionResult = new(false, earnedCoins, sessionScore);
             
            _targetSystemController.FreezeAllTargets();
            _screenController.SetupGameOverScreen(sessionResult.Score, Mathf.RoundToInt(sessionResult.RewardCoins));
            _screenController.OpenGameOverScreen();

            GameServices.GameSessionService.HandleEndedGame(sessionResult);
            AnalyticsService.Instance.ReportGameLoss();
        }

        private void HandleGameplayStarted()
        {
            if (_gameplayTutorialView != null)
                _gameplayTutorialView.Hide();

            _targetSystemController.Initialize();
            _playerController.StartGameplay();
        }
    }
}
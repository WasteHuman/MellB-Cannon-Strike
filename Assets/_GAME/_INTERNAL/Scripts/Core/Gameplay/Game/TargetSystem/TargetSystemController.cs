using Core.Gameplay.Game.DifficultSystem;
using Core.Services;
using SO.Game;
using SO.Game.DifficultSystem;
using UnityEngine;

namespace Core.Gameplay.Game.TargetSystem
{
    public class TargetSystemController : MonoBehaviour
    {
        [Header("Configs")]
        [SerializeField] private TargetBallSpritesConfig _spritesConfig;
        [SerializeField] private DifficultSystemConfig _difficultConfig;

        [Space(5), Header("Spawn System")]
        [SerializeField] private TargetBallSpawner _spawner;

        private GameDifficultSystem _gameDifficultSystem;

        void OnDestroy()
        {
            _spawner.OnTargetDestroyed -= HandleDestroyedTarget;
        }

        public void Initialize()
        {
            _gameDifficultSystem = new(_difficultConfig);
            _spawner.Init(_spritesConfig, _gameDifficultSystem);

            _spawner.OnTargetDestroyed += HandleDestroyedTarget;
        }

        public void FreezeAllTargets() => _spawner.StopSpawnAndFreezeActiveTargets();

        private void HandleDestroyedTarget(int earnedCoins)
        {
            GameServices.PlayerService.IncreasePlayerSessionScore();
            GameServices.PlayerService.AddEarnedCoins(earnedCoins);
        }
    }
}
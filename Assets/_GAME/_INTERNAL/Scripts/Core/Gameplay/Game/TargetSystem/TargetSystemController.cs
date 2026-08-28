using Core.Gameplay.Game.DifficultSystem;
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

        public void Initialize()
        {
            _gameDifficultSystem = new(_difficultConfig);
            _spawner.Init(_spritesConfig, _gameDifficultSystem);
        }

        public void FreezeAllTargets() => _spawner.StopSpawnAndFreezeActiveTargets();
    }
}
using SO.Game.DifficultSystem;
using UnityEngine;

namespace Core.Gameplay.Game.DifficultSystem
{
    public class GameDifficultSystem
    {
        private readonly int _minTargetHp;
        private readonly int _maxTargetHp;
        private readonly int _maxDifficultyDestroyedTargets;
        private readonly int _randomHpRange;

        private int _currentTargetsHp;
        private int _currentDestroyedTargets = 0;

        private readonly AnimationCurve _difficultCurve;

        public GameDifficultSystem(DifficultSystemConfig config)
        {
            _difficultCurve = config.DifficultCurve;
            _currentTargetsHp = config.MinTargetsHp;
            _minTargetHp = config.MinTargetsHp;
            _maxTargetHp = config.MaxTargetsHp;
            _randomHpRange = config.HpRandomRangeOffset;
            _maxDifficultyDestroyedTargets = config.MaxDifficultyDestroyedTargets;
        }

        public void RecalculateDifficult()
        {
            _currentDestroyedTargets++;

            float progress = Mathf.Clamp01((float)_currentDestroyedTargets / _maxDifficultyDestroyedTargets);
            float difficulty = _difficultCurve.Evaluate(progress);

            int baseHp = Mathf.RoundToInt(Mathf.Lerp(_minTargetHp, _maxTargetHp, difficulty));
            int hp = baseHp + Random.Range(-_randomHpRange, _randomHpRange);
            _currentTargetsHp = Mathf.Clamp(hp, _minTargetHp, _maxTargetHp);
        }

        public int GetCurrentTargetsHP() => _currentTargetsHp; 
    }
}
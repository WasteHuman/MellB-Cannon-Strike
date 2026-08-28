using System;
using System.Collections.Generic;
using System.Threading;
using Core.Gameplay.Game.DifficultSystem;
using Cysharp.Threading.Tasks;
using SO.Game;
using UnityEngine;
using Utils;

namespace Core.Gameplay.Game.TargetSystem
{
    public class TargetBallSpawner : MonoBehaviour
    {
        [Header("Spawnpoints Setup")]
        [SerializeField] private Transform _leftSpawnpoint;
        [SerializeField] private Transform _rightSpawnpoint;

        [Space(5), Header("Target Balls Setup")]
        [SerializeField] private TargetBallView _targetBallViewPrefab;
        [SerializeField] private int _initialCount = 15;

        [Space(5), Header("Spawn Setup")]
        [SerializeField] private float _spawnDelay = 1f;
        [SerializeField] private int _maxSpawnedTargetsAtMoment = 5;

        private TargetBallSpritesConfig _spritesConfig;

        private GameDifficultSystem _difficultSystem;

        private bool _nextSpawnLeft = false;
        private bool _canSpawn = true;
        private int _currentSpawnedTargets = 0;

        private readonly List<TargetBallView> _targets = new();

        private ObjectPool<TargetBallView> _targetsPool;

        void OnDestroy()
        {
            DestroySubscribes();
        }

        public void Init(TargetBallSpritesConfig spritesConfig, GameDifficultSystem difficultSystem)
        {
            _spritesConfig = spritesConfig;
            _difficultSystem = difficultSystem;

            _targetsPool = new(_targetBallViewPrefab, _initialCount, transform);
            _canSpawn = true;
            InitSubscribes();
            AsyncTargetBallSpawn(this.GetCancellationTokenOnDestroy()).Forget();
        }

        public void StopSpawnAndFreezeActiveTargets()
        {
            _canSpawn = false;
            for(int i = 0; i < _targets.Count; i++)
                _targets[i].FreezeTarget();
        }

        private void InitSubscribes()
        {
            _targets.AddRange(_targetsPool.GetFreeElements());

            for (int i = 0; i < _targets.Count; i++)
                _targets[i].OnTargetDestroyed += HandleDestroyedTarget;
        }

        private void DestroySubscribes()
        {
            for (int i = 0; i < _targets.Count; i++)
                _targets[i].OnTargetDestroyed -= HandleDestroyedTarget;
        }

        private async UniTask AsyncTargetBallSpawn(CancellationToken token)
        {
            while (!token.IsCancellationRequested && _canSpawn)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(_spawnDelay));

                if(_currentSpawnedTargets >= _maxSpawnedTargetsAtMoment)
                    continue;

                var newTargetBall = _targetsPool.GetFreeElement();
                var spawnPosition = _nextSpawnLeft ? _leftSpawnpoint.position : _rightSpawnpoint.position;
                var impulseDirection = _nextSpawnLeft ? Vector2.right : Vector2.left;

                newTargetBall.Init(_difficultSystem.GetCurrentTargetsHP(), spawnPosition, impulseDirection);
                newTargetBall.SetSprite(_spritesConfig.GetRandomSprite());
                _currentSpawnedTargets++;
                _nextSpawnLeft = !_nextSpawnLeft;
            }
        }

        private void HandleDestroyedTarget(TargetBallView view)
        {
            _targetsPool.ReturnToPool(view);

            _currentSpawnedTargets = Mathf.Max(0, _currentSpawnedTargets - 1);
            _difficultSystem.RecalculateDifficult();
        }
    }
}
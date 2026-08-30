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
        [SerializeField] private float _splittedTargetSpawnOffestX = 0.5f;
        [SerializeField] private float _splittedTargetSpawnOffestY = 0.5f;

        private TargetBallSpritesConfig _spritesConfig;

        private GameDifficultSystem _difficultSystem;

        private bool _nextSpawnLeft = false;
        private bool _canSpawn = true;
        private int _currentSpawnedTargets = 0;

        private readonly List<TargetBallView> _targets = new();

        private ObjectPool<TargetBallView> _targetsPool;

        public event Action<int> OnTargetDestroyed;

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
            {
                _targets[i].OnTargetDestroyed += HandleDestroyedTarget;
                _targets[i].OnTargetSplitted += HandleSplittedTarget;
            }
        }

        private void DestroySubscribes()
        {
            for (int i = 0; i < _targets.Count; i++)
            {
                _targets[i].OnTargetDestroyed -= HandleDestroyedTarget;
                _targets[i].OnTargetSplitted -= HandleSplittedTarget;
            }
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
                newTargetBall.SetScale(_difficultSystem.GetCurrentScale());
                _currentSpawnedTargets++;
                _nextSpawnLeft = !_nextSpawnLeft;
            }
        }

        private void HandleDestroyedTarget(TargetBallView view)
        {
            _targetsPool.ReturnToPool(view);

            _currentSpawnedTargets = Mathf.Max(0, _currentSpawnedTargets - 1);
            _difficultSystem.RecalculateDifficult();

            OnTargetDestroyed?.Invoke(view.InitialHp);
        }

        private void HandleSplittedTarget(TargetBallView view)
        {
            _targetsPool.ReturnToPool(view);

            var leftBall = _targetsPool.GetFreeElement();
            var rightBall = _targetsPool.GetFreeElement();

            var leftImpulseDirection = Vector2.left;
            var rightImpulseDirection = Vector2.right;

            var leftBallPosition = new Vector2(view.transform.position.x + _splittedTargetSpawnOffestX, view.transform.position.y - _splittedTargetSpawnOffestY);
            var rightBallPosition = new Vector2(view.transform.position.x - _splittedTargetSpawnOffestX, view.transform.position.y + _splittedTargetSpawnOffestY);

            var splittedScale = view.OriginalScale * 0.75f;
            var leftSplittedHp = view.InitialHp / 2;
            var rightSplittedHp = view.InitialHp - leftSplittedHp;

            leftBall.Init(leftSplittedHp, leftBallPosition, leftImpulseDirection);
            leftBall.SetSprite(_spritesConfig.GetRandomSprite());
            leftBall.SetScale(splittedScale);
            leftBall.Appear(splittedScale);

            rightBall.Init(rightSplittedHp, rightBallPosition, rightImpulseDirection);
            rightBall.SetSprite(_spritesConfig.GetRandomSprite());
            rightBall.SetScale(splittedScale);
            rightBall.Appear(splittedScale);
        }
    }
}
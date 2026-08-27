using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UI.Other;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Gameplay.Game.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Move Buttons Setup")]
        [SerializeField] private ActionButton _goToLeftButton;
        [SerializeField] private ActionButton _goToRightButton;

        [Space(5), Header("Player Setup")]
        [SerializeField] private GameObject _player;
        [SerializeField] private SpriteRenderer _playerView;
        [SerializeField] private float _playerMoveSpeed = 5f;
        [SerializeField] private float _playerReloadTime = 2f;
        [SerializeField] private float _playerShootForce = 5f;

        [Tooltip("Using for setup delay between Player Pose sprites")]
        [SerializeField] private float _playerSpriteChangeDelay = 0.35f;

        [Space(5), Header("Player Pose Sprites Setup")]
        [SerializeField] private Sprite _idlePose;
        [SerializeField] private Sprite _prepareToHitPose;
        [SerializeField] private Sprite _hitPose;

        [Space(5), Header("Ball Projectile Setup")]
        [SerializeField] private PlayerProjectile _ballProjectile;
        [SerializeField] private Transform _ballProjectileContainer;

        private PlayerState _state = PlayerState.Idle;

        private UniTaskCompletionSource _hitProjectileSource;

        public void Initialize()
        {
            _goToLeftButton.IsUseHeldFunc = true;
            _goToRightButton.IsUseHeldFunc = true;

            _goToLeftButton.OnButtonClick += HandleLeftButtonClick;
            _goToRightButton.OnButtonClick += HandleRightButtonClick;

            _ballProjectile.Init(_ballProjectileContainer);
            _ballProjectile.OnBallHitted += HandleHittedBall;

            LoadCurrentPlayerSkin();

            ProjectileFlowAsync(this.GetCancellationTokenOnDestroy()).Forget();
        }

        public void Dispose()
        {
            _goToLeftButton.OnButtonClick -= HandleLeftButtonClick;
            _goToRightButton.OnButtonClick -= HandleRightButtonClick;
            _ballProjectile.OnBallHitted -= HandleHittedBall;
        }

        void Update()
        {
#if UNITY_EDITOR
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
                HandleLeftButtonClick();

            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
                HandleRightButtonClick();
#endif
        }

        private void LoadCurrentPlayerSkin()
        {
            
        }

        private async UniTask ProjectileFlowAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                if (_state != PlayerState.Idle)
                {
                    await UniTask.Yield(token);
                    continue;
                }

                await UniTask.Delay(TimeSpan.FromSeconds(0.5f), cancellationToken: token);

                await PlayerHitProcessAsync(token);
                
                _ballProjectile.ShootProjectile(_playerShootForce);

                await UniTask.Delay(TimeSpan.FromSeconds(_playerSpriteChangeDelay * 0.5f), cancellationToken: token);

                _playerView.sprite = _idlePose;

                await ProcessProjectileHitAsync(token);

                await ProjectileReloadAsync(token);
            }
        }

        private async UniTask ProjectileReloadAsync(CancellationToken token)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_playerReloadTime), cancellationToken: token);

            _state = PlayerState.Idle;

            _ballProjectile.Show();

            return;
        }

        private async UniTask PlayerHitProcessAsync(CancellationToken token)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_playerSpriteChangeDelay), cancellationToken: token);
            _playerView.sprite = _prepareToHitPose;
            await UniTask.Delay(TimeSpan.FromSeconds(_playerSpriteChangeDelay), cancellationToken: token);
            _playerView.sprite = _hitPose;

            return;
        }

        private async UniTask ProcessProjectileHitAsync(CancellationToken token)
        {
            _hitProjectileSource = new();
            await _hitProjectileSource.Task.AttachExternalCancellation(token);

            _ballProjectile.Hide();
            await UniTask.Delay(10, cancellationToken: token);

            _ballProjectile.ResetProjectilePosition();
            await UniTask.Delay(10, cancellationToken: token);

            _state = PlayerState.Reload;
            return;
        }

        private void HandleLeftButtonClick()
        {
            Vector3 movement = new(-1f, 0f, 0f);
            _player.transform.position += _playerMoveSpeed * Time.deltaTime * movement;
        }

        private void HandleRightButtonClick()
        {
            Vector3 movement = new(1f, 0f, 0f);
            _player.transform.position += _playerMoveSpeed * Time.deltaTime * movement;
        }

        private void HandleHittedBall() => _hitProjectileSource?.TrySetResult();
    }
}
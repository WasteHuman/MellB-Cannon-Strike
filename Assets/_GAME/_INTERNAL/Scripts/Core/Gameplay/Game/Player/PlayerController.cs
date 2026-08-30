using System;
using System.Collections.Generic;
using System.Threading;
using Core.Data;
using Core.Gameplay.Game.TargetSystem;
using Core.Services;
using Cysharp.Threading.Tasks;
using Extensions.GameObject;
using UI.Other;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Gameplay.Game.Player
{
    public class PlayerController : MonoBehaviour
    {
        private const float MIN_X = -3.2f;
        private const float MAX_X = 3.2f;

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
        [SerializeField] private List<PlayerSkinData> _playerSkinDatas = new();

        [Space(5), Header("Ball Projectile Setup")]
        [SerializeField] private PlayerProjectile _ballProjectile;
        [SerializeField] private Transform _ballProjectileContainer;
        [SerializeField] private List<PlayerBallSkinData> _playerBallSkinDatas = new();

        private PlayerState _state = PlayerState.Idle;
        private PlayerSkinData _playerSkinData;
        private Sprite _currentBallSkin;

        private UniTaskCompletionSource _hitProjectileSource;
        private bool _isPlayerAlive = true;

        public event Action OnPlayerLose;

        public void Initialize()
        {
            _goToLeftButton.IsUseHeldFunc = true;
            _goToRightButton.IsUseHeldFunc = true;

            _goToLeftButton.OnButtonClick += HandleLeftButtonClick;
            _goToRightButton.OnButtonClick += HandleRightButtonClick;

            LoadCurrentPlayerSkins(
                GameServices.PlayerService.CurrentPlayerSkinId, 
                GameServices.PlayerService.CurrentPlayerBallSkinId);

            _ballProjectile.Init(_ballProjectileContainer, _currentBallSkin, GameServices.PlayerService.CurrentPlayerDamage);
            _playerReloadTime = GameServices.PlayerService.CurrentPlayerReload;
            _ballProjectile.OnBallHitted += HandleHittedBall;

            _isPlayerAlive = true;

            ProjectileFlowAsync(this.GetCancellationTokenOnDestroy()).Forget();
        }

        public void Dispose()
        {
            _goToLeftButton.OnButtonClick -= HandleLeftButtonClick;
            _goToRightButton.OnButtonClick -= HandleRightButtonClick;
            _ballProjectile.OnBallHitted -= HandleHittedBall;

            OnPlayerLose = null;
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if(!collision.gameObject.GetComponentOrNull<TargetBallView>())
                return;

            _isPlayerAlive = false;
            OnPlayerLose?.Invoke();
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

        private void LoadCurrentPlayerSkins(string currentPlayerSkinId, string currentPlayerBallSkinId)
        {
            if (_playerSkinDatas == null || _playerSkinDatas.Count == 0)
            {
                Debug.LogWarning("[PlayerController] Player skin list is empty. Using empty skin data fallback.");
                _playerSkinData = new PlayerSkinData();
                return;
            }

            var playerSkin = _playerSkinDatas.Find(skinData => skinData != null && skinData.SkinId == currentPlayerSkinId);

            if (playerSkin == null)
            {
                Debug.LogWarning($"[PlayerController] Skin id [{currentPlayerSkinId}] not found. Using first available skin.");
                playerSkin = _playerSkinDatas[0];
            }

            _playerSkinData = playerSkin;
            _playerView.sprite = playerSkin.IdlePose;

            if (_playerBallSkinDatas == null || _playerBallSkinDatas.Count == 0)
            {
                Debug.LogWarning("[PlayerController] Ball skin list is empty. Leaving player ball skin unset.");
                _currentBallSkin = null;
                return;
            }

            var playerBallSkin = _playerBallSkinDatas.Find(skinData => skinData != null && skinData.SkinId == currentPlayerBallSkinId);

            if (playerBallSkin == null)
            {
                Debug.LogWarning($"[PlayerController] Ball skin id [{currentPlayerBallSkinId}] not found. Using first available ball skin.");
                playerBallSkin = _playerBallSkinDatas[0];
            }

            _currentBallSkin = playerBallSkin?.Skin;
        }

        private async UniTask ProjectileFlowAsync(CancellationToken token)
        {
            while (_isPlayerAlive && !token.IsCancellationRequested)
            {
                if (_state != PlayerState.Idle)
                {
                    await UniTask.Yield(token);
                    continue;
                }

                await UniTask.Delay(TimeSpan.FromSeconds(0.5f), cancellationToken: token);

                await PlayerHitProcessAsync(token);
                
                _hitProjectileSource = new();
                _ballProjectile.ShootProjectile(_playerShootForce);

                await UniTask.Delay(TimeSpan.FromSeconds(_playerSpriteChangeDelay * 0.5f), cancellationToken: token);

                _playerView.sprite = _playerSkinData.IdlePose;

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
            _playerView.sprite = _playerSkinData.HitPreparePose;
            await UniTask.Delay(TimeSpan.FromSeconds(_playerSpriteChangeDelay), cancellationToken: token);
            _playerView.sprite = _playerSkinData.HitPose;

            return;
        }

        private async UniTask ProcessProjectileHitAsync(CancellationToken token)
        {
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
            if(!_isPlayerAlive)
                return;

            var nextX = _player.transform.position.x - _playerMoveSpeed * Time.deltaTime;
            _player.transform.position = new Vector3(
                Mathf.Max(nextX, MIN_X),
                _player.transform.position.y,
                _player.transform.position.z
            );
        }

        private void HandleRightButtonClick()
        {
            if(!_isPlayerAlive)
                return;

            var nextX = _player.transform.position.x + _playerMoveSpeed * Time.deltaTime;
            _player.transform.position = new Vector3(
                Mathf.Min(nextX, MAX_X),
                _player.transform.position.y,
                _player.transform.position.z
            );
        }

        private void HandleHittedBall() => _hitProjectileSource?.TrySetResult();
    }
}
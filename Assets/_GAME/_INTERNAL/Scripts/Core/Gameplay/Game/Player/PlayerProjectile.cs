using System;
using Core.Gameplay.Game.TargetSystem;
using Core.Services.Audio;
using UI.Animations.Game;
using UnityEngine;

namespace Core.Gameplay.Game.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CircleCollider2D))]
    public class PlayerProjectile : ObjectAnimations
    {
        [SerializeField] private Vector2 _shootDirection = Vector2.up;

        private int _currentDamage = 1;

        private Rigidbody2D _rb;
        private Transform _defaultPosition;
        private bool _hasHit;

        public int CurrentDamage => _currentDamage;

        public event Action OnBallHitted;

        void OnCollisionEnter2D(Collision2D collision)
        {
            if(!collision.gameObject.TryGetComponent<TargetBallView>(out var _))
                return;
            
            HitBall();
        }

        void OnDestroy()
        {
            OnBallHitted = null;
        }

        public void Init(Transform defaultPosition, int currentDamage = 1)
        {
            _originalScale = transform.localScale;
            _defaultPosition = defaultPosition;
            _rb = GetComponent<Rigidbody2D>();

            // TODO: Damage будет браться из PlayerService
            _currentDamage = currentDamage;
        }

        public void BallAtTheKillzone() => HitBall();
        public void Show() => Appear(_originalScale);
        public void Hide() => Disappear(() => gameObject.SetActive(false));

        public void ShootProjectile(float shootForce)
        {
            _hasHit = false;

            _rb.angularVelocity = 0f;
            _rb.linearVelocity = Vector2.zero;

            _rb.bodyType = RigidbodyType2D.Dynamic;
            _rb.AddForce(_shootDirection * shootForce, ForceMode2D.Impulse);
            AudioService.Instance.PlaySfx(SoundType.Player_Ball_Shoot);
            transform.SetParent(null);
        }

        public void ResetProjectilePosition()
        {
            _rb.angularVelocity = 0f;
            _rb.linearVelocity = Vector2.zero;
            _rb.bodyType = RigidbodyType2D.Kinematic;

            transform.SetParent(_defaultPosition);
            transform.position = _defaultPosition.position;
        }

        private void HitBall()
        {
            if(_hasHit)
                return;
                
            _hasHit = true;
            AudioService.Instance.PlaySfx(SoundType.Player_Ball_Hit);

            OnBallHitted?.Invoke();
            _rb.angularVelocity = 0f;
            _rb.linearVelocity = Vector2.zero;
            _rb.bodyType = RigidbodyType2D.Kinematic;
        }
    }
}
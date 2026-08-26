using System;
using Core.Gameplay.Game.TargetSystem;
using UI.Animations.Game;
using UnityEngine;

namespace Core.Gameplay.Game.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CircleCollider2D))]
    public class PlayerProjectile : ObjectAnimations
    {
        [SerializeField] private Vector2 _shootDirection = Vector2.up;

        private Rigidbody2D _rb;
        private Transform _defaultPosition;

        public event Action OnBallHitted;

        void OnCollisionEnter2D(Collision2D collision)
        {
            if(!collision.gameObject.TryGetComponent<TargetBallView>(out var element))
                return;

            OnBallHitted?.Invoke();
            _rb.bodyType = RigidbodyType2D.Kinematic;
        }

        void OnDestroy()
        {
            OnBallHitted = null;
        }

        public void Init(Transform defaultPosition)
        {
            _originalScale = transform.localScale;
            _defaultPosition = defaultPosition;
            _rb = GetComponent<Rigidbody2D>();
        }

        public void Show() => Appear(_originalScale);
        public void Hide() => Disappear(() => gameObject.SetActive(false));
        public void ShootProjectile(float shootForce)
        {
            _rb.bodyType = RigidbodyType2D.Dynamic;
            _rb.AddForce(_shootDirection * shootForce, ForceMode2D.Impulse);
            transform.SetParent(null);
        }

        public void ResetProjectilePosition()
        {
            transform.SetParent(_defaultPosition);
            transform.position = _defaultPosition.position;
        }
    }
}
using System;
using Core.Gameplay.Game.Player;
using TMPro;
using UnityEngine;

namespace Core.Gameplay.Game.TargetSystem
{
    public class TargetBallView : MonoBehaviour
    {
        [Header("Visual Setup")]
        [SerializeField] private SpriteRenderer _targetSprite;
        [SerializeField] private TextMeshPro _hpLabel;

        [Space(5), Header("Physics Setup")]
        [SerializeField] private float _impulseForce = 1.5f;

        private Rigidbody2D _rb;

        private int _currentHp;

        public event Action<TargetBallView> OnTargetDestroyed;

        void Awake() => _rb = GetComponent<Rigidbody2D>();

        void OnCollisionEnter2D(Collision2D collision)
        {
            if(!collision.gameObject.TryGetComponent<PlayerProjectile>(out var projectile))
                return;

            ApplyDamage(projectile.CurrentDamage);
        }

        public void Init(int currentHp, Vector2 position, Vector2 impulseDirection)
        {
            // TODO: Рандомный спрайт в параметры или отдельный метод
            _currentHp = currentHp;
            _hpLabel.text = $"{_currentHp}";
            _rb.linearVelocity = Vector2.zero;

            transform.position = position;

            _rb.AddForce(impulseDirection * _impulseForce, ForceMode2D.Impulse);
        }

        private void ApplyDamage(int damage)
        {
            if(_hpLabel == null)
                return;

            _currentHp -= damage;

            if(_currentHp <= 0)
            {
                _currentHp = 0;
                OnTargetDestroyed?.Invoke(this);
            }

            _hpLabel.text = $"{_currentHp}";
        }
    }
}
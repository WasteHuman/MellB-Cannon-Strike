using UI.Other;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Gameplay.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Move Buttons Setup")]
        [SerializeField] private ActionButton _goToLeftButton;
        [SerializeField] private ActionButton _goToRightButton;

        [Space(5), Header("Player Setup")]
        [SerializeField] private GameObject _player;
        [SerializeField] private float _playerMoveSpeed = 5f;

        public void Initialize()
        {
            _goToLeftButton.IsUseHeldFunc = true;
            _goToRightButton.IsUseHeldFunc = true;

            _goToLeftButton.OnButtonClick += HandleLeftButtonClick;
            _goToRightButton.OnButtonClick += HandleRightButtonClick;
        }

        public void Dispose()
        {
            _goToLeftButton.OnButtonClick -= HandleLeftButtonClick;
            _goToRightButton.OnButtonClick -= HandleRightButtonClick;
        }

        void Update()
        {
            #if UNITY_EDITOR
            if(Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
                HandleLeftButtonClick();
            
            if(Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
                HandleRightButtonClick();
            #endif
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
    }
}
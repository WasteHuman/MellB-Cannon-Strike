using System;
using Core.Data;
using TMPro;
using UI.Other;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Shop
{
    public class ShopItemView : MonoBehaviour
    {
        [SerializeField] private Image _itemIcon;
        [SerializeField] private Image _buttonIcon;
        [SerializeField] private TextMeshProUGUI _descriptionLabel;
        [SerializeField] private TextMeshProUGUI _costLabel;

        [Space(5), Header("Button Setup")]
        [SerializeField] private ActionButton _buyAndSelectButton;
        [SerializeField] private Sprite _selectedSprite;
        [SerializeField] private Sprite _unselectedSprite;
        [SerializeField] private Sprite _buySprite;
        [SerializeField] private GameObject _boughtCheckmark;

        private string _itemId = string.Empty;
        private ShopItemType _type;

        private bool _isPurchased = false;
        private bool _isSelected = false;

        public string ItemId => _itemId;
        public bool IsSelected => _isSelected;
        public ShopItemType Type => _type;

        public event Action<string> OnItemPurchased;
        public event Action<string> OnItemSelected;

        void OnDestroy()
        {
            _buyAndSelectButton.OnButtonClick -= HandleButtonClick;
            OnItemPurchased = null;
            OnItemSelected = null;
        }

        public void Init(Sprite itemSprite, string itemId, string description, int cost, ShopItemType type, bool isPurchased = false, bool isSelected = false)
        {
            _itemIcon.sprite = itemSprite;
            _itemId = itemId;
            _type = type;

            _isPurchased = isPurchased;
            _isSelected = isSelected;

            if(_descriptionLabel != null)
                _descriptionLabel.text = description;

            _costLabel.text = $"{cost}";
            _costLabel.gameObject.SetActive(true);

            if (isPurchased && !isSelected)
            {
                if(type == ShopItemType.Skin)
                    _buttonIcon.sprite = _unselectedSprite;
                else
                {
                    _buyAndSelectButton.gameObject.SetActive(false);
                    _boughtCheckmark.SetActive(true);
                }

                _costLabel.gameObject.SetActive(false);
            }
            else if(!isPurchased)
                _buttonIcon.sprite = _buySprite;

            if (isSelected)
            {
                _buttonIcon.sprite = _selectedSprite;
                _costLabel.gameObject.SetActive(false);
            }

            _buyAndSelectButton.OnButtonClick += HandleButtonClick;
        }

        public void UpdateToPurchasedItemView()
        {
            if (!_isPurchased)
            {
                _isPurchased = true;
                _costLabel.gameObject.SetActive(false);
                UpdateToUnselectedItemView(_type);
            }
        }

        public void UpdateToSelectedItemView()
        {
            if (_isPurchased)
            {
                _buttonIcon.sprite = _selectedSprite;
                _isSelected = true;
            }
        }

        public void UpdateToUnselectedItemView(ShopItemType type)
        {
            if (type == ShopItemType.Skin)
            {
                if (_isSelected)
                {
                    _buttonIcon.sprite = _unselectedSprite;
                    _isSelected = false;
                }
                else if(_isPurchased && !_isSelected)
                {
                    _buttonIcon.sprite = _unselectedSprite;
                    _isSelected = false;
                }
            }
            else
            {
                _buyAndSelectButton.gameObject.SetActive(false);
                _boughtCheckmark.SetActive(true);
            }
        }

        private void HandleButtonClick()
        {
            if(!_isPurchased)
                OnItemPurchased?.Invoke(_itemId);
            else if (_isPurchased && !_isSelected)
                OnItemSelected?.Invoke(_itemId);
        }
    }
}
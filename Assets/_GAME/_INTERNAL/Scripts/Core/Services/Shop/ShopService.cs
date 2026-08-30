using System;
using System.Collections.Generic;
using Core.Data;
using Core.Gameplay;
using Core.Services.Player;
using Core.SO;
using UnityEngine;

namespace Core.Services.Shop
{
    public class ShopService
    {
        private readonly List<ShopEntityData> _ballsData = new();
        private readonly List<ShopEntityData> _skinsData = new();
        private readonly List<ShopEntityData> _upgradesData = new();

        private PlayerService _playerService;
        private EconomyService _economyService;

        public IReadOnlyList<ShopEntityData> BallsData => _ballsData.AsReadOnly();
        public IReadOnlyList<ShopEntityData> SkinsData => _skinsData.AsReadOnly();
        public IReadOnlyList<ShopEntityData> UpgradesData => _upgradesData.AsReadOnly();

        public void Init(PlayerService playerService, EconomyService economyService, ShopEntitiesConfig entitiesConfig)
        {
            _playerService = playerService;
            _economyService = economyService;

            InitPlayerSkinDatas(entitiesConfig.PlayerSkinsData.AsReadOnly());
            InitPlayerBallSkinDatas(entitiesConfig.PlayerBallSkinsData.AsReadOnly());
            InitPlayerUpgradedatas(entitiesConfig.PlayerUpgradesData.AsReadOnly());
        }

        public void BuyPlayerSkin(string skinId)
        {
            var skin = _skinsData.Find(skin => skin.EntityID == skinId);

            if(skin == null)
            {
                Debug.LogWarning($"[Shop Service] Skin to bought is null!");
                return;
            }

            if (_economyService.HasEnoughBalance(skin.EntityCost) && !skin.IsPurchased)
            {
                _economyService.SpendCoins(skin.EntityCost);
                skin.IsPurchased = true;
                _playerService.AddSkinToPurchased(skin.EntityID);
            }
            else
                Debug.LogWarning($"[Shop Service] Player not enough coins for buy this skin {skinId}!");
        }

        public void BuyPlayerBallSkin(string skinId)
        {
            var skin = _ballsData.Find(skin => skin.EntityID == skinId);

            if(skin == null)
            {
                Debug.LogWarning($"[Shop Service] Skin to bought is null!");
                return;
            }

            if (_economyService.HasEnoughBalance(skin.EntityCost) && !skin.IsPurchased)
            {
                _economyService.SpendCoins(skin.EntityCost);
                skin.IsPurchased = true;
                _playerService.AddBallSkinToPurchased(skin.EntityID);
                SelectPlayerBallSkin(skin.EntityID);
            }
            else
                Debug.LogWarning($"[Shop Service] Player not enough coins for buy this skin {skinId}!");
        }

        public void BuyPlayerUpgrade(string upgradeId)
        {
            var upgrade = _upgradesData.Find(skin => skin.EntityID == upgradeId);

            if(upgrade == null)
            {
                Debug.LogWarning($"[Shop Service] Upgrade to bought is null!");
                return;
            }

            if (_economyService.HasEnoughBalance(upgrade.EntityCost) && !upgrade.IsPurchased)
            {
                _economyService.SpendCoins(upgrade.EntityCost);
                upgrade.IsPurchased = true;
                _playerService.AddUpgradeToPurchased(upgrade.EntityID);

                if(upgradeId.Contains("Damage"))
                    _playerService.DoublePlayerDamage();
                
                if(upgradeId.Contains("Reload"))
                    _playerService.ReducePlayerReload();
            }
            else
                Debug.LogWarning($"[Shop Service] Player not enough coins for buy this upgrade {upgradeId}!");
        }

        public void SelectPlayerSkin(string skinId)
        {
            var skin = _skinsData.Find(skin => skin.EntityID == skinId);

            if(skin == null)
            {
                Debug.LogWarning($"[Shop Service] Selected skin is null!");
                return;
            }

            _playerService.ChangePlayerSkin(skinId);
        }

        public void SelectPlayerBallSkin(string skinId)
        {
            var skin = _ballsData.Find(skin => skin.EntityID == skinId);

            if(skin == null)
            {
                Debug.LogWarning($"[Shop Service] Selected ball skin is null!");
                return;
            }

            _playerService.ChangePlayerBallSkin(skinId);
        }

        private void InitPlayerSkinDatas(IReadOnlyList<ShopEntityData> playerSkinDatas)
        {
            _skinsData.AddRange(playerSkinDatas);

            _skinsData.Sort((a, b) => a.EntityCost.CompareTo(b.EntityCost));
            var purchasedSkins = _playerService.GetData().PurchasedPlayerSkins;
            for (int i = 0; i < purchasedSkins.Count; i++)
            {
                var purchasedSkin = purchasedSkins[i];
                var skinDef = _skinsData.Find(skin => skin.EntityID == purchasedSkin);
                if (skinDef != null)
                {
                    skinDef.IsPurchased = true;
                }
            }
        }

        private void InitPlayerBallSkinDatas(IReadOnlyList<ShopEntityData> playerBallSkinDatas)
        {
            _ballsData.AddRange(playerBallSkinDatas);

            _ballsData.Sort((a, b) => a.EntityCost.CompareTo(b.EntityCost));
            var purchasedSkins = _playerService.GetData().PurchasedPlayerBallSkins;
            for (int i = 0; i < purchasedSkins.Count; i++)
            {
                var purchasedSkin = purchasedSkins[i];
                var skinDef = _ballsData.Find(skin => skin.EntityID == purchasedSkin);
                if (skinDef != null)
                {
                    skinDef.IsPurchased = true;
                }
            }
        }

        private void InitPlayerUpgradedatas(IReadOnlyList<ShopEntityData> upgradeDatas)
        {
            _upgradesData.AddRange(upgradeDatas);

            _upgradesData.Sort((a, b) => a.EntityCost.CompareTo(b.EntityCost));
            var purchasedSkins = _playerService.GetData().PurchasedUpgrades;
            for (int i = 0; i < purchasedSkins.Count; i++)
            {
                var purchasedSkin = purchasedSkins[i];
                var upgradeDef = _upgradesData.Find(upgrade => upgrade.EntityID == purchasedSkin);
                if (upgradeDef != null)
                {
                    upgradeDef.IsPurchased = true;
                }
            }
        }
    }
}
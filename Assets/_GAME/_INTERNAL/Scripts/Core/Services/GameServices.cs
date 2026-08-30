using Core.Gameplay;
using Core.Services.Player;
using Core.Services.SaveSystem;
using Core.Services.Shop;
using Core.SO;
using UnityEngine;

namespace Core.Services
{
    public static class GameServices
    {
        public static PlayerService PlayerService { get; private set; }
        public static SaveService SaveService { get; private set; }
        public static EconomyService EconomyService { get; private set; }
        public static ShopService ShopService { get; private set; }
        public static GameSessionService GameSessionService { get; private set; }

        public static void InitializeAll(bool isDebug = false)
        {
            SaveService = new();
            SaveService.Init(isDebug);

            PlayerService = new();
            PlayerService.Init(SaveService.PlayerData);

            EconomyService = new();
            EconomyService.Init(PlayerService.PlayerCoins);

            var shopEntitiesConfig = Resources.Load<ShopEntitiesConfig>("Configs/Shop/ShopEntitiesConfig");
            if(shopEntitiesConfig == null)
            {
                Debug.LogError($"[Game Services] Shop Entities Config is null!");
                return;
            }

            ShopService = new();
            ShopService.Init(PlayerService, EconomyService, shopEntitiesConfig);

            GameSessionService = new();
            GameSessionService.Init(EconomyService);
        }

        public static void SaveAll()
        {
            SaveService.PlayerData.Coins = EconomyService.GetCoinsBalance();
            SaveService.SavePlayerData();
        }
    }
}
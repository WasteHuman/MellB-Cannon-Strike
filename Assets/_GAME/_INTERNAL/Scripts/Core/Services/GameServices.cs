using Core.Gameplay;
using Core.Services.Player;
using Core.Services.SaveSystem;

namespace Core.Services
{
    public static class GameServices
    {
        public static PlayerService PlayerService { get; private set; }
        public static SaveService SaveService { get; private set; }
        public static EconomyService EconomyService { get; private set; }

        public static void InitializeAll(bool isDebug = false)
        {
            SaveService = new();
            SaveService.Init(isDebug);

            PlayerService = new();
            PlayerService.Init(SaveService.PlayerData);

            EconomyService = new();
            EconomyService.Init(PlayerService.PlayerCoins);
        }

        public static void SaveAll()
        {
            SaveService.PlayerData.Coins = EconomyService.GetCoinsBalance();
            SaveService.SavePlayerData();
        }
    }
}
using System;
using System.Collections.Generic;

namespace Core.Data
{
    [Serializable]
    public class PlayerData
    {
        public string CurrentPlayerSkinId;
        public string CurrentPlayerBallSkinId;
        public int CurrentPlayerDamage;
        public float CurrentPlayerReload;
        public float Coins;
        public DateTime? LastDailyBonusTime;

        public List<string> PurchasedPlayerSkins;
        public List<string> PurchasedPlayerBallSkins;
        public List<string> PurchasedUpgrades;

        public PlayerData()
        {
            Coins = GameConstants.INITIAL_COINS;

            LastDailyBonusTime = DateTime.Now;

            CurrentPlayerSkinId = GameConstants.INITIAL_PLAYER_SKIN_ID;
            CurrentPlayerDamage = GameConstants.INITIAL_PLAYER_DAMAGE;
            CurrentPlayerReload = GameConstants.INITIAL_PLAYER_RELOAD;
            CurrentPlayerBallSkinId = GameConstants.INITIAL_PLAYER_BALL_SKIN_ID;

            PurchasedPlayerSkins = new()
            {
                GameConstants.INITIAL_PLAYER_SKIN_ID  
            };

            PurchasedPlayerBallSkins = new()
            {
                GameConstants.INITIAL_PLAYER_BALL_SKIN_ID
            };

            PurchasedUpgrades = new();
        }
    }
}
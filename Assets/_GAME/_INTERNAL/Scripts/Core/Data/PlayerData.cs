using System;

namespace Core.Data
{
    [Serializable]
    public class PlayerData
    {
        public int CurrentPlayerSkinId;
        public int CurrentPlayerDamage;
        public float CurrentPlayerReload;
        public float Coins;
        public DateTime? LastDailyBonusTime;

        public PlayerData()
        {
            Coins = GameConstants.INITIAL_COINS;
            LastDailyBonusTime = DateTime.Now;
            CurrentPlayerSkinId = GameConstants.INITIAL_PLAYER_SKIN_ID;
            CurrentPlayerDamage = GameConstants.INITIAL_PLAYER_DAMAGE;
            CurrentPlayerReload = GameConstants.INITIAL_PLAYER_RELOAD;
        }
    }
}
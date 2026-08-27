using System;

namespace Core.Data
{
    [Serializable]
    public class PlayerData
    {
        public int CurrentPlayerSkinId;
        public float Coins;
        public DateTime? LastDailyBonusTime;

        public PlayerData()
        {
            Coins = GameConstants.INITIAL_COINS;
            LastDailyBonusTime = DateTime.Now;
            CurrentPlayerSkinId = 0;
        }
    }
}
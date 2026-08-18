using System;

namespace Core.Data
{
    [System.Serializable]
    public class PlayerData
    {
        public float Coins;
        public DateTime? LastDailyBonusTime;

        public PlayerData()
        {
            Coins = GameConstants.INITIAL_COINS;
            LastDailyBonusTime = DateTime.Now;
        }
    }
}
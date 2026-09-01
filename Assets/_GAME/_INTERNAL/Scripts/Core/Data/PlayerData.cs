using System;
using System.Collections.Generic;
using UnityEngine;

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
        public bool IsTutorialCompleted;
        public DateTime? LastDailyBonusTime;

        public List<string> PurchasedPlayerSkins;
        public List<string> PurchasedPlayerBallSkins;
        public List<string> PurchasedUpgrades;

        public PlayerData()
        {
            EnsureValidState();
        }

        public void EnsureValidState()
        {
            if (string.IsNullOrEmpty(CurrentPlayerSkinId))
                CurrentPlayerSkinId = GameConstants.INITIAL_PLAYER_SKIN_ID;

            if (string.IsNullOrEmpty(CurrentPlayerBallSkinId))
                CurrentPlayerBallSkinId = GameConstants.INITIAL_PLAYER_BALL_SKIN_ID;

            if (CurrentPlayerDamage <= 0)
                CurrentPlayerDamage = GameConstants.INITIAL_PLAYER_DAMAGE;

            if (CurrentPlayerReload <= 0)
                CurrentPlayerReload = GameConstants.INITIAL_PLAYER_RELOAD;

            if (Coins <= 0)
                Coins = GameConstants.INITIAL_COINS;

            if (PlayerPrefs.HasKey(GameConstants.KEY_TUTORIAL_COMPLETED) && !IsTutorialCompleted)
                IsTutorialCompleted = PlayerPrefs.GetInt(GameConstants.KEY_TUTORIAL_COMPLETED) == 1;

            LastDailyBonusTime ??= DateTime.Now;

            PurchasedPlayerSkins ??= new List<string>();
            if (!PurchasedPlayerSkins.Contains(GameConstants.INITIAL_PLAYER_SKIN_ID))
                PurchasedPlayerSkins.Add(GameConstants.INITIAL_PLAYER_SKIN_ID);

            PurchasedPlayerBallSkins ??= new List<string>();
            if (!PurchasedPlayerBallSkins.Contains(GameConstants.INITIAL_PLAYER_BALL_SKIN_ID))
                PurchasedPlayerBallSkins.Add(GameConstants.INITIAL_PLAYER_BALL_SKIN_ID);

            PurchasedUpgrades ??= new List<string>();
        }
    }
}
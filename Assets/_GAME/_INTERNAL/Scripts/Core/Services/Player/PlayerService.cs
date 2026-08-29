using Core.Data;

namespace Core.Services.Player
{
    public class PlayerService
    {
        private PlayerData _currentPlayerData;

        private int _sessionPlayerScore;
        private int _sessionEarnedCoins;

        public float PlayerCoins => _currentPlayerData.Coins;
        public int CurrentPlayerSkinId => _currentPlayerData.CurrentPlayerSkinId;
        public int CurrentPlayerDamage => _currentPlayerData.CurrentPlayerDamage;
        public float CurrentPlayerReload => _currentPlayerData.CurrentPlayerReload;
        public int SessionPlayerScore => _sessionPlayerScore;
        public int SessionEarnedCoins => _sessionEarnedCoins;

        public void Init(PlayerData playerData)
        {
            _currentPlayerData = playerData;
        }

        public void DoublePlayerDamage() => _currentPlayerData.CurrentPlayerDamage *= 2;
        public void ReducePlayerReload() => _currentPlayerData.CurrentPlayerReload *= 0.25f;

        public void ResetSessionScore() => _sessionPlayerScore = 0;
        public void ResetEarnedSessionCoins() => _sessionEarnedCoins = 0;
        public void IncreasePlayerSessionScore() => _sessionPlayerScore++;
        public void AddEarnedCoins(int amount) => _sessionEarnedCoins += amount;

        /// <summary>
        /// Получить прямой доступ к PlayerData для сложных операций
        /// Использовать осторожно, только для внутренних сервисов
        /// </summary>
        internal PlayerData GetData() => _currentPlayerData;
    }
}
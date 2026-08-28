using Core.Data;

namespace Core.Services.Player
{
    public class PlayerService
    {
        private PlayerData _currentPlayerData;

        public float PlayerCoins => _currentPlayerData.Coins;
        public int CurrentPlayerSkinId => _currentPlayerData.CurrentPlayerSkinId;

        public void Init(PlayerData playerData)
        {
            _currentPlayerData = playerData;
        }

        public void DoublePlayerDamage() => _currentPlayerData.CurrentPlayerDamage *= 2;
        public void ReducePlayerReload() => _currentPlayerData.CurrentPlayerReload *= 0.25f;

        /// <summary>
        /// Получить прямой доступ к PlayerData для сложных операций
        /// Использовать осторожно, только для внутренних сервисов
        /// </summary>
        internal PlayerData GetData() => _currentPlayerData;
    }
}
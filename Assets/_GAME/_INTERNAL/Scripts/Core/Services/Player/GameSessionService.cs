using System;
using Core.Data;
using Core.Gameplay;

namespace Core.Services.Player
{
    public class GameSessionService
    {
        private EconomyService _economyService;

        public event Action<GameResult> OnGameEnded;

        public void Init(EconomyService economyService)
        {
            _economyService = economyService;
        }

        public void HandleEndedGame(GameResult sessionResult)
        {
            _economyService.AddCoins(sessionResult.RewardCoins);

            OnGameEnded?.Invoke(sessionResult);
        }
    }
}
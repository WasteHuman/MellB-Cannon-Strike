namespace Core
{
    public static class GameConstants
    {
        #region Main Scene Names
        public const string MAIN_MENU = "Main_Menu";
        public const string GAME = "Game";
        public const string SETTINGS = "Settings";
        public const string SHOP_MENU = "Shop";
        #endregion

        #region Settings Prefs
        public const string KEY_NOTIFICATIONS = "Notifications";
        public const string KEY_VIBRATIONS = "Vibrations";
        #endregion

        #region Player Prefs
        public const string KEY_WELCOME_SCREEN_SHOWN = "Welcome_Screen_Shown";
        public const string KEY_PLAYER_DATA = "Player_Data_JSON";
        public const string KEY_LAST_FREE_DAILY_BONUS_CLAIM = "Last_Daily_Free_Bonus";
        #endregion

        #region Economy & Limits
        public const float INITIAL_COINS = 1000f;
        #endregion
    }
}
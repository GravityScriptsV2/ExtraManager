namespace ExtraManager.Engine.FrontendSystems
{
    internal static class ConfigMenu
    {
        #region Fields
        public static MenuPool Pool;
        public static UIMenu MainMenu;
        #endregion

        #region Setup
        public static void Setup()
        {
            Pool = new MenuPool();
            MainMenu = new UIMenu("ExtraManager", "MAIN MENU ~y~v1.0");

            Pool.Add(MainMenu);
            GameFiber.StartNew(ProcessMenus);
        }
        #endregion

        #region Menu Processing
        private static void ProcessMenus()
        {
            while (true)
            {
                GameFiber.Yield();
                Pool.ProcessMenus();
            }
        }
        #endregion
    }
}

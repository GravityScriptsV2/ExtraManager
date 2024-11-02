using RAGENativeUI.Elements;

namespace ExtraManager.Engine.FrontendSystems
{
    internal static class ConfigMenu
    {
        #region Fields

        private static MenuPool Pool;
        public static UIMenu MainMenu;
        private static UIMenuNumericScrollerItem<int> windowTintSelector;

        private static readonly Dictionary<int, string> TintLevelNames = new Dictionary<int, string>
        {
            { -1, "None" },
            { 0, "Stock" },
            { 1, "Pure Black" },
            { 2, "Dark Smoke" },
            { 3, "Light Smoke" },
            { 4, "Limo" },
            { 5, "Green" },
        };
        #endregion

        public static void Setup()
        {
            Pool = new MenuPool();
            MainMenu = new UIMenu("ExtraManager", "MAIN MENU ~y~v1.0");

            var playerVehicle = Game.LocalPlayer.Character.CurrentVehicle;

            //windowTintSelector = new UIMenuNumericScrollerItem<int>("Window Tint Level", "Adjust the tint level for your vehicle's windows.", -1, 5, 1) { Value = NativeFunction.CallByHash<int>(0x0EE21293DAD47C95, playerVehicle) };
            //windowTintSelector.Formatter = value =>
            //           TintLevelNames.ContainsKey(value) ? TintLevelNames[value] : "Unknown";

            //MainMenu.AddItem(windowTintSelector);

            Pool.Add(MainMenu);

            GameFiber.StartNew(ProcessMenus);
        }

        private static void ProcessMenus()
        {
            while (true)
            {
                GameFiber.Yield();

                Pool.ProcessMenus();
            }
        }
    }
}

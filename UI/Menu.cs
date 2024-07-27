using Rage;
using RAGENativeUI;
using RAGENativeUI.PauseMenu;
using System.Windows.Forms;

namespace ExtraManager.UI
{
    public static class Menu
    {
        #region Fields

        private static MenuPool Pool;
        public static UIMenu MainMenu;

        #endregion

        public static void Setup()
        {
            Pool = new MenuPool();
            MainMenu = new UIMenu("ExtraManager", "MAIN MENU ~y~v1.0");

            Pool.Add(MainMenu);

            GameFiber.StartNew(ProcessMenus);
        }

        private static void ProcessMenus()
        {
            while (true)
            {
                GameFiber.Yield();
                
                if (Game.IsShiftKeyDownRightNow && Game.IsKeyDown(Keys.F5) &&
                    !UIMenu.IsAnyMenuVisible && !TabView.IsAnyPauseMenuVisible)
                {
                    var playerPed = Game.LocalPlayer.Character;

                    if (playerPed.IsInAnyVehicle(false))
                    {
                        MainMenu.Visible = true;
                    }
                    else
                    {
                        Game.DisplaySubtitle("Not in Vehicle, or No Saved Vehicle");
                    }
                }

                Pool.ProcessMenus();
            }
        }
    }
}

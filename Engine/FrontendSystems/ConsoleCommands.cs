using RAGENativeUI.PauseMenu;

namespace ExtraManager.Engine.FrontendSystems
{
    internal class ConsoleCommands : CommonPlugin
    {
        [ConsoleCommand("Open the ExtraManager configuration menu")]
        internal static void OpenExMConfigMenu()
        {
            if (!UIMenu.IsAnyMenuVisible && !TabView.IsAnyPauseMenuVisible)
            {
                if (ClientPed.IsInAnyVehicle(false))
                {
                    ConfigMenu.MainMenu.Visible = true;
                }
                else
                {
                    Game.DisplaySubtitle("You must be in a vehicle to access this menu.");
                }
            }
        }
    }
}

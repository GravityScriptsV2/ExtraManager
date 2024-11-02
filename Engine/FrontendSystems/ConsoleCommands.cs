using RAGENativeUI.PauseMenu;

namespace ExtraManager.Engine.FrontendSystems
{
    internal static class ConsoleCommands
    {
        [ConsoleCommand("Open the RiskierTrafficStops config menu")]
        internal static void RtsOpenConfigMenu()
        {
            if (!UIMenu.IsAnyMenuVisible && !TabView.IsAnyPauseMenuVisible)
            {
                var playerPed = Game.LocalPlayer.Character;

                if (playerPed.IsInAnyVehicle(false))
                {
                    ConfigMenu.MainMenu.Visible = true;
                }
                else
                {
                    Game.DisplaySubtitle("Not in Vehicle, or No Saved Vehicle");
                }
            }
        }
    }
}

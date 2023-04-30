namespace HoyoLauncherProject.Core;

sealed class HoyoLauncher 
{
    public static bool IsGameRunning { get; set; } = false;

    public static void Initialize()
    {
        EventHandlers.Initialize();
    }
}
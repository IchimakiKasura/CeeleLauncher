namespace HoyoLauncherProject.Core;

sealed class HoyoLauncher 
{
    public static void Initialize()
    {
        EventHandlers.Initialize();
    }
}
namespace HoyoLauncherProject.Core;

sealed class GameConfig
{
    public string GameFolder { get; private set; }
    public string GameExecutable { get; private set; }
    public string GameBackground { get; private set; }

    public static GameConfig Read(string GameDirectory)
    {
        var ConfigData = File.ReadAllText(Path.Combine(GameDirectory, "config.ini"));
        var RawData = new IniParser.IniDataParser().Parse(ConfigData);

        return new()
        {
            GameFolder = RawData["launcher"]["game_install_path"],
            GameExecutable = RawData["launcher"]["game_start_name"],
            GameBackground = Path.Combine(GameDirectory, "bg", RawData["launcher"]["game_dynamic_bg_name"])
        };
    }

    public static bool IsConfigExist(string path) =>
        File.Exists(Path.Combine(path, "config.ini"));

}
using IniParser;

namespace HoyoLauncher.Core.Config;

public sealed class ConfigRead
{
    public bool FilePathNone { get; private set; } = false;
    public bool ConfigExist { get; private set; } = false;
    public string GameInstallPath { get; private set; }
    public string GameBackgroundName { get; private set; }
    public string GameStartName { get; private set; }
    public string GameName { get ; private set; }
    public ImageBrush GameBackground { get; private set; }
    public string GameVersion { get; private set; }
    public string GameBackgroundMD5 { get; private set; }

    public static ConfigRead GetConfig(string FilePath)
    {
        ImageBrush GameBG_TEMP = null;

        const string GroupName = "launcher";
        string gamepath, gamebg, gamename, gamebgmd5, gamever;

        var ConfigFile = Path.Combine(FilePath, "config.ini");
        bool configexist = File.Exists(ConfigFile);

        if(string.IsNullOrEmpty(FilePath))
            return new()
            {
                FilePathNone = true
            };

        if(!configexist)
            return new()
            {
                ConfigExist = configexist
            };

        var LauncherConfig = File.ReadAllText(Path.Combine(FilePath, "config.ini"));
        var ParsedLauncherObject = new IniDataParser().Parse(LauncherConfig);

        try
        {
            gamepath = ParsedLauncherObject[GroupName]["game_install_path"];
            gamebg = ParsedLauncherObject[GroupName]["game_dynamic_bg_name"];
            gamename = ParsedLauncherObject[GroupName]["game_start_name"];
            gamebgmd5 = ParsedLauncherObject[GroupName]["game_dynamic_bg_md5"];
        }
        catch
        {
            return new()
            {
                ConfigExist = false
            };
        }

        var CheckBGExist = Path.Combine(FilePath, "bg", gamebg);

        if(File.Exists(CheckBGExist))
            GameBG_TEMP = new(new BitmapImage(new(Path.Combine(FilePath, "bg", gamebg), UriKind.RelativeOrAbsolute)));

        var GameConfig = File.ReadAllText(Path.Combine(gamepath, "config.ini"));
        var ParsedGameObject = new IniDataParser().Parse(GameConfig);

        gamever = ParsedGameObject["General"]["game_version"];

        return new()
        {
            ConfigExist = configexist,
            GameInstallPath = gamepath,
            GameBackgroundName = Path.Combine(FilePath, "bg", gamebg),
            GameStartName = Path.Combine(gamepath, gamename),
            GameBackground = GameBG_TEMP,
            GameName = gamename,
            GameVersion = gamever,
            GameBackgroundMD5 = gamebgmd5
        };
    }    
}
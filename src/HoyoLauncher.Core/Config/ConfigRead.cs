using IniParser;

namespace HoyoLauncher.Core.Config;

public sealed class ConfigRead
{
    // I like how the "GameInstallPath" and the "GameBackgroundName" isn't being used on other methods
    public bool FilePathNone { get; private set; } = false;
    public bool ConfigExist { get; private set; } = false;
    public string GameInstallPath { get; private set; }
    public string GameBackgroundName { get; private set; }
    public string GameStartName { get; private set; }
    public ImageBrush GameBackground { get; private set; }

    public static ConfigRead GetConfig(string FilePath)
    {
        ImageBrush GameBG_TEMP = null;

        const string grp = "launcher";
        string gamepath, gamebg, gamename;

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

        var DataObject = File.ReadAllText(Path.Combine(FilePath, "config.ini"));
        var ParsedObject = new IniDataParser().Parse(DataObject);

        try
        {
            gamepath = ParsedObject[grp]["game_install_path"];
            gamebg = ParsedObject[grp]["game_dynamic_bg_name"];
            gamename = ParsedObject[grp]["game_start_name"];
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

        return new()
        {
            ConfigExist = configexist,
            GameInstallPath = gamepath,
            GameBackgroundName = Path.Combine(FilePath, "bg", gamebg),
            GameStartName = Path.Combine(gamepath, gamename),
            GameBackground = GameBG_TEMP
        };
    }    
}
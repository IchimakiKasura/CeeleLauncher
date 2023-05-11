using IniParser;

namespace HoyoLauncher.Core.Config;

public sealed partial class ConfigRead
{
    public bool FilePathNone = false;
    public bool ConfigExist = false;
    public string GameInstallPath { get; private set; }
    public string GameBackgroundName { get; private set; }
    public string GameStartName { get; private set; }
    public ImageBrush GameBackground { get; private set; }

    public ConfigRead() { }

    public ConfigRead GetConfig(string FilePath)
    {
        const string grp = "launcher";
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

        var gamepath = ParsedObject[grp]["game_install_path"];
        var gamebg = ParsedObject[grp]["game_dynamic_bg_name"];
        var gamename = ParsedObject[grp]["game_start_name"];

        return new()
        {
            ConfigExist = configexist,
            GameInstallPath = gamepath,
            GameBackgroundName = Path.Combine(FilePath, "bg", gamebg),
            GameStartName = Path.Combine(gamepath, gamename),
            GameBackground = new(new BitmapImage(new(Path.Combine(FilePath, "bg", gamebg), UriKind.RelativeOrAbsolute)))
        };
    }
}
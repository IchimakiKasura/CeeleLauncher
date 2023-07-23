namespace HoyoLauncher.Core.Config;

public sealed class GameConfigRead
{
    public bool FilePathNone { get; private set; } = false;
    public bool ConfigExist { get; private set; } = false;
    public bool GameConfigExist { get; private set; } = false;
    public string GameInstallPath { get; private set; }
    public string GameBackgroundName { get; private set; }
    public string GameStartName { get; private set; }
    public string GameName { get ; private set; }
    public ImageBrush GameBackground { get; private set; }
    public string GameVersion { get; private set; }
    public string GameBackgroundMD5 { get; private set; }

    // ☠️
    private static GameConfigRead instance = null;
    public static GameConfigRead Instance => instance ??= new GameConfigRead();
    private GameConfigRead() { }

    public static async Task<GameConfigRead> GetConfig(string FilePath)
    {
        ImageBrush GameBG_TEMP = null;

        const string GroupName = "launcher";

        var ConfigFile = Path.Combine(FilePath, "config.ini");
        bool configexist = File.Exists(ConfigFile);
        string gamepath, gamebg, gamename, gamebgmd5, gamever = "0";

        if (string.IsNullOrEmpty(FilePath))
            return new()
            {
                FilePathNone = true
            };

        if (!configexist)
            return new()
            {
                ConfigExist = configexist
            };

        var ParsedLauncherObject = await ReadFile(Path.Combine(FilePath, "config.ini"));
        
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

        bool GameConfigExist;
        if (Directory.Exists(gamepath))
        {
            GameConfigExist = true;
            var ParsedGameObject = await ReadFile(Path.Combine(gamepath, "config.ini"));
            gamever = ParsedGameObject["General"]["game_version"];
        }
        else GameConfigExist = false;

        return new()
        {
            ConfigExist = configexist,
            GameConfigExist = GameConfigExist,
            GameInstallPath = gamepath,
            GameBackgroundName = Path.Combine(FilePath, "bg", gamebg),
            GameStartName = Path.Combine(gamepath, gamename),
            GameBackground = GameBG_TEMP,
            GameName = gamename,
            GameVersion = gamever,
            GameBackgroundMD5 = gamebgmd5
        };
    }    

    static async Task<IniData> ReadFile(string filePath)
    {
        using FileStream fileStream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        using StreamReader streamReader = new(fileStream, Encoding.ASCII);
        var dataMessage = await streamReader.ReadToEndAsync();
        return new IniDataParser().Parse(dataMessage); 
    }
}
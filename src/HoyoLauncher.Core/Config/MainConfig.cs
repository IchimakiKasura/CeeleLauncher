namespace HoyoLauncher.Core.Config;

[Serializable]
public sealed class MainConfig
{
    static readonly string FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.ini");

    /// <summary> Genshin Impact Directory </summary>
    public string GI_DIR = "";
    /// <summary> Honkai Star Rail Directory </summary>
    public string HSR_DIR = "";
    /// <summary> Honkai Impact 3rd Directory </summary>
    public string HI3_DIR = "";
    /// <summary> Zenless Zone Zero Directory </summary>
    public string ZZZ_DIR = "";

    /// <summary> Last game after close </summary>
    public int LAST_GAME = 0;

    public bool CHECKBOX_EXIT_TRAY = true;
    public bool CHECKBOX_BACKGROUND = true;
    public bool CHECKBOX_LAST_GAME = true;
    public bool CHECKBOX_TITLE = true;

    public bool FIRST_RUN  = false;

    private static MainConfig instance = null;
    public static MainConfig Instance
    {
        get => instance ??= new MainConfig();
    }

    private MainConfig()
    { }

    public void Reset()
    {
        GI_DIR = string.Empty;
        HSR_DIR = string.Empty;
        HI3_DIR = string.Empty;
        ZZZ_DIR = string.Empty;

        LAST_GAME = 0;

        CHECKBOX_EXIT_TRAY = true;
        CHECKBOX_BACKGROUND = true;
        CHECKBOX_LAST_GAME = true;
        CHECKBOX_TITLE = true;

        FIRST_RUN = false;
    }
    
    public string GetConfigAsString() =>
    $"""
    [DIRECTORIES]
    GenshinImpact_DIR={GI_DIR}
    HonkaiStarRail_DIR={HSR_DIR}
    HonkaiImpact_DIR={HI3_DIR}
    ZenlessZoneZero_DIR={ZZZ_DIR}

    [SETTINGS]
    LastGame={LAST_GAME}
    ExitToTray={CHECKBOX_EXIT_TRAY}
    ShowBackground={CHECKBOX_BACKGROUND}
    LastGameStart={CHECKBOX_LAST_GAME}
    ShowTitle={CHECKBOX_TITLE}

    [APP]
    FirstRun={FIRST_RUN}
    """;

    public void SaveConfig()
    {
        using StreamWriter streamWriter = new(FilePath, false);

        streamWriter.Write(GetConfigAsString());
        streamWriter.Close();
    }

    public static bool CheckConfig() =>
        File.Exists(FilePath);

    public static async Task<MainConfig> ReadConfig()
    {
        IniData ParsedData;
        using StreamReader StreamReader = new(File.OpenRead(FilePath));

        string ConfigData = await StreamReader.ReadToEndAsync();
        StreamReader.Close();

        try { ParsedData = ParsedData = new IniDataParser().Parse(ConfigData); }
        catch(IniParser.Exceptions.ParsingException x)
        {
            ERROR("CONFIG ERROR", x.Message,  MessageBoxButton.OK, MessageBoxImage.Error);
            return new();
        }
    
        try
        {
            return new()
            {
                GI_DIR = ParsedData["DIRECTORIES"][0],
                HSR_DIR = ParsedData["DIRECTORIES"][1],
                HI3_DIR = ParsedData["DIRECTORIES"][2],
                ZZZ_DIR = ParsedData["DIRECTORIES"][3],

                LAST_GAME = ParsedData["SETTINGS"][0],
                CHECKBOX_EXIT_TRAY = ParsedData["SETTINGS"][1],
                CHECKBOX_BACKGROUND = ParsedData["SETTINGS"][2],
                CHECKBOX_LAST_GAME = ParsedData["SETTINGS"][3],
                CHECKBOX_TITLE = ParsedData["SETTINGS"][4],

                FIRST_RUN = ParsedData["APP"][0]
            };
        }
        catch(Exception x)
        {
            ERROR("CONFIG ERROR", x.Message, MessageBoxButton.OK, MessageBoxImage.Error);
            return new();
        }
    }

    public static void CreateConfig()
    {
        MainConfig mainConfig = new()
        {
            LAST_GAME = 0,
            CHECKBOX_EXIT_TRAY = true,
            CHECKBOX_BACKGROUND = true,
            CHECKBOX_LAST_GAME = true,
            CHECKBOX_TITLE = true,
            FIRST_RUN = false
        };

        using StreamWriter streamWriter = new(File.Create(FilePath));

        streamWriter.Write(mainConfig.GetConfigAsString());
        streamWriter.Close();
    }

    static void ERROR(string Title, string Message, MessageBoxButton button, MessageBoxImage Icon)
    {
        HoyoWindow.Hide();
        HoyoWindow.ShowInTaskbar = false;
        MessageBox.Show(Message, Title, button, Icon);
        Environment.Exit(13);
    }

}
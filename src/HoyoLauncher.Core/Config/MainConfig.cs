namespace HoyoLauncher.Core.Config;

public sealed class MainConfig
{
    static readonly string FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.ini");

    /// <summary> Genshin Impact Directory </summary>
    [DefaultValue("")]
    public string GI_DIR { get; set; }
    /// <summary> Honkai Star Rail Directory </summary>
    [DefaultValue("")]
    public string HSR_DIR { get; set; }
    /// <summary> Honkai Impact 3rd Directory </summary>
    [DefaultValue("")]
    public string HI3_DIR { get; set; }
    /// <summary> Zenless Zone Zero Directory </summary>
    [DefaultValue("")]
    public string ZZZ_DIR { get; set; }

    /// <summary> Last game after close </summary>
    [DefaultValue(0)]
    public short LAST_GAME { get; set; }

    [DefaultValue(1)]
    public short EXIT_MODE { get; set; }
    [DefaultValue(true)]
    public bool CHECKBOX_BACKGROUND { get; set; }
    [DefaultValue(true)]
    public bool CHECKBOX_LAST_GAME { get; set; }
    [DefaultValue(true)]
    public bool CHECKBOX_TITLE { get; set; }
    [DefaultValue(1.0D)]
    public double SCALING { get; set; }

    [DefaultValue(false)]
    public bool FIRST_RUN { get; set; }

    public object this[string MethodName]
    {
        get => GetType().GetProperty(MethodName).GetValue(this, null);
        set => GetType().GetProperty(MethodName).SetValue(this, value);
    }

    /// <summary>
    /// Check If the Config Exist on the current directory.
    /// </summary>
    public static bool IsConfigExist => File.Exists(FilePath);

    private static MainConfig instance = null;
    public static MainConfig Instance => instance ??= new();
    private MainConfig() { }

    /// <summary>
    /// Resets the Config values.
    /// </summary>
    public void Reset()
    {
        var MainConfigProperties = GetType().GetProperties();

        foreach(var property in MainConfigProperties)
            if (property.GetCustomAttributes(typeof(DefaultValueAttribute), true).Length > 0)
                property.SetValue(this, property.GetCustomAttribute<DefaultValueAttribute>().Value);
    }

    public override string ToString() =>
    $"""
    [DIRECTORIES]
    GenshinImpact_DIR={GI_DIR}
    HonkaiStarRail_DIR={HSR_DIR}
    HonkaiImpact_DIR={HI3_DIR}
    ZenlessZoneZero_DIR={ZZZ_DIR}

    [SETTINGS]
    LastGame={LAST_GAME}
    ExitMode={EXIT_MODE}
    ShowBackground={CHECKBOX_BACKGROUND}
    LastGameStart={CHECKBOX_LAST_GAME}
    ShowTitle={CHECKBOX_TITLE}
    WindowScale={SCALING}

    [APP]
    FirstRun={FIRST_RUN}
    """;

    /// <summary>
    /// Save the Data values on the Config File.
    /// </summary>
    public void SaveConfig()
    {
        using StreamWriter streamWriter = new(FilePath, false);

        streamWriter.Write(ToString());
        streamWriter.Close();
    }

    /// <summary>
    /// Reads the Config File.
    /// </summary>
    public static async Task<MainConfig> ReadConfig()
    {
        using StreamReader StreamReader = new(File.OpenRead(FilePath));

        string ConfigData = await StreamReader.ReadToEndAsync();
        StreamReader.Close();

        if(!IniDataParser.TryParse(ConfigData, out IniData ParsedData))
        {
            ERROR("CONFIG ERROR", "Error while parsing config!\rMake sure the values are correct!\rDelete the config to reset if problem re-occurs.");
            return null;
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
                EXIT_MODE = ParsedData["SETTINGS"][1],
                CHECKBOX_BACKGROUND = ParsedData["SETTINGS"][2],
                CHECKBOX_LAST_GAME = ParsedData["SETTINGS"][3],
                CHECKBOX_TITLE = ParsedData["SETTINGS"][4],
                SCALING = ParsedData["SETTINGS"][5],

                FIRST_RUN = ParsedData["APP"][0]
            };
        }
        catch(Exception x)
        {
            ERROR("CONFIG ERROR", x.Message);
            return null;
        }

    }

    /// <summary>
    /// Creates a Config File
    /// </summary>
    public static void CreateConfig()
    {
        MainConfig mainConfig = new()
        {
            LAST_GAME = 0,
            EXIT_MODE = 1,
            CHECKBOX_BACKGROUND = true,
            CHECKBOX_LAST_GAME = true,
            CHECKBOX_TITLE = true,
            SCALING = 1.0D,
            FIRST_RUN = false
        };

        using StreamWriter streamWriter = new(File.Create(FilePath));

        streamWriter.Write(mainConfig.ToString());
        streamWriter.Close();
    }

    static void ERROR(string Title, string Message)
    {
        HoyoWindow.Hide();
        HoyoWindow.ShowInTaskbar = false;
        HoyoMessageBox.Show(Title, Message);
        Environment.Exit(13);
    }
}
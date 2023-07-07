// json names are case sensitive and this warning is irritating me.
#pragma warning disable IDE1006 

namespace HoyoLauncher.Core.API;

public sealed class ObjectList
{
    public string message { get; set; }
    public Data__ data { get; set; }

    // Shortcuts
    public string GetLatestVersion =>
        data.game.latest.version;

    public string GetBackgroundHash =>
        data.adv.bg_checksum;
    public string GetBackgroundLink =>
        data.adv.background;

    public string GetPreInstallation =>
        data.pre_download_game.diff[0].path;

    public bool IsLatestPathEmpty =>
        data.game.latest is { path: "" };
    public bool IsPreInstallAvailable =>
        data.pre_download_game is not null;
}

public sealed class Data__
{
    public Game__ game { get; set; }
    public PredownloadGame__ pre_download_game { get; set; }
    public Adv__ adv { get; set; }
}

public sealed class Game__
{
    public Latest__ latest { get; set; }
    public IList<Diffs__> diffs { get; set; }
}

public sealed class PredownloadGame__
{
    public IList<Diffs__> diff { get; set; }
}

public sealed class Latest__
{
    public string version { get; set; }
    public string path { get; set; }
}

public sealed class Diffs__
{
    public string path { get; set; }
}

public sealed class Adv__
{
    public string bg_checksum { get; set; }
    public string background { get; set; }
}
#pragma warning disable IDE1006
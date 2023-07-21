namespace HoyoLauncher.Core.API;

#pragma warning disable IDE1006 
#if DEBUG
public static class JsonDebug
{
    public readonly static string arrayed = """
    {
        "retcode": 0,
        "message": "OK",
        "data": {
            "game": {
                "latest": {
                    "name": "",
                    "version": "1.2.0",
                    "path": "https://autopatchos.starrails.com/client/download/20230710105424_73M0QhfClo2h6mt9/StarRail_1.2.0.zip"
                }
            },
            "pre_download_game": {
                "diffs": [
                    {
                        "name": "game_1.1.0_1.2.0_hdiff_SJqF73LhiQrsv2cU.zip",
                        "version": "1.1.0",
                        "path": "https://autopatchos.starrails.com/client/hkrpg_global/35/game_1.1.0_1.2.0_hdiff_SJqF73LhiQrsv2cU.zip"
                    }
                ]
            }
        }
    }
    """;
    public readonly static string normal = """
    {
        "retcode": 0,
        "message": "OK",
        "data": {
            "game": {
                "latest": {
                    "name": "",
                    "version": "1.2.0",
                    "path": "https://autopatchos.starrails.com/client/download/20230710105424_73M0QhfClo2h6mt9/StarRail_1.2.0.zip"
                }
            },
            "pre_download_game": {
                "latest": {
                    "name": "game_1.1.0_1.2.0_hdiff_SJqF73LhiQrsv2cU.zip",
                    "version": "1.1.0",
                    "path": "https://autopatchos.starrails.com/client/hkrpg_global/35/game_1.1.0_1.2.0_hdiff_SJqF73LhiQrsv2cU.zip"
                }
            }
        }
    }
    """;
    public readonly static string none = """
    {
        "retcode": 0,
        "message": "OK",
        "data": {
            "game": {
                "latest": {
                    "name": "",
                    "version": "1.2.0",
                    "path": "https://autopatchos.starrails.com/client/download/20230710105424_73M0QhfClo2h6mt9/StarRail_1.2.0.zip"
                }
            },
            "pre_download_game": null
        }
    }
    """;

    public static void TestJson()
    {
        ObjectList test1 = JsonSerializer.Deserialize<ObjectList>(arrayed);
        ObjectList test2 = JsonSerializer.Deserialize<ObjectList>(normal);
        ObjectList test3 = JsonSerializer.Deserialize<ObjectList>(none);

        Debug.WriteLineIf(test1.IsPreInstallAvailable,$$"""
        JSON DEBUG
        {
            Type                    :   arrayed
            PreInstallAvailability  :   true
            Link                    :   {{test1.GetPreInstallation}}
        }
        """);
        Debug.WriteLineIf(test2.IsPreInstallAvailable,$$"""
        JSON DEBUG
        {
            Type                    :   normal
            PreInstallAvailability  :   true
            Link                    :   {{test2.GetPreInstallation}}
        }
        """);
        // If the Link was "test3.GetPreInstallation", it will throw an error as
        // the GetPreInstallation() will fail. Thats why there's IsPreInstallAvailable()
        Debug.WriteLineIf(!test3.IsPreInstallAvailable,$$"""
        JSON DEBUG
        {
            Type                    :   none
            PreInstallAvailability  :   false
            Link                    :   null
        }
        """);
    }
}
#endif
#pragma warning restore IDE1006
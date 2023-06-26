namespace HoyoLauncher.HoyoLauncherSettings;

public partial class HoyoSettings
{
    void GithubHome(object s, RoutedEventArgs e) =>
        HoyoMain.ProcessStart("https://github.com/IchimakiKasura");
    void GithubLicense(object s, RoutedEventArgs e) =>
        HoyoMain.ProcessStart("https://github.com/IchimakiKasura/CeeleLauncher/blob/master/LICENSE");
    void GithubProject(object s, RoutedEventArgs e) =>
        HoyoMain.ProcessStart("https://github.com/IchimakiKasura/CeeleLauncher");
    void GithubPR(object s, RoutedEventArgs e) =>
        HoyoMain.ProcessStart("https://github.com/IchimakiKasura/CeeleLauncher/pulls");
    void GithubIssue(object s, RoutedEventArgs e) =>
        HoyoMain.ProcessStart("https://github.com/IchimakiKasura/CeeleLauncher/issues");

    void MihoyoPage(object s, RoutedEventArgs e) =>
        HoyoMain.ProcessStart("https://www.mihoyo.com/en/");
    void HoyoversePage(object s, RoutedEventArgs e) =>
        HoyoMain.ProcessStart("https://www.hoyoverse.com/en-us/");

    void GithubIconButton(object s, MouseButtonEventArgs e) =>
        HoyoMain.ProcessStart("https://github.com/IchimakiKasura/CeeleLauncher");
}
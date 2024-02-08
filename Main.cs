namespace HiddenQol;

internal class Main : MelonMod
{
    public override void OnInitializeMelon()
    {
        LoggerInstance.Msg("HiddenQol is loaded!");
        Load();
    }

    public override void OnDeinitializeMelon() =>
        File.WriteAllText(Path.Combine("UserData", "HiddenQol.cfg"), TomletMain.TomlStringFrom(Setting));
}
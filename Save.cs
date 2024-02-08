using Tomlet.Attributes;

namespace HiddenQol;

internal static class Save
{
    internal static Data Setting { get; private set; } = new(true);

    internal static void Load()
    {
        if (!File.Exists(Path.Combine("UserData", $"{Name}.cfg")))
        {
            var defaultConfig = TomletMain.TomlStringFrom(Setting);
            File.WriteAllText(Path.Combine("UserData", $"{Name}.cfg"), defaultConfig);
        }

        var data = File.ReadAllText(Path.Combine("UserData", $"{Name}.cfg"));
        Setting = TomletMain.To<Data>(data);
    }
}

public class Data
{
    [TomlPrecedingComment("Whether the All Hidden Mode checkbox is enabled")]
    internal bool QolEnabled { get; set; }

    public Data()
    {
    }

    internal Data(bool qolEnabled) => QolEnabled = qolEnabled;
}
using Il2CppBmsPair = Il2CppSystem.Collections.Generic.KeyValuePair<string, Il2Cpp.SpecialSongManager.HideBmsInfo>;

namespace HiddenQol;

public static class Extension
{
    public static void Deconstruct(this Il2CppBmsPair keyValuePair, out string key, out SpecialSongManager.HideBmsInfo value)
    {
        key = keyValuePair.Key;
        value = keyValuePair.Value;
    }
}
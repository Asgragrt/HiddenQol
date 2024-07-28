using HarmonyLib;
using Il2CppAssets.Scripts.Database;

namespace HiddenQol.Patches;

[HarmonyPatch(typeof(SpecialSongManager), nameof(SpecialSongManager.HideBmsCheck))]
internal static class SpecialSongManagerPatch
{
    private static void Prefix(MusicInfo selectedMusic, ref int selectedDifficulty)
    {
        GlobalDataBase.s_DbUISpecial.isBarrageMode = selectedDifficulty switch
        {
            2 when Setting.QolEnabled => true,
            3 when Setting.QolEnabled => false,
            _ => GlobalDataBase.s_DbUISpecial.isBarrageMode
        };
    }
}

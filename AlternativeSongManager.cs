using Il2CppAssets.Scripts.Database;
using Il2CppAssets.Scripts.PeroTools.Commons;

namespace HiddenQol;

internal static class AlternativeSongManager
{
    // Charts with alternative songs :D
    private static readonly List<string> Uids = ["51-4", "21-2"];

    internal static void ActivateAlternatives()
    {
        foreach (var uid in Uids)
        {
            var musicInfo = GlobalDataBase.dbMusicTag.GetMusicInfoFromAll(uid);
            musicInfo.AddMaskValue("music", musicInfo.music + "2");
            musicInfo.AddMaskValue("demo", musicInfo.demo + "2");
            Singleton<SpecialSongManager>.instance.m_IsInvokeHideDic[uid] = true;
        }
    }

    internal static void DeactivateAlternatives()
    {
        foreach (var uid in Uids)
        {
            var musicInfo = GlobalDataBase.dbMusicTag.GetMusicInfoFromAll(uid);
            musicInfo.ClearMaskValue();
            Singleton<SpecialSongManager>.instance.m_IsInvokeHideDic[uid] = false;
        }
    }
}

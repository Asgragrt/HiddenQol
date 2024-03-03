using Il2CppAssets.Scripts.Database;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppPeroPeroGames.GlobalDefines;

namespace HiddenQol
{
    internal static class YumeOuManager
    {
        private static MusicInfo HiddenOuSong { get; set; }
        private static MusicInfo BaseOuSong { get; set; }

        private static void GetInfo()
        {
            HiddenOuSong ??= GlobalDataBase.dbMusicTag.GetMusicInfoFromAll("0-53");
            BaseOuSong ??= GlobalDataBase.dbMusicTag.GetMusicInfoFromAll("0-54");
        }

        private static void SetBaseAsHidden(bool useHidden)
        {
            if (BaseOuSong == null || HiddenOuSong == null) return;

            var tempInfo = BaseOuSong;
            if (useHidden) tempInfo = HiddenOuSong;

            GlobalDataBase.dbMusicTag.SetMusicInfo(BaseOuSong.uid, tempInfo);
            GlobalDataBase.dbMusicTag.SetMusicInfo(HiddenOuSong.uid, tempInfo);
        }

        private static void RemoveFromCulling()
        {
            if (!DBMusicTagDefine.s_CullingMusicUids.Contains("0-53")) return;

            var newCullingArray = new Il2CppStringArray(DBMusicTagDefine.s_CullingMusicUids.Length - 1);
            for (var i = 0; i < DBMusicTagDefine.s_CullingMusicUids.Length; i++)
            {
                if (DBMusicTagDefine.s_CullingMusicUids[i].Equals("0-53")) continue;
                newCullingArray[i] = DBMusicTagDefine.s_CullingMusicUids[i];
            }
            DBMusicTagDefine.s_CullingMusicUids = newCullingArray;
        }

        private static void AddToCulling()
        {
            if (DBMusicTagDefine.s_CullingMusicUids.Contains("0-53")) return;

            var newCullingArray = new Il2CppStringArray(DBMusicTagDefine.s_CullingMusicUids.Length + 1);
            for (var i = 0; i < DBMusicTagDefine.s_CullingMusicUids.Length; i++)
            {
                newCullingArray[i] = DBMusicTagDefine.s_CullingMusicUids[i];
            }
            newCullingArray[^1] = "0-53";
            DBMusicTagDefine.s_CullingMusicUids = newCullingArray;
        }

        internal static void ActivateYume()
        {
            GetInfo();
            RemoveFromCulling();
            SetBaseAsHidden(true);
        }

        internal static void DisableYume()
        {
            AddToCulling();
            SetBaseAsHidden(false);
        }
    }
}

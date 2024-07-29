using Il2CppAssets.Scripts.Database;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppPeroPeroGames.GlobalDefines;

namespace HiddenQol.Managers;

internal static class SpecialMusicManager
{
    internal static bool IsInitialized { get; private set; } = false;
    internal static List<SpecialMusic> SpecialMusics { get; private set; }

    internal static void ActivateSpecials()
    {
        foreach (var specialMusic in SpecialMusics)
        {
            specialMusic.Activate();
        }
    }

    internal static void DeactivateSpecials()
    {
        foreach (var specialMusic in SpecialMusics)
        {
            specialMusic.Deactivate();
        }
    }

    internal static void Init()
    {
        if (IsInitialized)
            return;

        IsInitialized = true;

        SpecialMusics =
        [
            new SpecialMusic("0-54", "0-53"), // Yume Ou
            new SpecialMusic("0-56", "0-55"), // Echo over you
            new SpecialMusic("33-4", "33-12", replaceCover: true, replaceDemo: true), // Chaos
            new SpecialMusic("39-0", "39-8", replaceCover: true), // Sea-Saw
        ];
    }

    internal static bool IsSpecial(string uid)
    {
        return SpecialMusics?.Any(m => m?.HiddenUid.Equals(uid) ?? false) ?? false;
    }

    internal class SpecialMusic
    {
        private static readonly DBMusicTag musicTag = GlobalDataBase.dbMusicTag;

        internal SpecialMusic(
            string baseUid,
            string hiddenUid,
            bool replaceCover = false,
            bool replaceDemo = false
        )
        {
            BaseInfo = musicTag.GetMusicInfoFromAll(baseUid);
            HiddenInfo = musicTag.GetMusicInfoFromAll(hiddenUid);

            BaseUid = BaseInfo.uid;
            HiddenUid = HiddenInfo.uid;

            // * Set hidden cover to base to avoid empty covers :D
            if (replaceCover)
            {
                HiddenInfo.cover = BaseInfo.coverName;
                musicTag.SetMusicInfo(HiddenUid, HiddenInfo);
            }

            // * Replace demo song if not available
            if (replaceDemo)
            {
                HiddenInfo.demo = BaseInfo.demo;
                musicTag.SetMusicInfo(HiddenUid, HiddenInfo);
            }
        }

        internal MusicInfo BaseInfo { get; init; }
        internal string BaseUid { get; init; }
        internal MusicInfo HiddenInfo { get; init; }
        internal string HiddenUid { get; init; }

        internal void Activate()
        {
            CullingRemove(HiddenUid);
            SetBaseAsHidden(true);
        }

        internal void Deactivate()
        {
            CullingAdd(HiddenUid);
            SetBaseAsHidden(false);
        }

        private static void CullingAdd(string uid)
        {
            var cullingUids = DBMusicTagDefine.s_CullingMusicUids;
            if (cullingUids.Contains(uid))
                return;

            var newCullingUids = new Il2CppStringArray(cullingUids.Length + 1);

            for (var i = 0; i < cullingUids.Length; i++)
            {
                newCullingUids[i] = cullingUids[i];
            }
            newCullingUids[^1] = uid;
            DBMusicTagDefine.s_CullingMusicUids = newCullingUids;
        }

        private static void CullingRemove(string uid)
        {
            var cullingUids = DBMusicTagDefine.s_CullingMusicUids;
            DBMusicTagDefine.s_CullingMusicUids = cullingUids.Where(x => x != uid).ToArray();
        }

        private void SetBaseAsHidden(bool useHidden)
        {
            //return;
            if (BaseInfo is null || HiddenInfo is null)
                return;

            var tempInfo = BaseInfo;
            if (useHidden)
            {
                tempInfo = HiddenInfo;
            }

            musicTag.SetMusicInfo(BaseUid, tempInfo);
            musicTag.SetMusicInfo(HiddenUid, tempInfo);
        }
    }
}

using HarmonyLib;
using Il2CppAssets.Scripts.Database;
using Il2CppAssets.Scripts.PeroTools.Commons;
using Object = UnityEngine.Object;

namespace HiddenQol.Patches;

[HarmonyPatch(typeof(PnlStage))]
internal static class PnlStagePatch
{
    [HarmonyPatch(nameof(PnlStage.Awake))]
    [HarmonyPostfix]
    private static void AwakePostfix(PnlStage __instance)
    {
        SpecialMusicManager.Init();
        Stage = __instance;

        if (Setting.QolEnabled)
        {
            ActivateAllHidden();
        }

        Stage.musicFancyScrollView.onItemIndexChange += new Action<int>(_ =>
        {
            var uid = GlobalDataBase.s_DbMusicTag.CurMusicInfo().uid;
            bool isInvoke = Singleton<SpecialSongManager>.instance.IsInvokeHideBms(uid);
            Melon<Main>.Logger.Msg($"{uid} - {isInvoke}");
        });

        var vSelect = __instance
            .transform.parent.parent.Find("Forward")
            ?.Find("PnlVolume")
            ?.gameObject;

        if (QolToggle != null || vSelect == null)
        {
            return;
        }

        QolToggle = Object.Instantiate(
            vSelect.transform.Find("LogoSetting").Find("Toggles").Find("TglOn").gameObject,
            GameObject.Find("Info").transform
        );
        SetupToggle();
    }

    [HarmonyPatch(nameof(PnlStage.OnEnable))]
    [HarmonyPrefix]
    private static void OnEnablePostfix()
    {
        if (!Setting.QolEnabled)
        {
            return;
        }
        DeactivateAllHidden();
        ActivateAllHidden();
    }
}

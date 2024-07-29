using HarmonyLib;
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

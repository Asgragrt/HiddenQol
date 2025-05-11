using HarmonyLib;
using HiddenQol.Managers;
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

        var originalTgl = GameObject
            .Find("UI/Forward/PnlVolume/VoiceSetContent/LogoSetting/Toggles/TglOn")
            ?.gameObject;

        if (QolToggle is not null || originalTgl is null)
        {
            return;
        }

        var infoTransform = GameObject.Find("UI/Standerd/PnlStage/StageUi/Info").transform;
        QolToggle = Object.Instantiate(originalTgl, infoTransform);
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

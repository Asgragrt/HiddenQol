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

        // if (true)
        // {
        //     __instance.musicFancyScrollView.onItemIndexChange += new Action<int>(i =>
        //     {
        //         Melon<Main>.Logger.Msg(
        //             GlobalDataBase.dbMusicTag.GetMusicInfoFromShowMusicUids(i)?.uid
        //         );
        //     });
        // }

        if (Setting.QolEnabled)
        {
            ActivateAllHidden();
        }

        if (QolToggle != null)
        {
            return;
        }

        var infoTransform = GameObject.Find("UI/Standerd/PnlStage/StageUi/Info/Bottom").transform;
        var originalTgl = GameObject
            .Find("UI/Forward/PnlVolume/VoiceSetContent/LogoSetting/Toggles/TglOn")
            ?.gameObject;

        if (originalTgl == null || infoTransform == null)
        {
            Melon<Main>.Logger.Warning(
                "Could not find the toggle game object or the parent transform."
            );
            return;
        }

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

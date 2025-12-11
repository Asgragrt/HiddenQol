using HarmonyLib;
using HiddenQol.Managers;
using Object = UnityEngine.Object;

namespace HiddenQol.Patches;

[HarmonyPatch(typeof(PnlStage))]
internal static class PnlStagePatch
{
    private static readonly List<string> ToggleParents =
    [
        "UI/Standerd/PnlStage/StageUi/Info/Bottom",
        "UI/Standerd/PnlStage/StageUi/Info"
    ];

    private static readonly List<string> TogglePaths =
    [
        "UI/Forward/PnlVolume/VoiceSetContent/LogoSetting/Toggles/TglOn",
        "UI/Forward/PnlDisplaySettingPC/Setting/Vsync/Toggles/TglOn",
        "UI/Forward/PnlPlaySetting/AreaAdvancedSetting/SettingElements/TglAdvancedJudge/TglsJudge/TglOn"
    ];

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

        var infoTransform =
            ToggleParents
                .Select(path => GameObject.Find(path))
                .FirstOrDefault(go => go != null)
                ?.transform ?? __instance.stageAchievementPercent?.transform;

        var originalTgl = TogglePaths
            .Select(path => GameObject.Find(path))
            .FirstOrDefault(go => go != null)
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

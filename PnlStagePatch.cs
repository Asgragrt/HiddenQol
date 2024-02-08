using HarmonyLib;
using Object = UnityEngine.Object;

namespace HiddenQol;

[HarmonyPatch(typeof(PnlStage), nameof(PnlStage.Awake))]
internal static class PnlStagePatch
{
    private static void Postfix(PnlStage __instance)
    {
        Stage = __instance;

        if (Setting.QolEnabled)
        {
            ActivateAllHidden();
        }

        GameObject vSelect = null;
        foreach (var @object in __instance.transform.parent.parent.Find("Forward"))
        {
            var transform = @object.Cast<Transform>();
            if (transform.name == "PnlVolume")
            {
                vSelect = transform.gameObject;
            }
        }

        if (QolToggle != null || vSelect == null)
        {
            return;
        }

        QolToggle = Object.Instantiate(vSelect.transform.Find("LogoSetting").Find("Toggles").Find("TglOn").gameObject, __instance.transform);
        SetupToggle();
    }
}
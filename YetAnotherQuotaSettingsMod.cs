using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace YetAnotherQuotaSettingsMod
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class YetAnotherQuotaSettingsMod : BaseUnityPlugin
    {
        public static YetAnotherQuotaSettingsMod Instance { get; private set; } = null!;
        internal new static ManualLogSource Logger { get; private set; } = null!;
        internal static Harmony? Harmony { get; set; }

        public static float baseIncrease;
        public static int deadlineDaysAmount;
        public static float increaseSteepness;
        public static string randomizerCurve = "";
        public static float randomizerMultiplier;
        public static int startingCredits;
        public static int startingQuota;

        private void Awake()
        {
            Logger = base.Logger;
            Instance = this;

            baseIncrease = Config.Bind<float>("General", "Base Increase", 100, "The base increase for quota calculation.").Value;
            deadlineDaysAmount = Config.Bind<int>("General", "Days Until Deadline", 3, "The amount of days until deadline of the quota.").Value;
            increaseSteepness = Config.Bind<float>("General", "Increase Steepness", 16, "The increase steepness for quota calculation.").Value;
            randomizerCurve = Config.Bind<string>("General", "Randomizer Curve", "", "The randomizer curve used to randomize the next quota roll. LC by default evaluates the curve from 0 to 1. Default LC curve min/max values are -0.503/0.503. You can use values beyond vanilla if desired so. Ex: 0,-0.5 ; 0.5,-0.5 ; 1,0.6 for a curve with first half flat at value -0.5 then linear increase to value 0.6 on other half. You can type in a single constant value as well. Ex: -0.1 Leave empty to use vanilla curve. You can also define in/out tangets and weights for each key like so: 0.4,0.2,1.1,2,0.4,0.6 (Check Keyframe declaration in unity documentation!)").Value;
            startingCredits = Config.Bind<int>("General", "Starting Credits", 60, "The amount of credits in the terminal at the start.").Value;
            startingQuota = Config.Bind<int>("General", "Starting Quota", 130, "The value of the first quota.").Value;

            Patch();

            Logger.LogInfo($"{MyPluginInfo.PLUGIN_GUID} v{MyPluginInfo.PLUGIN_VERSION} has loaded!");
        }

        internal static void Patch()
        {
            Harmony ??= new Harmony(MyPluginInfo.PLUGIN_GUID);

            Logger.LogDebug("Patching...");

            Harmony.PatchAll();

            Logger.LogDebug("Finished patching!");
        }

        internal static void Unpatch()
        {
            Logger.LogDebug("Unpatching...");

            Harmony?.UnpatchSelf();

            Logger.LogDebug("Finished unpatching!");
        }
    }
}

using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace YetAnotherQuotaSettingsMod.Patches
{
    [HarmonyPatch(typeof(TimeOfDay))]
    public class TimeOfDayPatch
    {
        [HarmonyPatch("Awake")]
        [HarmonyPostfix]
        private static void AwakePostfix(TimeOfDay __instance)
        {
            __instance.quotaVariables.startingQuota = YetAnotherQuotaSettingsMod.startingQuota;
            __instance.quotaVariables.deadlineDaysAmount = YetAnotherQuotaSettingsMod.deadlineDaysAmount;
            __instance.quotaVariables.increaseSteepness = YetAnotherQuotaSettingsMod.increaseSteepness;
            __instance.quotaVariables.startingCredits = YetAnotherQuotaSettingsMod.startingCredits;
            __instance.quotaVariables.startingQuota = YetAnotherQuotaSettingsMod.startingQuota;

            if (string.IsNullOrWhiteSpace(YetAnotherQuotaSettingsMod.randomizerCurve))
            {
                __instance.quotaVariables.randomizerCurve = CreateCurve(YetAnotherQuotaSettingsMod.randomizerCurve);
            }
        }

        private static AnimationCurve CreateCurve(string s)
        {
            string[] keys = s.Split(';');
            if (keys.Length == 1)
            {
                if (float.TryParse(keys[0], out float value))
                {
                    return AnimationCurve.Constant(0f, 1f, value);
                }
            }
            List<Keyframe> ks = new List<Keyframe>();
            foreach (string key in keys)
            {
                string[] parts = key.Split(",");
                if (parts.Length != 2) continue;

                string timeStr = parts[0].Trim();
                string valueStr = parts[1].Trim();

                if (float.TryParse(timeStr, out float time) && float.TryParse(valueStr, out float value))
                {
                    ks.Add(new Keyframe(time, value));
                }
            }
            return new AnimationCurve([.. ks]);
        }
    }
}

// SPDX-License-Identifier: MPL-2.0

using System;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace StudyRewardInsight
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    public sealed class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "nikich.graveyardkeeper.studyrewardinsight";
        public const string PluginName = "Study Reward Insight";
        public const string PluginVersion = "0.2.0";

        internal static ManualLogSource Log;

        private Harmony _harmony;

        private void Awake()
        {
            Log = Logger;

            try
            {
                GameApi.Bind();

                _harmony = new Harmony(PluginGuid);
                _harmony.Patch(
                    GameApi.GetTooltipDataMethod,
                    postfix: new HarmonyMethod(
                        typeof(RuntimePatches).GetMethod(
                            nameof(RuntimePatches.GetTooltipDataPostfix),
                            BindingFlags.Static | BindingFlags.Public)));

                Log.LogInfo(PluginName + " " + PluginVersion + " loaded.");
            }
            catch (Exception ex)
            {
                Log.LogError(PluginName + " failed to initialize: " + ex);
            }
        }

        private void OnDestroy()
        {
            if (_harmony != null)
                _harmony.UnpatchSelf();
        }
    }

    internal static class RuntimePatches
    {
        public static void GetTooltipDataPostfix(
            object __instance,
            object __0,
            bool __1,
            object __result)
        {
            try
            {
                StudyTooltip.Apply(
                    __instance,
                    __0,
                    __1,
                    __result);
            }
            catch (Exception ex)
            {
                Plugin.Log.LogWarning(
                    "Study tooltip preview failed safely; vanilla tooltip preserved where possible: "
                    + ex);
            }
        }
    }
}

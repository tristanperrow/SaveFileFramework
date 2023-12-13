using HarmonyLib;
using SaveFileFramework.Utils;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Reflection;
using System.Linq;
using UnityEngine;

namespace SaveFileFramework.Patches
{

    [HarmonyPatch(typeof(SaveDataGarden))]
    internal class SaveDataGarden_Patches
    {

        [HarmonyPatch(nameof(SaveDataGarden.LoadData))]
        [HarmonyPostfix]
        public static void LoadData_Postfix(SaveDataGarden __instance)
        {
            ModRegistry.LoadPluginVariables(__instance.es3File);
        }

        [HarmonyPatch("_SaveData")]
        [HarmonyPostfix]
        public static void _SaveData_Postfix(SaveDataGarden __instance)
        {
            ModRegistry.SavePluginVariables(__instance);
        }
    }
}
using UnityEngine;
using System;
using System.Collections.Generic;
using BepInEx;
using System.Runtime.CompilerServices;
using System.Reflection;

namespace SaveFileFramework.Utils
{
    public static class ModRegistry
    {
        private static Dictionary<string, PluginInfo> modInfoDictionary = new Dictionary<string, PluginInfo>();

        public static void RegisterMod(PluginInfo modInfo)
        {
            if (!modInfoDictionary.ContainsKey(modInfo.Metadata.GUID))
            {
                modInfoDictionary.Add(modInfo.Metadata.GUID, modInfo);
            }
            else
            {
                // Handle duplicate GUID (conflict resolution or logging)
                SaveFileFrameworkPlugin.Log.LogWarning(modInfo.Metadata.Name + " is already loaded.");
            }
        }

        public static bool ModExists(string guid)
        {
            return modInfoDictionary.ContainsKey(guid);
        }

        // Add other methods as needed
        public static void SavePluginVariables(ref ES3File es3File)
        {
            foreach (var modInfo in modInfoDictionary.Values)
            {
                MethodInfo saveMethod = modInfo.Instance.GetType().GetMethod("Save");
                if (saveMethod != null)
                {
                    // Invoke the Save method
                    saveMethod.Invoke(modInfo.Instance, new object[] { es3File });
                }
            }

            es3File.Sync();
        }

        public static void LoadPluginVariables(ES3File file)
        {
            foreach (var modInfo in ModRegistry.modInfoDictionary.Values)
            {
                // Check if the mod has a Load method
                MethodInfo loadMethod = modInfo.Instance.GetType().GetMethod("Load");
                if (loadMethod != null)
                {
                    // Invoke the Load method
                    loadMethod.Invoke(modInfo.Instance, new object[] {file});
                }
            }
        }

        public static string GetVariableString(BaseUnityPlugin plugin, string variableName)
        {
            return plugin.Info.Metadata.GUID.ToString() + "." + variableName;
        }
    }

}

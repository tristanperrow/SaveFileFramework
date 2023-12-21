using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using HarmonyLib.Tools;
using SaveFileFramework.Utils;
using UnityEngine;

namespace SaveFileFramework
{
    // TODO Review this file and update to your own requirements.

    [BepInPlugin(MyGUID, PluginName, VersionString)]
    public class SaveFileFrameworkPlugin : BaseUnityPlugin
    {
        // Mod specific details. MyGUID should be unique, and follow the reverse domain pattern
        // e.g.
        // com.mynameororg.pluginname
        // Version should be a valid version string.
        // e.g.
        // 1.0.0
        private const string MyGUID = "com.nanopoison.SaveFileFramework";
        private const string PluginName = "SaveFileFramework";
        private const string VersionString = "0.2.0";

        private static readonly Harmony Harmony = new Harmony(MyGUID);
        public static ManualLogSource Log = new ManualLogSource(PluginName);

        // variable strings
        private string moddedSaveString;

        private void Awake()
        {
            Log = Logger;
            PluginInfo info = Info;
            ModRegistry.RegisterMod(info);

            HarmonyFileLog.Enabled = true;
            Harmony.PatchAll();

            // set up strings
            moddedSaveString = ModRegistry.GetVariableString(this, "ModdedSave");
        }

        public void Save(ES3File file)
        {
            file.Save<bool>(moddedSaveString, true);
        }

        public void Load(ES3File file)
        {
            if (file.KeyExists(moddedSaveString))
            {
                bool isModded = file.Load<bool>(moddedSaveString);
                Log.LogInfo("Is modded? " + isModded);
            } 
            else
            {
                Log.LogInfo("Save file is not a modded save.");
            }
        }
    }
}

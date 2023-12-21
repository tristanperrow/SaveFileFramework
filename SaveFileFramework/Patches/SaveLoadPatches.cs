using HarmonyLib;
using SaveFileFramework.Utils;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Reflection;
using System.Linq;
using UnityEngine;
using System;

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
        [HarmonyPrefix]
        public static bool _SaveData_Prefix(SaveDataGarden __instance, bool _savingFromAutoSave, bool _forceNewFile)
        {
            DateTime now = DateTime.Now;
            string filePath;
            if ((_savingFromAutoSave && __instance.pauseUI.save_autoSaveNewFile == 1) || _forceNewFile)
            {
                string text = now.ToString("yyyy_MM_dd HH_mm_ss");
                filePath = string.Concat(new string[]
                {
            "SaveFile",
            (__instance.saveSlotIndex + 1).ToString(),
            " - ",
            text,
            ".es3"
                });
            }
            else
            {
                filePath = "SaveFile" + (__instance.saveSlotIndex + 1).ToString() + ".es3";
            }
            ES3File es3File = new ES3File(filePath);
            es3File.Save<int>("GameVersionType", 2);
            string value = now.ToString("yyyy-MM-dd HH:mm:ss");
            es3File.Save<string>("GameLastPlayDate", value);
            es3File.Save<float>("GameStats.instance.timeInIsland", GameStats.instance.timeInIsland);
            es3File.Save<int[]>("GameStats.instance.statsArray", GameStats.instance.statsArray);
            es3File.Save<int[]>("GameStats.instance.creaturesKilledInTotal", GameStats.instance.creaturesKilledInTotal);
            es3File.Save<bool[]>("GameStats.instance.achievementsUnlocked", GameStats.instance.achievementsUnlocked);
            es3File.Save<int>("PlayerGarden.instance.credits", PlayerGarden.instance.credits);
            es3File.Save<float>("PlayerGarden.instance.statBonusEnergy", PlayerGarden.instance.statBonusEnergy);
            es3File.Save<float>("PlayerGarden.instance.statBonusMaxEnergy", PlayerGarden.instance.statBonusMaxEnergy);
            es3File.Save<float>("PlayerGarden.instance.statMaxStamina", PlayerGarden.instance.statMaxStamina);
            es3File.Save<float>("PlayerGarden.instance.statCurrStamina", PlayerGarden.instance.statCurrStamina);
            HoldClickModeAtStart.instance.SaveData(es3File);
            int count = StatusManager.instance.currStatusTimerList.Count;
            int[] array = new int[count];
            float[] array2 = new float[count];
            for (int i = 0; i < StatusManager.instance.currStatusTimerList.Count; i++)
            {
                array[i] = StatusManager.instance.currStatusTimerList[i].itemInfo.itemID;
                array2[i] = StatusManager.instance.currStatusTimerList[i].timeToDestroy;
            }
            es3File.Save<int[]>("StatusManager.instance.statusEffectsIndexList", array);
            es3File.Save<float[]>("StatusManager.instance.statusEffectsDurationList", array2);
            es3File.Save<int[]>("InventoryManager.instance.itemsInInv", InventoryManager.instance.itemsInInv);
            es3File.Save<int[]>("InventoryManager.instance.itemesPickedInTotal", InventoryManager.instance.itemesPickedInTotal);
            es3File.Save<bool[]>("UpgradesData.instance.skillsUnlocked", UpgradesData.instance.skillsUnlocked);
            es3File.Save<int[]>("UpgradesData.instance.skillsTiers", UpgradesData.instance.skillsTiers);
            es3File.Save<List<int>>("UpgradesData.instance.blockedSkillsList", UpgradesData.instance.blockedSkillsList);
            es3File.Save<float>("DayNightCycle.instance.time", DayNightCycle.instance.time);
            es3File.Save<int>("DayNightCycle.instance.days", DayNightCycle.instance.days);
            es3File.Save<int>("DayNightCycle.instance.currWeather", DayNightCycle.instance.currWeather);
            es3File.Save<bool>("DayNightCycle.instance.dynamic", DayNightCycle.instance.dynamic);
            es3File.Save<int[]>("CraftManager.instance.craftedTimesList", CraftManager.instance.craftedTimesList);
            es3File.Save<int>("CraftManager.instance.totalCraftedItems", CraftManager.instance.totalCraftedItems);
            int[] value2 = BottlesManager.instance.notesAlreadyInInv.ToArray();
            es3File.Save<int[]>("BottlesManager.instance.notesAlreadyInInv", value2);
            int[] value3 = BottlesManager.instance.lettersToSpawnNext.ToArray();
            es3File.Save<int[]>("BottlesManager.instance.lettersToSpawnNext", value3);
            es3File.Save<bool>("weReachedTheFinal", __instance.weReachedTheFinal);
            es3File.Save<bool>("goodbye", __instance.goodbye);
            int[] array3 = new int[ArtifactsManager.instance.artifactsInUseList.Length];
            for (int j = 0; j < ArtifactsManager.instance.artifactsInUseList.Length; j++)
            {
                if (ArtifactsManager.instance.artifactsInUseList[j] != null)
                {
                    array3[j] = ArtifactsManager.instance.artifactsInUseList[j].itemID;
                }
                else
                {
                    array3[j] = -1;
                }
            }
            es3File.Save<int[]>("ArtifactsManager.instance.artifactsInUseList", array3);
            List<int> list = new List<int>();
            for (int k = 0; k < ToolsManager.instance.extraItemsInHand.Count; k++)
            {
                list.Add(ToolsManager.instance.extraItemsInHand[k].itemID);
            }
            int[] value4 = list.ToArray();
            es3File.Save<int[]>("ToolsManager.instance.extraItemsInHand", value4);
            es3File.Save<Vector3>("PlayerGarden.instance.transform.position", PlayerGarden.instance.transform.position);
            es3File.Save<float>("CameraController.currentAngleY", PlayerGarden.instance.controller.cameraController.currentAngleY);
            es3File.Save<float>("CameraController.currentAngleX", PlayerGarden.instance.controller.cameraController.currentAngleX);
            es3File.Save<int[]>("ResearchManager.instance.RecipesUnlocked", ResearchManager.instance.recipesUnlocked);
            es3File.Save<int>("IslandsManager.currIsleIndex", IslandsManager.instance.currIsleIndex);
            es3File.Save<List<int>>("IslandsManager.passiveIslands", IslandsManager.instance.PassiveIslands);
            es3File.Save<int>("IslandsManager.instance.maxBiomeIndex", IslandsManager.instance.maxBiomeIndex);
            es3File.Save<int>("ArchipelagoManager.instance.archipelagoIndex", ArchipelagoManager.instance.archipelagoIndex);
            es3File.Save<int[]>("MineManager.checkpointsArray", MineManager.instance.checkpointsArray);
            es3File.Save<int[]>("MineManager.chestsRewardsPicked", MineManager.instance.chestsRewardsPicked);
            Transform child = __instance.objectPoolerTrans.GetChild(2);
            int[] array4 = new int[child.childCount];
            int[] array5 = new int[child.childCount];
            Vector3[] array6 = new Vector3[child.childCount];
            for (int l = 0; l < child.childCount; l++)
            {
                if (child.GetChild(l).gameObject.activeSelf)
                {
                    ItemPrefab component = child.GetChild(l).GetComponent<ItemPrefab>();
                    array4[l] = component.itemInfo.itemID;
                    array5[l] = component.quantity;
                    array6[l] = component.transform.position;
                }
                else
                {
                    array5[l] = 0;
                }
            }
            es3File.Save<int[]>("itemsInFloorIds", array4);
            es3File.Save<int[]>("itemsInFloorQuantity", array5);
            es3File.Save<Vector3[]>("itemsInFloorPos", array6);
            es3File.Save<int>("DeathManager.instance.numberOfDeaths", DeathManager.instance.numberOfDeaths);
            for (int m = 0; m < __instance.IPG_List.Count; m++)
            {
                __instance.IPG_List[m].IsleParentGenerator.saveData_IPG.SaveData(es3File);
            }
            TasksManager.instance.SaveData(es3File);
            SummonsManager.instance.SaveData(es3File);
            es3File.Save<bool>("CaveManager.instance.inCave", CaveManager.instance.inCave);
            es3File.Save<int>("CaveManager.instance.caveBiomeIndex", CaveManager.instance.caveBiomeIndex);
            es3File.Save<int>("CaveManager.instance.caveDesignIndex", CaveManager.instance.caveDesignIndex);
            es3File.Save<Vector3>("CaveManager.instance.savedPosFromLastEntrance", CaveManager.instance.savedPosFromLastEntrance);
            CaveManager.instance.SaveData(es3File);

            // add in variables call
            ModRegistry.SavePluginVariables(ref es3File);

            es3File.Sync();

            return false;
        }
    }
}
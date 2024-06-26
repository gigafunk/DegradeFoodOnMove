﻿using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using ServerSync;

namespace DegradeOnMove
{
    [BepInPlugin(ModGUID, ModName, ModVersion)]
    public class DegradeOnMoveMod : BaseUnityPlugin
    {
        private const string ModName = "Degrade On Move";
        private const string ModVersion = "1.0.1";
        private const string ModGUID = "come.littleroom.dev";
        private static Harmony harmony = null!;
        ConfigSync configSync = new(ModGUID) 
            { DisplayName = ModName, CurrentVersion = ModVersion, MinimumRequiredVersion = ModVersion};
        internal static ConfigEntry<bool> ServerConfigLocked = null!;
        ConfigEntry<T> config<T>(string group, string name, T value, ConfigDescription description, bool synchronizedSetting = true)
        {
            ConfigEntry<T> configEntry = Config.Bind(group, name, value, description);

            SyncedConfigEntry<T> syncedConfigEntry = configSync.AddConfigEntry(configEntry);
            syncedConfigEntry.SynchronizedConfig = synchronizedSetting;

            return configEntry;
        }
        ConfigEntry<T> config<T>(string group, string name, T value, string description, bool synchronizedSetting = true) => config(group, name, value, new ConfigDescription(description), synchronizedSetting);

        internal static ConfigEntry<bool> UseMod;
        public void Awake()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            harmony = new(ModGUID);
            harmony.PatchAll(assembly);
            ServerConfigLocked = config("1 - General", "Lock Configuration", true, "If on, the configuration is locked and can be changed by server admins only.");
            configSync.AddLockingConfigEntry(ServerConfigLocked);

            UseMod = config("1 - General", "Use Mod", true, "Should the mod be used");
        }
    }
}

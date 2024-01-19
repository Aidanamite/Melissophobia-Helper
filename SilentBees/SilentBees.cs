using UnityEngine;
using HarmonyLib;
using FMODUnity;
using HMLLibrary;

public class SilentBees : Mod
{
    Harmony harmony;
    static public JsonModInfo modInfo;

    public void Start()
    {
        modInfo = modlistEntry.jsonmodinfo;
        harmony = new Harmony("com.aidanamite.SilentBees");
        harmony.PatchAll();
        Debug.Log(modInfo.name + " has been loaded!");
    }

    public void OnModUnload()
    {
        harmony.UnpatchAll();
        Debug.Log(modInfo.name + " has been unloaded!");
    }
}

[HarmonyPatch(typeof(BeeHive), "SetBeeAmbienceSoundActive")]
public class Patch_BeeHiveSound
{
    static void Prefix(ref bool value)
    {
        value = false;
    }
}

[HarmonyPatch(typeof(StudioEventEmitter), "Play")]
public class Patch_HoneycombSound
{
    static bool Prefix(ref StudioEventEmitter __instance)
    {
        PickupItem parent = __instance.GetComponentInParent<PickupItem>();
        if (parent != null && parent.PickupName == "Honeycomb")
            return false;
        if (__instance.Event == "event:/wildlife/beehive")
            return false;
        return true;
    }
}
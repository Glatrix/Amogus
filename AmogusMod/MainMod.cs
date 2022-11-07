using System;
using System.Runtime.InteropServices;
using MelonLoader;
using UnityEngine;
using HarmonyLib;
using Hazel;
using static AmogusMod.State;
using Il2CppSystem.Reflection;

namespace AmogusMod
{
    public class Main : MelonMod
    {
        public override void OnInitializeMelon()
        {
            HarmonyInstance.PatchAll();
        }

        public override void OnGUI()
        {
            windowRect0 = GUI.Window(0, windowRect0, (GUI.WindowFunction)DoMyWindow, "My Window");
        }

        void DoMyWindow(int windowID)
        {
            if (GUILayout.Button("Kill Self", null))
            {
                PlayerControl.LocalPlayer?.Die(DeathReason.Kill);
            }
            if (GUILayout.Button("Revive Self", null))
            {
                PlayerControl.LocalPlayer?.Revive();
            }
            if (GUILayout.Button("Fullbright", null))
            {
                PlayerControl.LocalPlayer.myLight.lightHits.Clear();
                PlayerControl.LocalPlayer.myLight.LightRadius = 50f;
            }
            if (GUILayout.Button("Unlock All", null))
            {
                foreach (var csmtc in HatManager.Instance.allPets) { csmtc.Free = true; }
                foreach (var csmtc in HatManager.Instance.allSkins) { csmtc.Free = true; }
                foreach (var csmtc in HatManager.Instance.allVisors) { csmtc.Free = true; }
                foreach (var csmtc in HatManager.Instance.allHats) { csmtc.Free = true; }
                foreach (var csmtc in HatManager.Instance.allNamePlates) { csmtc.Free = true; }
            }

            fullbright = GUILayout.Toggle(fullbright, "Fullbright", null);

            // Make the windows be draggable.
            GUI.DragWindow(new Rect(0, 0, 10000, 10000));
        }

        public override void OnUpdate()
        {
            if (fullbright)
            {
                PlayerControl.LocalPlayer.myLight.LightRadius = 50f;
                PlayerControl.LocalPlayer.myLight.lightHits.Clear();
            }
        }
    }


    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.FixedUpdate))]
    class PlayerControl_FixedUpdate
    {
        [HarmonyPrefix]
        public static void FixedUpdate(PlayerControl __instance)
        {
            if (__instance._cachedData?.RoleType == RoleTypes.Impostor)
            {
                __instance.cosmetics.nameText.color = Color.red;
            }
            else
            {
                __instance.cosmetics.nameText.color = Color.white;
            }
        }
    }

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.HandleRpc))]
    class PlayerControl_HandleRPC
    {
        [HarmonyPrefix]
        public static void HandleRPC(PlayerControl __instance,
            [HarmonyArgument(0)] byte callId,
            [HarmonyArgument(1)] MessageReader reader)
        {
            System.Console.WriteLine($"Call: {((RpcCalls)callId).ToString()} From: {__instance.cosmetics.nameText.m_text}");
        }
    }

}

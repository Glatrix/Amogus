using System;
using System.Runtime.InteropServices;
using MelonLoader;
using UnityEngine;
using HarmonyLib;

namespace AmogusMod
{
    public class Main : MelonMod
    {
        public override void OnInitializeMelon()
        {
            
        }

        public override void OnGUI()
        {
            if(GUI.Button(new Rect(50,50,200,20),"Kill Self"))
            {
                PlayerControl.LocalPlayer?.Die(DeathReason.Kill);
            }
            if (GUI.Button(new Rect(50, 75, 200, 20), "Unlock All"))
            {
                foreach (var csmtc in HatManager.Instance.allPets) { csmtc.Free = true; }
                foreach (var csmtc in HatManager.Instance.allSkins) { csmtc.Free = true; }
                foreach (var csmtc in HatManager.Instance.allVisors) { csmtc.Free = true; }
                foreach (var csmtc in HatManager.Instance.allHats) { csmtc.Free = true; }
                foreach (var csmtc in HatManager.Instance.allNamePlates) { csmtc.Free = true; }
            }
        }

        public override void OnUpdate()
        {
            
        }
    }


    class PlayerControl_FixedUpdate
    {
        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.FixedUpdate))]
        [HarmonyPrefix]
        public void FixedUpdate(PlayerControl __instance)
        {
            __instance?.Die(DeathReason.Kill);
        }
    }
}

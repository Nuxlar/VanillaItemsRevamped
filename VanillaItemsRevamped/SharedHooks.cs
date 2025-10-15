using R2API;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace VanillaItemsRevamped.SharedHooks
{
    public class OnCharacterDeath
    {
        public delegate void OnCharacterDeathInventory(GlobalEventManager self, DamageReport damageReport, CharacterBody attackerBody, Inventory attackerInventory, CharacterBody victimBody);
        public static OnCharacterDeathInventory OnCharacterDeathInventoryActions;

        public static void GlobalEventManager_OnCharacterDeath(On.RoR2.GlobalEventManager.orig_OnCharacterDeath orig, GlobalEventManager self, DamageReport damageReport)
        {
            orig(self, damageReport);

            if (!NetworkServer.active) return;
            CharacterBody attackerBody = damageReport.attackerBody;
            CharacterMaster attackerMaster = damageReport.attackerMaster;

            if (!attackerBody || !attackerMaster) return;

            CharacterBody victimBody = damageReport.victimBody;
            if (victimBody)
            {
                Inventory attackerInventory = attackerMaster ? attackerMaster.inventory : null;
                if (attackerInventory)
                {
                    OnCharacterDeathInventoryActions?.Invoke(self, damageReport, attackerBody, attackerInventory, victimBody);
                }
            }
        }
    }
}
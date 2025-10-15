using R2API;
using RoR2;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Networking;

namespace VanillaItemsRevamped.Items.White
{
    public class Warbanner
    {
        public static bool enabled = true;
        public static GameObject warbannerPrefab = Main.warbannerWard;

        public Warbanner()
        {
            if (!enabled) return;

            ItemsCore.ModifyItemDefActions += ModifyItem;
            RecalculateStatsAPI.GetStatCoefficients += AddWarbannerDamage;
            On.RoR2.HoldoutZoneController.OnEnable += SpawnHoldoutBanner;
            On.EntityStates.InfiniteTowerSafeWard.Active.OnEnter += SpawnSimulacrumBanner;
        }

        private void SpawnHoldoutBanner(On.RoR2.HoldoutZoneController.orig_OnEnable orig, HoldoutZoneController self)
        {
            orig(self);
            SpawnBanners();
        }

        private void SpawnSimulacrumBanner(On.EntityStates.InfiniteTowerSafeWard.Active.orig_OnEnter orig, EntityStates.InfiniteTowerSafeWard.Active self)
        {
            orig(self);
            SpawnBanners();
        }

        private static void ModifyItem()
        {
            HG.ArrayUtils.ArrayAppend(ref ItemsCore.changedItemPickups, RoR2Content.Items.WardOnLevel);
            HG.ArrayUtils.ArrayAppend(ref ItemsCore.changedItemDescs, RoR2Content.Items.WardOnLevel);
        }

        private void SpawnBanners()
        {
            ReadOnlyCollection<TeamComponent> teamMembers = TeamComponent.GetTeamMembers(TeamIndex.Player);
            for (int j = 0; j < teamMembers.Count; j++)
            {
                TeamComponent teamComponent = teamMembers[j];
                CharacterBody body = teamComponent.body;
                if (body)
                {
                    CharacterMaster master = teamComponent.body.master;
                    if (master)
                    {
                        int itemCount = master.inventory.GetItemCount(RoR2Content.Items.WardOnLevel);
                        if (itemCount > 0)
                        {
                            GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(warbannerPrefab, body.transform.position, Quaternion.identity);
                            gameObject.GetComponent<TeamFilter>().teamIndex = TeamIndex.Player;
                            gameObject.GetComponent<BuffWard>().Networkradius = 8f + 8f * itemCount;
                            NetworkServer.Spawn(gameObject);
                        }
                    }
                }
            }
        }

        private void AddWarbannerDamage(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (sender.HasBuff(RoR2Content.Buffs.Warbanner))
            {
                args.damageMultAdd += 0.15f;
            }
        }
    }
}
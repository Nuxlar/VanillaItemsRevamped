using RoR2;
using R2API;
using UnityEngine;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;

namespace VanillaItemsRevamped.Items.Green
{
    public class PredatoryInstincts
    {
        public static bool enabled = true;

        public PredatoryInstincts()
        {
            if (!enabled) return;

            ItemsCore.ModifyItemDefActions += ModifyItem;
            RecalculateStatsAPI.GetStatCoefficients += IncreaseCrit;
        }

        private static void IncreaseCrit(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (sender.inventory)
            {
                int itemCount = sender.inventory.GetItemCount(RoR2Content.Items.AttackSpeedOnCrit);
                if (itemCount > 0)
                {
                    args.critAdd += 5f * (itemCount - 1);
                }
            }
        }

        private static void ModifyItem()
        {
            HG.ArrayUtils.ArrayAppend(ref ItemsCore.changedItemPickups, RoR2Content.Items.AttackSpeedOnCrit);
            HG.ArrayUtils.ArrayAppend(ref ItemsCore.changedItemDescs, RoR2Content.Items.AttackSpeedOnCrit);
        }
    }
}
using RoR2;
using R2API;

namespace VanillaItemsRevamped.Items.Green
{
    public class HarvestersScythe
    {
        public static bool enabled = true;

        public HarvestersScythe()
        {
            if (!enabled) return;

            ItemsCore.ModifyItemDefActions += ModifyItem;
            RecalculateStatsAPI.GetStatCoefficients += IncreaseCrit;
        }

        private static void IncreaseCrit(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (sender.inventory)
            {
                int itemCount = sender.inventory.GetItemCount(RoR2Content.Items.HealOnCrit);
                if (itemCount > 0)
                {
                    args.critAdd += 5f * (itemCount - 1);
                }
            }
        }

        private static void ModifyItem()
        {
            HG.ArrayUtils.ArrayAppend(ref ItemsCore.changedItemPickups, RoR2Content.Items.HealOnCrit);
            HG.ArrayUtils.ArrayAppend(ref ItemsCore.changedItemDescs, RoR2Content.Items.HealOnCrit);
        }
    }
}
using MonoMod.Cil;
using RoR2;
using UnityEngine;

namespace VanillaItemsRevamped.Items.White
{
    public class StickyBomb
    {
        public static bool enabled = true;

        public StickyBomb()
        {
            if (!enabled) return;

            ItemsCore.ModifyItemDefActions += ModifyItem;
            IL.RoR2.GlobalEventManager.ProcessHitEnemy += IncreaseChance;
        }

        private static void IncreaseChance(ILContext il)
        {
            bool error = true;
            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(
                 x => x.MatchLdsfld(typeof(RoR2Content.Items), "StickyBomb")
                ))
            {
                if (c.TryGotoNext(
                x => x.MatchLdcR4(5)
                ))
                {
                    c.Next.Operand = 8f;
                    error = false;
                }
            }

            if (error)
            {
                Debug.LogError("VanillaItemsRevamped: StickyBomb IncreaseChance Hook failed");
            }
        }

        private static void ModifyItem()
        {
            HG.ArrayUtils.ArrayAppend(ref ItemsCore.changedItemPickups, RoR2Content.Items.StickyBomb);
            HG.ArrayUtils.ArrayAppend(ref ItemsCore.changedItemDescs, RoR2Content.Items.StickyBomb);
        }
    }
}
using MonoMod.Cil;
using RoR2;
using UnityEngine;

namespace VanillaItemsRevamped.Items.White
{
    public class LensMakers
    {
        public static bool enabled = true;

        public LensMakers()
        {
            if (!enabled) return;

            ItemsCore.ModifyItemDefActions += ModifyItem;
            IL.RoR2.CharacterBody.RecalculateStats += ChangeCritChance;
        }

        private static void ChangeCritChance(ILContext il)
        {
            bool error = true;
            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(
                 x => x.MatchLdsfld(typeof(RoR2Content.Items), "CritGlasses")
                ))
            {
                if (c.TryGotoNext(
                      x => x.MatchConvR4(),
                      x => x.MatchLdcR4(10)
                      ))
                {
                    c.Next.Operand = 15f;
                    error = false;
                }
            }
            if (error)
            {
                Debug.LogError("VanillaItemsRevamped: LensMakers ChangeCritChance Hook failed");
            }
        }

        private static void ModifyItem()
        {
            HG.ArrayUtils.ArrayAppend(ref ItemsCore.changedItemPickups, RoR2Content.Items.CritGlasses);
            HG.ArrayUtils.ArrayAppend(ref ItemsCore.changedItemDescs, RoR2Content.Items.CritGlasses);
        }
    }
}
using System;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using UnityEngine;

namespace VanillaItemsRevamped.Items.White
{
    public class StunGrenade
    {
        public static bool enabled = true;

        public StunGrenade()
        {
            if (!enabled) return;

            ItemsCore.ModifyItemDefActions += ModifyItem;
            IL.RoR2.SetStateOnHurt.OnTakeDamageServer += ChangeStunChance;
        }

        private static void ChangeStunChance(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(MoveType.After,
                 x => x.MatchCall(typeof(Util), "ConvertAmplificationPercentageIntoReductionPercentage")
                ))
            {
                c.Emit(OpCodes.Ldloc_3);    //ItemCount
                c.Emit(OpCodes.Ldarg_1);    //DamageReport
                c.EmitDelegate<Func<float, int, DamageReport, float>>((origChance, itemCount, damageReport) =>
                {
                    if (damageReport.damageInfo.procCoefficient != 0)
                        return itemCount * 5f;
                    else
                        return 0f;
                });
            }
            else
            {
                Debug.LogError("VanillaItemsRevamped: StunGrenade ChangeStunChance Hook failed");
            }
        }

        private static void ModifyItem()
        {
            HG.ArrayUtils.ArrayAppend(ref ItemsCore.changedItemPickups, RoR2Content.Items.StunChanceOnHit);
            HG.ArrayUtils.ArrayAppend(ref ItemsCore.changedItemDescs, RoR2Content.Items.StunChanceOnHit);
        }
    }
}
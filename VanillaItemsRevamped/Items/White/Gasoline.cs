using RoR2;
using R2API;
using UnityEngine;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;

namespace VanillaItemsRevamped.Items.White
{
    public class Gasoline
    {
        public static bool enabled = true;

        public Gasoline()
        {
            if (!enabled) return;

            ItemsCore.ModifyItemDefActions += ModifyItem;
            IL.RoR2.GlobalEventManager.ProcIgniteOnKill += TweakGasoline;
        }

        private static void TweakGasoline(ILContext il)
        {
            bool error = true;
            ILCursor c = new ILCursor(il);

            if (c.TryGotoNext(
            x => x.MatchLdcR4(8f)
                ))
            {
                // Reduce initial blast damage
                if (c.TryGotoNext(
                 x => x.MatchLdcR4(1.5f)
                ))
                {
                    c.Next.Operand = 0.75f;
                    error = false;
                }
            }

            if (error)
            {
                Debug.LogError("VanillaItemsRevamped: Gasoline TweakGasoline Hook failed");
            }
        }

        private static void ModifyItem()
        {
            HG.ArrayUtils.ArrayAppend(ref ItemsCore.changedItemPickups, RoR2Content.Items.IgniteOnKill);
            HG.ArrayUtils.ArrayAppend(ref ItemsCore.changedItemDescs, RoR2Content.Items.IgniteOnKill);
        }
    }
}
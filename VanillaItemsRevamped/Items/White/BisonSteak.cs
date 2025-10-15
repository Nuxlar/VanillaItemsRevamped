using RoR2;
using R2API;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using UnityEngine;

namespace VanillaItemsRevamped.Items.White
{
    public class BisonSteak
    {
        public static bool enabled = true;

        public BisonSteak()
        {
            if (!enabled) return;

            ItemsCore.ModifyItemDefActions += ModifyItem;
            RecalculateStatsAPI.GetStatCoefficients += IncreaseHealth;
            IL.RoR2.CharacterBody.RecalculateStats += RemoveVanillaSteak;
        }

        private static void ModifyItem()
        {
            HG.ArrayUtils.ArrayAppend(ref ItemsCore.changedItemPickups, RoR2Content.Items.FlatHealth);
            HG.ArrayUtils.ArrayAppend(ref ItemsCore.changedItemDescs, RoR2Content.Items.FlatHealth);
        }

        private static void IncreaseHealth(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (sender.inventory)
            {
                int itemCount = sender.inventory.GetItemCount(RoR2Content.Items.FlatHealth);
                if (itemCount > 0)
                {
                    args.healthMultAdd += 0.05f * itemCount;
                    args.baseHealthAdd += 10f * itemCount;
                }
            }
        }

        private static void RemoveVanillaSteak(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(
                 x => x.MatchLdsfld(typeof(RoR2Content.Items), "FlatHealth")
                ))
            {
                c.Remove();
                c.Emit<Main>(OpCodes.Ldsfld, nameof(Main.emptyItemDef));
            }
            else
            {
                Debug.LogError("VanillaItemsRevamped: BisonSteak RemoveVanillaSteak Hook failed");
            }
        }
    }
}
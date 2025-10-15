using RoR2;
using RoR2.Orbs;
using UnityEngine;
using Mono.Cecil.Cil;
using MonoMod.Cil;

namespace VanillaItemsRevamped.Items.White
{
    public class MonsterTooth
    {
        public static bool enabled = true;

        public MonsterTooth()
        {
            if (!enabled) return;

            ItemsCore.ModifyItemDefActions += ModifyItem;
            GlobalEventManager.onCharacterDeathGlobal += NewMoothOrb;
            IL.RoR2.GlobalEventManager.OnCharacterDeath += RemoveMoothOrb;
        }

        private static void ModifyItem()
        {
            HG.ArrayUtils.ArrayAppend(ref ItemsCore.changedItemPickups, RoR2Content.Items.Tooth);
            HG.ArrayUtils.ArrayAppend(ref ItemsCore.changedItemDescs, RoR2Content.Items.Tooth);
        }

        private void RemoveMoothOrb(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdsfld(typeof(RoR2Content.Items), "Tooth")))
            {
                c.Remove();
                c.Emit<Main>(OpCodes.Ldsfld, nameof(Main.emptyItemDef));
            }
        }

        private void NewMoothOrb(DamageReport damageReport)
        {
            if (!damageReport.victim || !damageReport.attacker || !damageReport.attackerBody) return;
            Inventory inventory = damageReport.attackerBody.inventory;
            if (!inventory) return;

            int itemCount = inventory.GetItemCount(RoR2Content.Items.Tooth);
            if (itemCount <= 0) return;
            Vector3 vector;
            if (damageReport.victim)
                vector = damageReport.victim.transform.position;
            else
                vector = Vector3.zero;
            HealOrb healOrb = new HealOrb();
            healOrb.origin = vector;
            healOrb.target = damageReport.attackerBody.mainHurtBox;
            healOrb.healValue = 8f + damageReport.attackerBody.maxHealth * (0.02f * itemCount);
            healOrb.duration = 0.75f;
            OrbManager.instance.AddOrb((Orb)healOrb);
        }

    }
}
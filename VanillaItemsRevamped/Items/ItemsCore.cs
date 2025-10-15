using RoR2;
using VanillaItemsRevamped.Items.Green;
using VanillaItemsRevamped.Items.Uncommon;
using VanillaItemsRevamped.Items.White;

namespace VanillaItemsRevamped.Items
{
    public class ItemsCore
    {
        public static bool whiteEnabled = true;
        public static bool greenEnabled = true;
        public static bool redEnabled = true;
        public static bool bossEnabled = true;
        public static bool voidEnabled = true;
        public static bool lunarEnabled = true;
        public static bool equipmentEnabled = true;

        public static ItemDef[] changedItemPickups = new ItemDef[0];
        public static ItemDef[] changedItemDescs = new ItemDef[0];

        public static EquipmentDef[] changedEquipPickups = new EquipmentDef[0];
        public static EquipmentDef[] changedEquipDescs = new EquipmentDef[0];

        public ItemsCore()
        {
            ModifyItemTokens();
            ModifyWhite();
            ModifyGreen();
            ModifyRed();
            ModifyVoid();
            ModifyBoss();
            ModifyLunar();
            ModifyEquipment();
        }

        private void ModifyWhite()
        {
            if (!whiteEnabled) return;

            new BisonSteak();
            new MonsterTooth();
            new Gasoline();
            new LensMakers();
            new StunGrenade();
            new StickyBomb();
            new Warbanner();
        }

        private void ModifyGreen()
        {
            if (!greenEnabled) return;

            new PredatoryInstincts();
            new HarvestersScythe();
            new Infusion();
        }

        private void ModifyRed()
        {
            if (!redEnabled) return;
        }

        private void ModifyVoid()
        {
            if (!voidEnabled) return;
        }

        private void ModifyBoss()
        {
            if (!bossEnabled) return;
        }

        private void ModifyLunar()
        {
            if (!lunarEnabled) return;
        }

        private void ModifyEquipment()
        {
            if (!equipmentEnabled) return;
        }

        private void ModifyItemTokens()
        {
            On.RoR2.ItemCatalog.Init += (orig) =>
            {
                orig();

                if (ModifyItemDefActions != null) ModifyItemDefActions.Invoke();

                foreach (ItemDef item in changedItemPickups)
                {
                    item.pickupToken += "_VANILLAITEMSREVAMPED";
                }
                foreach (ItemDef item in changedItemDescs)
                {
                    item.descriptionToken += "_VANILLAITEMSREVAMPED";
                }
                foreach (EquipmentDef item in changedEquipPickups)
                {
                    item.pickupToken += "_VANILLAITEMSREVAMPED";
                }
                foreach (EquipmentDef item in changedEquipDescs)
                {
                    item.descriptionToken += "_VANILLAITEMSREVAMPED";
                }
            };
        }

        public delegate void ModifyItemDef();
        public static ModifyItemDef ModifyItemDefActions;

        public static void ChangeEquipmentCooldown(EquipmentDef ed, float cooldown)
        {
            ed.cooldown = cooldown;
        }
    }
}
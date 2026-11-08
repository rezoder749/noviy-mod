using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using newmod.Content.Items;

namespace newmod.Content.Items
{
    public class CustomSword : ModItem
    {
        public int attackType = 0;

        public override void SetDefaults()
        {
            Item.width = 46;
            Item.height = 48;

            Item.damage = 65;
            Item.DamageType = DamageClass.Melee;
            Item.knockBack = 6f;
            Item.crit = 6;

            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = true;

            Item.shoot = ModContent.ProjectileType<CustomSwordProjectile>();
            Item.shootSpeed = 1f;

            Item.value = Item.buyPrice(gold: 3);
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item1;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, 
            Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, 
                player.whoAmI, attackType);

            attackType = (attackType + 1) % 2;

            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.HellstoneBar, 15)
                .AddIngredient(ItemID.SoulofFright, 8)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}			

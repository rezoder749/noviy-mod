using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;

namespace newmod.Content.Items
{
    public class CustomSwordProjectile : ModProjectile
    {
        private const float SwingRange = 1.6f * MathHelper.Pi;
        private const float SpinRange = 3.2f * MathHelper.Pi;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.HeldProjDoesNotUsePlayerGfxOffY[Type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 46;
            Projectile.height = 48;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.ownerHitCheck = true;
            Projectile.extraUpdates = 1;
        }

        public override string Texture => "newmod/Content/Items/TutorialSword";

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (player.dead || !player.active || player.HeldItem.type != ModContent.ItemType<CustomSword>())
            {
                Projectile.Kill();
                return;
            }

            player.heldProj = Projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;

            float duration = 30f;
            if (Projectile.ai[0] == 1)
                duration = 45f;

            if (Projectile.localAI[0] == 0)
            {
                Projectile.localAI[0] = 1;
                Projectile.timeLeft = (int)duration;
            }

            float progress = 1f - ((float)Projectile.timeLeft / duration);
            float direction = player.direction;

            float angle;
            if (Projectile.ai[0] == 0)
            {
                angle = MathHelper.Lerp(-SwingRange / 2, SwingRange / 2, progress) * direction;
            }
            else
            {
                angle = MathHelper.Lerp(0, SpinRange, progress) * direction;
            }

            Vector2 offset = new Vector2(0, -60f).RotatedBy(angle + player.fullRotation);
            Projectile.Center = player.MountedCenter + offset;
            Projectile.rotation = angle + MathHelper.PiOver4 * direction + player.fullRotation;

            player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation - MathHelper.PiOver2);
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Player player = Main.player[Projectile.owner];
            float collisionPoint = 0f;

            return Collision.CheckAABBvLineCollision(
                targetHitbox.TopLeft(), targetHitbox.Size(),
                player.MountedCenter,
                Projectile.Center,
                20f,
                ref collisionPoint);
        }
    }
}

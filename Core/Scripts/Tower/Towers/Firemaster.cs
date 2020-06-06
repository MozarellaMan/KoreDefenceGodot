using System;
using Godot;
using KoreDefenceGodot.Core.Scripts.Enemy;
using KoreDefenceGodot.Core.Scripts.Engine.Game;

namespace KoreDefenceGodot.Core.Scripts.Tower.Towers
{
    public class Firemaster : BaseTower
    {
        public override void _Ready()
        {
            TowerType = TowerType.Firemaster;
            ProjectileSpeed = 15;
            base._Ready();
        }

        public override void Shoot(BaseEnemy enemy, float delta)
        {
            _shootTimeCounter += delta;
            if (!(_shootTimeCounter > _firePeriod)) return;
            _hasShot = true;
            // projectile exists and is instantiated
            if (!(_projectileResource.Instance() is Projectile projectile)) return;
            if (!(_projectileResource.Instance() is Projectile projectile2)) return;

            // adding projectiles (i.e. instancing) to the screen, xOffset from the gun

            AddChild(projectile);
            AddChild(projectile2);
            projectile.Position = new Vector2(TowerGun.Position.x + 8, TowerGun.Position.y + 5);
            projectile2.Position = new Vector2(TowerGun.Position.x - 8, TowerGun.Position.y - 5);


            SetupProjectile(projectile, enemy, 8, 5);
            SetupProjectile(projectile2, enemy, 8, 5, -1);
            _shootTimeCounter -= _firePeriod;
        }

        /// <summary>
        ///     Special function for the firemaster tower due to it shooting two parallel projectiles
        /// </summary>
        /// <param name="projectile">the projectile</param>
        /// <param name="enemy">the target enemy</param>
        /// <param name="xOffset">the x offset for the projectiles</param>
        /// <param name="yOffset">the y offset for the projectiles</param>
        /// <param name="isSecond">
        ///     used to set the angle of projectile depending on what side of the offset it is on (negative or
        ///     positive)
        /// </param>
        private void SetupProjectile(Projectile projectile, Node2D enemy, float xOffset, float yOffset,
            int isSecond = 1)
        {
            projectile.Setup(_projectileCollateral);
            projectile.Source = this;
            projectile.Damage = _attackDamage;

            // get direction and magnitude
            var direction =
                Position.DirectionTo(new Vector2(enemy.GlobalPosition.x + xOffset, enemy.GlobalPosition.y));

            var mag = (float) Math.Sqrt(direction.x * direction.x + direction.y * direction.y);


            // sets velocity of projectile
            projectile.SetVelocity(
                new Vector2(direction.x / mag * ProjectileSpeed, direction.y / mag * ProjectileSpeed));


            // turns sprite to look at enemy
            projectile.LookAt(new Vector2(enemy.Position.x + xOffset * isSecond,
                enemy.Position.y + yOffset * isSecond));
        }
    }
}
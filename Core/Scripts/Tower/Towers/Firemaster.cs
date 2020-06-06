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
            ProjectileSpeed = 20;
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

            // adding projectiles (i.e. instancing) to the screen, offset from the gun
            projectile.Position = new Vector2(TowerGun.Position.x + 10, TowerGun.Position.y);
            projectile2.Position = new Vector2(TowerGun.Position.x - 10, TowerGun.Position.y);
            AddChild(projectile);
            AddChild(projectile2);

            var projectileOffset = projectile.Position.DistanceTo(projectile2.Position) / 2;

            SetupProjectile(projectile, enemy, 10);
            SetupProjectile(projectile2, enemy, -10);
            _shootTimeCounter -= _firePeriod;
        }

        private void SetupProjectile(Projectile projectile, Node2D enemy, float offset)
        {
            projectile.Setup(3);
            projectile.Source = this;
            projectile.Damage = _attackDamage;
            //projectile.SetVelocity(this, enemy.GlobalPosition, ProjectileSpeed);

            //var direction = new Vector2(enemy.GlobalPosition.x, enemy.GlobalPosition.y) - Position;


            // offset corresponds to the offset the projectiles are from each other
            // get direction and magnitude
            var direction =
                Position.DirectionTo(new Vector2(enemy.GlobalPosition.x + offset, enemy.GlobalPosition.y + offset));

            var mag = (float) Math.Sqrt(direction.x * direction.x + direction.y * direction.y);


            // sets velocity of projectile
            projectile.SetVelocity(
                new Vector2(direction.x / mag * ProjectileSpeed, direction.y / mag * ProjectileSpeed));

            projectile.FlipSprite();

            // turns sprite to look at enemy
            projectile.LookAt(new Vector2(enemy.GlobalPosition.x + offset, enemy.GlobalPosition.y + offset));
        }
    }
}
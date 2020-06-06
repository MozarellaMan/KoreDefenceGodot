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
            AddChild(projectile);
            AddChild(projectile2);

            projectile.Position = new Vector2(TowerGun.Position.x + 10, TowerGun.Position.y);
            projectile2.Position = new Vector2(TowerGun.Position.x - 10, TowerGun.Position.y);

            SetupProjectile(projectile, enemy);
            SetupProjectile(projectile2, enemy);
            _shootTimeCounter -= _firePeriod;
        }

        private void SetupProjectile(Projectile projectile, Node2D enemy)
        {
            projectile.Setup(_projectileCollateral);
            projectile.Source = this;
            projectile.Damage = _attackDamage;
            //projectile.SetVelocity(this, enemy.GlobalPosition, ProjectileSpeed);

            var direction = enemy.GlobalPosition - Position;

            var mag = (float) Math.Sqrt(direction.x * direction.x + direction.y * direction.y);


            projectile.SetVelocity(
                new Vector2(direction.x / mag * ProjectileSpeed, direction.y / mag * ProjectileSpeed));


            projectile.FlipSprite();
            projectile.LookAt(enemy.GlobalPosition);
        }
    }
}
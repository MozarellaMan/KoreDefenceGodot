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

		public override void Shoot(BaseEnemy? enemy, float delta)
		{
			ShootTimeCounter += delta;

			if (enemy != null)
			{
				if (!(ShootTimeCounter > FirePeriod)) return;
				_hasShot = true;
				// projectile exists and is instantiated
				if (!(ProjectileResource.Instance() is Projectile projectile)) return;
				if (!(ProjectileResource.Instance() is Projectile projectile2)) return;

				// adding projectiles (i.e. instancing) to the screen, xOffset from the gun

				AddChild(projectile);
				AddChild(projectile2);
				projectile.Position = new Vector2(TowerGun.Position.x + 5, TowerGun.Position.y + 5);
				projectile2.Position = new Vector2(TowerGun.Position.x - 5, TowerGun.Position.y - 5);
				projectile.Rotation = Rotation;
				projectile2.Rotation = Rotation;


				SetupProjectile(projectile, enemy, 5, 5);
				SetupProjectile(projectile2, enemy, 5, 5, -1);
			}

			ShootTimeCounter -= FirePeriod;
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
			projectile.Setup(ProjectileCollateral);
			projectile.Lifetime = 0.2f;
			projectile.Source = this;
			projectile.Damage = AttackDamage;

			var enemyPos = enemy.Position + new Vector2(xOffset, xOffset);
			// get direction and magnitude
			var direction = Position.DirectionTo(enemyPos);

			var mag = (float) Math.Sqrt(direction.x * direction.x + direction.y * direction.y);


			// sets velocity of projectile
			projectile.SetVelocity(
				new Vector2(direction.x / mag * ProjectileSpeed, direction.y / mag * ProjectileSpeed));


			// turns sprite to look at enemy
			projectile.LookAt(new Vector2(enemy.Position.x + xOffset * isSecond,
				enemy.Position.y + xOffset * isSecond));
		}


		/// <summary>
		///     Makes a smaller bounding rectangle for the firemaster tower
		/// </summary>
		/// <returns> the bounding rectangle </returns>
		protected internal override Rect2 GetRect()
		{
			var rect = base.GetRect();

			// shrink rectangle to better match sprite on screen
			rect.Size /= 2;
			rect.Position += rect.Size / 2;

			return rect;
		}
	}
}

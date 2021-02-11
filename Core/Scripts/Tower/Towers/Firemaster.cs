using System;
using Godot;
using KoreDefenceGodot.Core.Scripts.Enemy;
using KoreDefenceGodot.Core.Scripts.Engine.Game;

namespace KoreDefenceGodot.Core.Scripts.Tower.Towers
{
	public abstract class Firemaster : BaseTower
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
				HasShot = true;
				// projectile exists and is instantiated
				if (!(ProjectileResource.Instance() is Projectile projectile)) return;
				if (!(ProjectileResource.Instance() is Projectile projectile2)) return;

				// adding projectiles (i.e. instancing) to the screen, xOffset from the gun

				AddChild(projectile);
				AddChild(projectile2);
				projectile.GlobalPosition = new Vector2(TowerGun.GlobalPosition.x + 5, TowerGun.GlobalPosition.y + 5);
				projectile2.GlobalPosition = new Vector2(TowerGun.GlobalPosition.x - 5, TowerGun.GlobalPosition.y - 5);
				projectile.GlobalRotation = GlobalRotation;
				projectile2.GlobalRotation = GlobalRotation;


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

			var enemyPos = enemy.GlobalPosition + new Vector2(xOffset, xOffset);
			// get direction and magnitude
			var direction = GlobalPosition.DirectionTo(enemyPos);

			var mag = (float) Math.Sqrt(direction.x * direction.x + direction.y * direction.y);


			// sets velocity of projectile
			projectile.SetVelocity(
				new Vector2(direction.x / mag * ProjectileSpeed, direction.y / mag * ProjectileSpeed));


			// turns sprite to look at enemy
			projectile.LookAt(new Vector2(enemy.GlobalPosition.x + xOffset * isSecond,
				enemy.GlobalPosition.y + xOffset * isSecond));
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

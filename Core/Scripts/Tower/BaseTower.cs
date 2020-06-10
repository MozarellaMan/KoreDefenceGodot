using System;
using System.Collections.Generic;
using Godot;
using KoreDefenceGodot.Core.Scripts.Enemy;
using KoreDefenceGodot.Core.Scripts.Engine.Game;
using KoreDefenceGodot.Core.Scripts.Engine.State;

namespace KoreDefenceGodot.Core.Scripts.Tower
{
	public abstract class BaseTower : Node2D
	{
		private const int TargetingSpeed = 20;
		private protected const int ProjectileSpeed = 10;

		// TODO : Tower upgrades
		// TODO : Tower projectile status effect

		private Vector2 _clickOffsetInTower;
		private protected bool _hasShot;
		private bool _isDeleted;

		private Sprite _towerBody;


		public Area2D AttackArea;
		public Color AttackColour = GameInfo.ValidColour;
		private protected int AttackDamage;
		public int AttackRadius;
		public BaseEnemy CurrentTarget;
		public Vector2 DragStart;
		private protected float FirePeriod;
		public CollisionShape2D PlayerCollision;
		private protected int ProjectileCollateral;
		private protected PackedScene ProjectileResource;
		public bool Purchased = true;
		private protected float ShootTimeCounter;
		public List<BaseEnemy> Targets;
		public AnimatedSprite TowerGun;
		public TowerType TowerType;


		// TODO : Tower type
		public NodeStateMachine<BaseTower, DefaultTowerState> TowerStateMachine { get; private set; }

		public override void _Ready()
		{
			AttackArea = GetNode<Area2D>("Area2D");
			TowerGun = GetNode<AnimatedSprite>("Gun");
			ProjectileResource = GD.Load<PackedScene>(TowerType.ProjectilePath);
			TowerStateMachine =
				new NodeStateMachine<BaseTower, DefaultTowerState>(this, DefaultTowerState.Idle,
					DefaultTowerState.Global);

			FirePeriod = 1f / TowerType.FireRate;
			AttackRadius = TowerType.AttackRadius;
			AttackDamage = TowerType.Damage;
			ProjectileCollateral = TowerType.Collateral;
			_hasShot = false;
			Targets = new List<BaseEnemy>();
			PlayerCollision = GetNode("StaticBody2D").GetNode<CollisionShape2D>("CollisionShape2D");
		}

		public override void _PhysicsProcess(float delta)
		{
			TowerStateMachine.Update(delta);
		}

		public override void _Input(InputEvent @event)
		{
			TowerStateMachine.UpdateInput(@event);
		}

		public virtual void Shoot(BaseEnemy enemy, float delta)
		{
			ShootTimeCounter += delta;
			if (!(ShootTimeCounter > FirePeriod)) return;
			_hasShot = true;
			// projectile exists and is instantiated
			if (!(ProjectileResource.Instance() is Projectile projectile)) return;
			AddChild(projectile);
			projectile.Setup(ProjectileCollateral);
			projectile.Source = this;
			projectile.Damage = AttackDamage;
			projectile.SetVelocity(this, enemy.GlobalPosition, ProjectileSpeed);
			projectile.FlipSprite();
			projectile.LookAt(enemy.GlobalPosition);
			ShootTimeCounter -= FirePeriod;
		}

		public void TrackNextTarget(float delta)
		{
			if (CurrentTarget == null) return;

			var difference = Mathf.Rad2Deg(TowerGun.GetAngleTo(CurrentTarget.GlobalPosition)) + 90;

			TowerGun.GlobalRotationDegrees += difference * (TargetingSpeed * delta);
		}

		public override void _Draw()
		{
			// uncomment to test bounding rectangle
			//DrawRect(GetRect(), Colors.White);
			// GD.Print(ToGlobal(GetRect().Position));
			TowerStateMachine.Draw();
		}

		private void OnAreaEntered(Node body)
		{
			if (!body.IsInGroup("enemies")) return;
			if (body is BaseEnemy enemy) Targets.Add(enemy);
		}

		private void OnBodyExit(Node body)
		{
			if (!body.IsInGroup("enemies")) return;
			if (!(body is BaseEnemy enemy)) return;
			Targets.Remove(enemy);
		}

		public void PlayAttackAnimation()
		{
			TowerGun.Play("Attacking");
		}

		public void PlayIdleAnimation()
		{
			TowerGun.Play("Idle");
		}

		public void DragTo(Vector2 pos)
		{
			Position = pos - _clickOffsetInTower;
		}

		/// <summary>
		///     Get the tower gun sprite's bounding rectangle
		/// </summary>
		/// <returns> the rectangle </returns>
		protected virtual Rect2 GetRect()
		{
			return GameInfo.GetRect(TowerGun);
		}

		/// <summary>
		///     Checks if the tower can be placed at it's current Position value
		/// </summary>
		/// <returns> true if the tower can be placed at it's position </returns>
		public bool CanPlaceTower()
		{
			var notOnPath = !CollidesWithPath();
			// position is outside screen
			if (Position.x < 0 || Position.x > GetViewport().Size.x || Position.y < 0 ||
				Position.y > GetViewport().Size.y) return false;

			if (notOnPath)
			{
				// TODO : Check if tower collides with other towers + buildings
				// TODO : Check if tower collides with lava tiles
			}

			return notOnPath;
		}


		/// <summary>
		///     Tests for a collision between the tower and a game Path
		///     Simply finds the stretch of path and checks if the tower is within the bounds of the path stretch.
		///     Method should be scalable for both different paths and different tower sizes.
		/// </summary>
		/// <returns>true if colliding with path, false otherwise</returns>
		public bool CollidesWithPath()
		{
			var pathPoints = GameInfo.GamePath.PathPoints;
			for (var i = 0; i < pathPoints.Length / 2 - 1; i++)
			{
				var x1 = pathPoints[i, 0];
				var y1 = pathPoints[i, 1];
				var x2 = pathPoints[i + 1, 0];
				var y2 = pathPoints[i + 1, 1];


				if (x1 == x2) x2 += 40;

				if (y1 == y2)
				{
					y2 += 40;
					if (x1 < x2) x2 += 40;
					else x1 += 40;
				}


				if (CollidesWithBox(Math.Min(y1, y2), Math.Max(y1, y2),
					Math.Min(x1, x2), Math.Max(x1, x2)))
					return true;
			}

			return false;
		}

		public virtual void DrawAttackRadius()
		{
			DrawCircle(ToLocal(GlobalPosition), AttackRadius, AttackColour);
		}


		/// <summary>
		///     Determines whether a tower will collide with an arbitrary hitbox.
		/// </summary>
		/// <param name="upY">Top of hitbox in Y direction</param>
		/// <param name="downY">Bottom of hitbox in Y direction</param>
		/// <param name="leftX">Left of hitbox in X direction</param>
		/// <param name="rightX">Right of hitbox in X direction</param>
		/// <returns>A boolean describing whether the actor will collide with this box.</returns>
		private bool CollidesWithBox(int upY, int downY, int leftX, int rightX)
		{
			var bounds = GetRect();
			bounds.Position = ToGlobal(GetRect().Position);

			var targetBounds = new Rect2(leftX - 20, upY - 20, rightX - leftX, downY - upY);

			return bounds.Intersects(targetBounds);
		}

		public void ResetToDragStart()
		{
			Position = DragStart;
		}
	}
}

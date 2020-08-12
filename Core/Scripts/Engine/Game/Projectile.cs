using System;
using System.Collections.Generic;
using Godot;
using KoreDefenceGodot.Core.Scripts.Enemy;

namespace KoreDefenceGodot.Core.Scripts.Engine.Game
{
	public abstract class Projectile : Area2D
	{
		private readonly List<BaseEnemy> _enemiesShot = new List<BaseEnemy>();

		private int _collateral;

		// TODO Status effects
		private int _enemyHitCount;
		private AnimatedSprite _projectileSprite = null!;
		private float _timeSinceCreation;
		public Vector2 Acceleration = new Vector2(0, 0);
		public Action? OnDeath;
		public int Damage;
		public float Lifetime = 2;
		internal Vector2 Velocity { get; set; }
		public bool PlayAnimation { get; set; }

		public Node? Source { get; set; }

		/// <summary>
		///     Part of instantiating a new projectile
		/// </summary>
		/// <param name="collateral">the amount of enemies the projectile can pass through</param>
		/// <param name="playAnimation">if this projectile should play it's animation, default is true</param>
		public void Setup(int collateral, bool playAnimation = true)
		{
			(_collateral, PlayAnimation) = (collateral, playAnimation);
		}


		public override void _Ready()
		{
			_projectileSprite = GetNode<AnimatedSprite>("AnimatedSprite");
		}

		public void SetVelocity(Node2D source, Vector2 target, int speed, bool doCalculateAngle = false)
		{
			var direction = target - source.GlobalPosition;
			var mag = (float) Math.Sqrt(direction.x * direction.x + direction.y * direction.y);
			var velocity = new Vector2(direction.x / mag * speed, direction.y / mag * speed);
			Velocity = velocity;

			if (!doCalculateAngle) return;
			GlobalRotation = CalculateBulletAngle();
		}

		public void SetVelocity(Vector2 velocity, bool doCalculateAngle = false)
		{
			Velocity = velocity;
			if (!doCalculateAngle) return;
			GlobalRotation = CalculateBulletAngle();
		}


		public override void _PhysicsProcess(float delta)
		{
			UpdateProjectile(delta);
		}


		private void UpdateProjectile(float delta)
		{
			_timeSinceCreation += delta;
			var newVelocity = Velocity += Acceleration;
			Translate(newVelocity);

			if (!IsDead()) return;
			ActionOnDeath();
			QueueFree();
		}

		private bool IsDead()
		{
			return _timeSinceCreation > Lifetime || _enemyHitCount >= _collateral;
		}

		private void ActionOnDeath()
		{
			OnDeath?.Invoke();
		}

		private void OnBodyEntered(int body_id, Node body, int body_shape, int area_shape)
		{
			if (_enemyHitCount >= _collateral) return;
			if (!body.IsInGroup("enemies")) return;
			var enemy = (BaseEnemy) body;
			if (_enemiesShot.Contains(enemy)) return;
			_enemyHitCount++;

			// TODO : Implement Collateral achievement

			if (enemy.EnemyType == EnemyType.Korechanic)
			{
				if (Source != null) enemy.DealDamage(Damage, Source);
			}
			else
			{
				enemy.DealDamage(Damage);
			}

			_enemiesShot.Add(enemy);

			// TODO : Check for effects
			// TODO : Increment player damage achievement
		}

		public void FlipSprite()
		{
			if (_projectileSprite != null) _projectileSprite.FlipH = !_projectileSprite.FlipH;
		}

		// Square root is expensive so only needed if bullet angle is needed to be calculated this way
		private float CalculateBulletAngle()
		{
			var mag = (float) Math.Sqrt(Velocity.x * Velocity.x + Velocity.y * Velocity.y);
			var unitVector = Velocity / mag;
			unitVector = unitVector.x < 0 ? new Vector2(unitVector.x, -unitVector.y) : unitVector;
			var angle = (float) Math.Asin(unitVector.y);
			return angle;
		}
	}
}

using System;
using Godot;
using KoreDefenceGodot.Core.Scripts.Engine.State;
using KoreDefenceGodot.Core.Scripts.Player;
using Path = KoreDefenceGodot.Core.Scripts.Engine.Tiles.Path;

namespace KoreDefenceGodot.Core.Scripts.Enemy
{
	public abstract class BaseEnemy : KinematicBody2D
	{
		private int Health { get; set; }
		private float Speed { get; set; }
		private bool Affected { get; set; }
		private float _time;
		private bool _takingDmg;
		private AnimatedSprite _enemySprite;
		public NodeStateMachine<BaseEnemy, DefaultEnemyState> EnemyStateMachine { get; private set; }
		private Path _gamePath;
		private PlayerBase _targetBase;
		private EnemyType EnemyType { get; set; }
		/// <summary>
		/// Used to check if there are any points go to left on the path
		/// </summary>
		private bool _hasTarget;

		/// <summary>
		/// The next point for the enemy to travel to
		/// </summary>
		private Vector2 _target;
		private int _travelCount = 0;

		/// <summary>
		/// Flag that tells whether the enemy has reached the player base.
		/// </summary>
		public bool HasReachedBase { get; private set; } = false;

		public void Setup(Path gamePath, PlayerBase targetBase, EnemyType enemyType)
		{
			_gamePath = gamePath;
			_targetBase = targetBase;
			EnemyType = enemyType;
			Health = enemyType.Health;
			Speed = enemyType.Speed;
			EnemyStateMachine = new NodeStateMachine<BaseEnemy, DefaultEnemyState>(this, DefaultEnemyState.Moving, DefaultEnemyState.Global);
			Position = new Vector2(_gamePath.PathPoints[0,0], _gamePath.PathPoints[0,1]);
			_enemySprite = GetNode<AnimatedSprite>("AnimatedSprite");
		}

		public override void _Process(float delta)
		{
			EnemyStateMachine.Update(delta);
			UpdateDamageEffect(delta);
		}

		/// <summary>
		/// Deal damage to the enemy
		/// </summary>
		/// <param name="amount"> the amount of damage to be dealt</param>
		/// <param name="source"> the node that called the damage function</param>
		/// <returns></returns>
		public Node DealDamage(int amount, Node source)
		{
			Health = Health - amount < 0 ? 0 : Health - amount;
			_takingDmg = true;
			return source;
		}

		public void Heal(int amount)
		{
			Health = Health + amount > EnemyType.Health ? EnemyType.Health : Health + amount;
		}

		public bool Travel(float delta, bool isDead)
		{
			if (_hasTarget)
			{
				var distanceToTarget = _target - Position;
				var moveX = 0f;
				var moveY = 0f;

				if (distanceToTarget.x > 0)
				{
					moveX = Speed * delta;
					_enemySprite.Play("WalkRight");
				} else if (distanceToTarget.x < 0)
				{
					moveX = -Speed * delta;
					_enemySprite.Play("WalkLeft");
				}

				if (distanceToTarget.y > 0)
				{
					moveY = Speed * delta;
					_enemySprite.Play("WalkDown");
				} else if (distanceToTarget.y < 0)
				{
					moveY = -Speed * delta;
					_enemySprite.Play("WalkUp");
				}

				if (Math.Abs(moveX) > Math.Abs(distanceToTarget.x) || Math.Abs(moveY) > Math.Abs(distanceToTarget.y))
				{
					// don't overshoot target, just go there
					
					Translate(new Vector2(distanceToTarget.x, distanceToTarget.y));
					_hasTarget = false;
					return false;
				}
				
				Translate(new Vector2(moveX, 0));
				Translate(new Vector2(0, moveY));
			}
			else
			{
				_travelCount++;
				var pathPoints = _gamePath.PathPoints;

				if (pathPoints.Length > _travelCount)
				{
					var nextPoint = new Vector2(pathPoints[_travelCount,0],pathPoints[_travelCount,1]);
					_target = nextPoint;
					_hasTarget = true;
				}
				else
				{
					return true;
				}
			}

			return false;
		}

		public void AttackBase(int amount)
		{
			_targetBase.Damage(amount);
		}
		

		public bool IsDead()
		{
			return Health == 0;
		}

		private void CheckToPlayDeathAnimation(bool isDead)
		{
			// TODO Implement death animation
		}

		private void CheckToAffect(bool affected)
		{
			// TODO Implement enemy status effects
		}
		
		private void UpdateDamageEffect(float delta)
		{
			_time += delta;
			const float animTime = 0.05f;
			if (!(_time > animTime)) return;
			_time = 0;
			((ShaderMaterial)Material).SetShaderParam("FlashStatus", _takingDmg ? 1 : 0);
			_takingDmg = false;
		}
		public void EnemyReachedBase()
		{
			HasReachedBase = true;
		}
	}
}




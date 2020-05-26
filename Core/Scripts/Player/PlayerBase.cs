using Godot;
using KoreDefenceGodot.Core.Scripts.Enemy;

namespace KoreDefenceGodot.Core.Scripts.Player
{
	public abstract class PlayerBase : Area2D
	{
		private const int DefaultHealth = 1000;
		private int Health { get; set; }
		public AnimatedSprite BaseSprite;
		private bool _takingDmg;
		private float _time;

		public void Setup(int x, int y, int health = DefaultHealth)
		{
			Health = health;
			Position = new Vector2(x,y);
		}

		public void Setup(Vector2 pos, int health = DefaultHealth)
		{
			Health = health;
			Position = pos;
		}

		public override void _Ready()
		{
			BaseSprite = GetNode<AnimatedSprite>("Base");
			BaseSprite.Play("BaseNormal");
		}

		public override void _Process(float delta)
		{
			UpdateDamage(delta);
			if(IsDead()) return;
			if (Health > DefaultHealth * 0.33 && Health < DefaultHealth * 0.66)
			{

				BaseSprite.Play("BaseDamage1");
			}

			if (Health < DefaultHealth * 0.33)
			{
				BaseSprite.Play("BaseDamage2");
			}
		}

		public void Damage(int amount)
		{
			var newHealth = Health - amount;
			Health = newHealth < 0 ? 0 : newHealth;
			_takingDmg = true;
		}

		private void UpdateDamage(float delta)
		{
			_time += delta;
			const float animTime = 0.05f;
			if (!(_time > animTime)) return;
			_time = 0;
			((ShaderMaterial) BaseSprite.Material).SetShaderParam("FlashStatus", _takingDmg ? 1 : 0);
			_takingDmg = false;
		}

		private bool IsDead()
		{
			return Health == 0;
		}
		
		private void _OnBodyEntered(object body)
		{
			if (body is BaseEnemy enemy)
			{
				enemy.EnemyReachedBase();
			}
		}
	}
	
	
}




using Godot;
using KoreDefenceGodot.Core.Scripts.Enemy;

namespace KoreDefenceGodot.Core.Scripts.Player
{
    public class PlayerBase : Area2D
    {
        private const int DefaultHealth = 1000;
        private AnimatedSprite _baseSprite;
        private bool _takingDmg;
        private float _time;
        private int Health { get; set; }

        public void Setup(int x, int y, int health = DefaultHealth)
        {
            Health = health;
            Position = new Vector2(x, y);
        }

        public void Setup(Vector2 pos, int health = DefaultHealth)
        {
            Health = health;
            Position = pos;
        }

        public override void _Ready()
        {
            _baseSprite = GetNode<AnimatedSprite>("Base");
            _baseSprite.Play("BaseNormal");
        }

        public override void _Process(float delta)
        {
            UpdateDamage(delta);
            if (IsDead()) return;
            if (Health > DefaultHealth * 0.33 && Health < DefaultHealth * 0.66) _baseSprite.Play("BaseDamage1");

            if (Health < DefaultHealth * 0.33) _baseSprite.Play("BaseDamage2");

            if (_takingDmg) GD.Print("Damaged!");
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
            const float animTime = 0.1f;
            if (!(_time > animTime)) return;
            _time = 0;
            if (_takingDmg)
            {
                ((ShaderMaterial) _baseSprite.Material).SetShaderParam("FlashStatus", 1);
                _takingDmg = false;
            }
            else
            {
                ((ShaderMaterial) _baseSprite.Material).SetShaderParam("FlashStatus", 0);
            }
        }

        private bool IsDead()
        {
            return Health == 0;
        }

        protected virtual void _OnBodyEntered(object body)
        {
            if (body is BaseEnemy enemy) enemy.EnemyReachedBase();
        }
    }
}
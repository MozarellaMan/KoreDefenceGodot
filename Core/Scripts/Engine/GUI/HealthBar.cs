using Godot;
using KoreDefenceGodot.Core.Scripts.Engine.Game;

namespace KoreDefenceGodot.Core.Scripts.Engine.GUI
{
    public abstract class HealthBar : TextureProgress
    {
        private bool _baseExists;
        private Tween _tween = null!;
        private int _animatedHealth = GameInfo.DefaultMaxHealth;
        
        public override void _Ready()
        {
            _tween = GetParent().GetNode<Tween>("Tween");
            MaxValue = GameInfo.DefaultMaxHealth;
            GameInfo.PlayerBase?.Connect("HealthChanged", this, nameof(OnHealthChange));
        }

        public override void _Process(float delta)
        {
            // Find the instanced player base and connect HealthChanged signal
            if (_baseExists) Value = Mathf.Round(_animatedHealth);
            if (_baseExists || !(GameInfo.PlayerBase is { } playerBase)) return;
            _baseExists = true;
            playerBase.Connect("HealthChanged", this, nameof(OnHealthChange));
        }

        public void OnHealthChange(int newHealth)
        {
            // interpolate health bar linearly for smooth health bar
            _tween.InterpolateProperty(this, nameof(_animatedHealth),
                _animatedHealth, newHealth, 0.1f);
            if (!_tween.IsActive()) _tween.Start();
        }
    }
}
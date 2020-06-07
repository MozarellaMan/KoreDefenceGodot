using Godot;
using KoreDefenceGodot.Core.Scripts.Engine.Game;

namespace KoreDefenceGodot.Core.Scripts.Player
{
    public class Player : KinematicBody2D
    {
        private bool _downFlag;
        private bool _leftFlag;


        private PackedScene _playerProjectile;
        private AnimatedSprite _playerSprite;
        [Export] private int _projectileCollateral = 1;
        [Export] private float _projectileCooldown = 0.25f;
        [Export] private int _projectileDamage = 50;
        [Export] private float _projectileLifetime = 0.25f;
        [Export] private int _projectileSpeed = 20;
        private bool _rightFlag;

        [Export] private int _speed = 150;

        private float _timeSinceLastShot;
        private bool _upFlag;

        private void MovePlayer(float delta)
        {
            _timeSinceLastShot += delta;
            var moveX = 0;
            var moveY = 0;
            if (_upFlag)
            {
                moveY += (int) (-_speed * delta);
                _playerSprite.Play("walk_up");
            }

            if (_downFlag)
            {
                moveY += (int) (_speed * delta);
                _playerSprite.Play("walk_down");
            }

            if (_leftFlag)
            {
                moveX += (int) (-_speed * delta);
                if (moveY == 0) _playerSprite.Play("walk_left");
            }

            if (_rightFlag)
            {
                moveX += (int) (_speed * delta);
                if (moveY == 0) _playerSprite.Play("walk_right");
            }

            if (!(_upFlag || _downFlag || _leftFlag || _rightFlag))
                _playerSprite.Stop();

            MoveAndCollide(new Vector2(moveX, moveY));
        }

        public override void _Ready()
        {
            _playerSprite = GetNode<AnimatedSprite>("AnimatedSprite");
            _playerProjectile = GD.Load<PackedScene>("res://Data/Scenes/Game/Projectile.tscn");
        }

        public override void _PhysicsProcess(float delta)
        {
            if (Input.IsActionPressed("ui_up"))
                _upFlag = true;
            if (Input.IsActionPressed("ui_down"))
                _downFlag = true;
            if (Input.IsActionPressed("ui_right"))
                _rightFlag = true;
            if (Input.IsActionPressed("ui_left"))
                _leftFlag = true;

            if (Input.IsActionJustReleased("ui_up"))
                _upFlag = false;
            if (Input.IsActionJustReleased("ui_down"))
                _downFlag = false;
            if (Input.IsActionJustReleased("ui_right"))
                _rightFlag = false;
            if (Input.IsActionJustReleased("ui_left"))
                _leftFlag = false;
            MovePlayer(delta);
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            if (!(@event is InputEventMouseButton eventMouseButton) || !@event.IsActionPressed("shoot")) return;
            Shoot(eventMouseButton);
        }

        private void Shoot(InputEventMouse @event)
        {
            if (!(_timeSinceLastShot > _projectileCooldown)) return;
            if (_playerProjectile.Instance() is Projectile projectile)
            {
                AddChild(projectile);
                projectile.Setup(_projectileCollateral, false);
                projectile.Damage = _projectileDamage;
                projectile.Lifetime = _projectileLifetime;
                projectile.SetVelocity(this, @event.Position, _projectileSpeed);
                projectile.LookAt(@event.Position);
            }

            _timeSinceLastShot = 0;
        }
    }
}
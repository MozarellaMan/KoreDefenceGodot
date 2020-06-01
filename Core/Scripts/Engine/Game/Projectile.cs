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
        private float _timeSinceCreation;
        public Vector2 Acceleration = new Vector2(0, 0);

        public int Damage;
        public float Lifetime = 2;
        private Vector2 Velocity { get; set; }
        public bool PlayAnimation { get; set; }

        public Node Source { get; private set; }

        /// <summary>
        ///     Part of instantiating a new projectile
        /// </summary>
        /// <param name="x">x position of projectile</param>
        /// <param name="y">y position of projectile</param>
        /// <param name="collateral">the amount of enemies the projectile can pass through</param>
        /// <param name="playAnimation">if this projectile should play it's animation, default is true</param>
        public void Setup(float x, float y, int collateral, bool playAnimation = true)
        {
            _collateral = collateral;
            PlayAnimation = playAnimation;
        }

        public void SetVelocity(Vector2 velocity, bool doCalculateAngle)
        {
            Velocity = velocity;
            // Square root is expensive so only needed if bullet angle is needed
            if (!doCalculateAngle) return;
            var mag = (float) Math.Sqrt(Velocity.x * Velocity.x + Velocity.y * Velocity.y);
            var unitVector = Velocity / mag;
            unitVector = unitVector.x < 0 ? new Vector2(unitVector.x, -unitVector.y) : unitVector;
            var angle = (float) Math.Asin(unitVector.y);
            Rotation = angle;
        }

        public override void _PhysicsProcess(float delta)
        {
            UpdateProjectile(delta);
        }


        public void UpdateProjectile(float delta)
        {
            _timeSinceCreation += delta;
            Velocity += Acceleration;
            Translate(Velocity);

            if (IsDead()) QueueFree();
        }

        public bool IsDead()
        {
            return _timeSinceCreation > Lifetime || _enemyHitCount >= _collateral;
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
                enemy.DealDamage(Damage, Source);
            else
                enemy.DealDamage(Damage);
            _enemiesShot.Add(enemy);

            // TODO : Check for effects
            // TODO : Increment player damage achievement
        }
    }
}
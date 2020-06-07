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
        private readonly Color _attackAreaColor = new Color(255, 255, 255, 0.6f);

        // TODO : Tower upgrades
        // TODO : Tower projectile status effect

        private Vector2 _clickOffsetInTower;
        private protected bool _hasShot;
        private bool _isDeleted;


        private Sprite _towerBody;


        public Area2D AttackArea;
        private protected int AttackDamage;
        public int AttackRadius;
        public BaseEnemy CurrentTarget;
        public Vector2 DragStart;
        private protected float FirePeriod;
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
            // DrawRect(GetRect(), Colors.White);
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

        public virtual void DrawAttackRadius()
        {
            DrawCircle(ToLocal(GlobalPosition), AttackRadius, _attackAreaColor);
        }
    }
}
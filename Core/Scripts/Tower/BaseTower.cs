using Godot;
using KoreDefenceGodot.Core.Scripts.Enemy;
using KoreDefenceGodot.Core.Scripts.Engine.Game;
using KoreDefenceGodot.Core.Scripts.Engine.State;

namespace KoreDefenceGodot.Core.Scripts.Tower
{
    public abstract class BaseTower : Node2D
    {
        private const int TargetingSpeed = 20;
        public Color _attackAreaColor = new Color(255, 255, 255, 0.6f);
        private protected int _attackDamage;

        // TODO : Tower upgrades
        // TODO : Tower projectile status effect

        private Vector2 _clickOffsetInTower;
        private protected float _firePeriod;
        private protected bool _hasShot;
        private bool _isDeleted;
        private protected int _projectileCollateral;
        private protected PackedScene _projectileResource;
        private protected float _shootTimeCounter;


        private Sprite _towerBody;


        public CollisionShape2D AttackArea;
        public int AttackRadius;
        public BaseEnemy CurrentTarget;
        public Vector2 DragStart;
        private protected int ProjectileSpeed = 10;
        public bool Purchased = true;
        public AnimatedSprite TowerGun;
        public TowerType TowerType;

        // TODO : Tower type
        public NodeStateMachine<BaseTower, DefaultTowerState> TowerStateMachine { get; private set; }

        public override void _Ready()
        {
            AttackArea = GetNode("Area2D").GetNode<CollisionShape2D>("TowerRange");
            TowerGun = GetNode<AnimatedSprite>("Gun");
            _projectileResource = GD.Load<PackedScene>(TowerType.ProjectilePath);
            TowerStateMachine =
                new NodeStateMachine<BaseTower, DefaultTowerState>(this, DefaultTowerState.Idle,
                    DefaultTowerState.Global);

            _firePeriod = 1f / TowerType.FireRate;
            AttackRadius = TowerType.AttackRadius;
            _attackDamage = TowerType.Damage;
            _projectileCollateral = TowerType.Collateral;
            _hasShot = false;
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
            _shootTimeCounter += delta;
            if (!(_shootTimeCounter > _firePeriod)) return;
            _hasShot = true;
            // projectile exists and is instantiated
            if (!(_projectileResource.Instance() is Projectile projectile)) return;
            AddChild(projectile);
            projectile.Setup(_projectileCollateral);
            projectile.Source = this;
            projectile.Damage = _attackDamage;
            projectile.SetVelocity(this, enemy.GlobalPosition, ProjectileSpeed);
            projectile.FlipSprite();
            projectile.LookAt(enemy.GlobalPosition);
            _shootTimeCounter -= _firePeriod;
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

        private void OnAreaEntered(object body)
        {
            if (CurrentTarget != null && !CurrentTarget.IsDead()) return;
            if (body is BaseEnemy enemy) CurrentTarget = enemy;
        }

        private void OnBodyExit(object body)
        {
            if (!(body is BaseEnemy enemy)) return;
            if (CurrentTarget == enemy) CurrentTarget = null;
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
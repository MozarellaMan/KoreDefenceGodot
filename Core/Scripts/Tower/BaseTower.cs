using Godot;
using KoreDefenceGodot.Core.Scripts.Enemy;
using KoreDefenceGodot.Core.Scripts.Engine.Game;
using KoreDefenceGodot.Core.Scripts.Engine.State;

namespace KoreDefenceGodot.Core.Scripts.Tower
{
    public abstract class BaseTower : Node2D
    {
        private readonly int _projectileSpeed = 10;
        private CollisionShape2D _attackArea;
        private int _attackDamage;

        private int _attackRadius;
        // TODO : Tower upgrades
        // TODO : Tower projectile status effect

        private Vector2 _clickOffsetInTower;
        private Vector2 _dragStart;
        private float _firePeriod;
        private bool _hasShot;
        private bool _isDeleted;
        private bool _isPurchased;
        private int _projectileCollateral;
        private PackedScene _projectileResource;
        private float _shootTimeCounter;

        private Sprite _towerBody;
        private AnimatedSprite _towerGun;
        private BaseEnemy currentTarget;

        // TODO : Tower type
        public NodeStateMachine<BaseTower, DefaultTowerState> TowerStateMachine { get; private set; }

        public override void _Ready()
        {
            _attackArea = GetNode("Area2D").GetNode<CollisionShape2D>("TowerRange");
            _towerGun = GetNode<AnimatedSprite>("Gun");
            _projectileResource = GD.Load<PackedScene>("res://Data/Scenes/Tower/Projectiles/FiremasterBullet.tscn");
            TowerStateMachine =
                new NodeStateMachine<BaseTower, DefaultTowerState>(this, DefaultTowerState.Idle,
                    DefaultTowerState.Global);
        }

        public void Shoot(BaseEnemy enemy, float delta)
        {
            _shootTimeCounter += delta;
            if (!(_shootTimeCounter > _firePeriod) && _hasShot) return;
            _hasShot = true;
            if (!(_projectileResource.Instance() is Projectile projectile)) return;
            projectile.Setup(_projectileCollateral);
            projectile.Source = this;
            projectile.SetVelocity(this, enemy.Position, _projectileSpeed, false);
            _shootTimeCounter -= _firePeriod;
        }

        public void TrackNextTarget()
        {
            if (currentTarget != null) _towerGun.LookAt(currentTarget.Position);
        }

        private void OnAreaEntered(object body)
        {
            if (currentTarget != null && !currentTarget.IsDead()) return;
            if (body is BaseEnemy enemy) currentTarget = enemy;
        }
    }
}
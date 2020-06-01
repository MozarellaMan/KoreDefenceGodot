using Godot;
using KoreDefenceGodot.Core.Scripts.Enemy;

namespace KoreDefenceGodot.Core.Scripts.Tower
{
    public abstract class BaseTower : Node2D
    {
        private CollisionShape2D _attackArea;
        private int _attackDamage;
        private int _attackRadius;
        private Vector2 _clickOffsetInTower;

        private Vector2 _dragStart;

        // TODO : Tower type
        private float _firePeriod;

        private bool _hasShot;

        // TODO : Tower state machine
        // TODO : Tower upgrades
        // TODO : Tower projectile status effect
        private bool _isDeleted;
        private bool _isPurchased;
        private int _projectileCollateral;
        private float _timeCounter;
        private Sprite _towerBody;

        private AnimatedSprite _towerGun;
        private BaseEnemy currentTarget;


        private void OnAreaEntered(object body)
        {
            // Replace with function body.
        }
    }
}
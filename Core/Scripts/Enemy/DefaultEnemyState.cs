using System;
using Godot;
using KoreDefenceGodot.Core.Scripts.Engine.Game;
using KoreDefenceGodot.Core.Scripts.Engine.State;

namespace KoreDefenceGodot.Core.Scripts.Enemy
{
    public abstract class DefaultEnemyState : IState<BaseEnemy>
    {
        public static readonly DefaultEnemyState Idle = new IdleState();
        public static readonly DefaultEnemyState Global = new GlobalState();
        public static readonly DefaultEnemyState Moving = new MovingState();
        public static readonly DefaultEnemyState Attacking = new AttackingState();
        public static readonly DefaultEnemyState Dead = new DeadState();

        public virtual void OnEnter(BaseEnemy entity)
        {
        }

        public virtual void Update(BaseEnemy entity, float delta)
        {
        }

        public virtual void Draw(BaseEnemy entity)
        {
        }

        public virtual void HandleInput(BaseEnemy entity, InputEvent inputEvent)
        {
        }

        public virtual void OnExit(BaseEnemy entity)
        {
        }

        private sealed class GlobalState : DefaultEnemyState
        {
            public override void Update(BaseEnemy entity, float delta)
            {
                if (entity.IsDead() && !entity.EnemyStateMachine.IsInState(Dead))
                    entity.EnemyStateMachine.ChangeState(Dead);

                // GD.Print(entity.HasReachedBase());
            }
        }

        private sealed class IdleState : DefaultEnemyState
        {
        }

        private sealed class MovingState : DefaultEnemyState
        {
            public override void Update(BaseEnemy entity, float delta)
            {
                if (entity.HasReachedBase)
                    entity.EnemyStateMachine.ChangeState(Attacking);
                else
                    entity.Travel(delta, entity.IsDead());
            }
        }

        private sealed class AttackingState : DefaultEnemyState
        {
            public override void OnEnter(BaseEnemy entity)
            {
                entity.AttackBase();
                var rnd = new Random();
                entity.Position = 
                    new Vector2(rnd.Next(-50,60) + entity.Position.x,entity.Position.y);
            }

            public override void Update(BaseEnemy entity, float delta)
            {
                entity.AttackTimer += delta;
                if (!(entity.AttackTimer >= entity.EnemyType.AttackRate)) return;
                entity.AttackBase();
                entity.AttackTimer = 0;
            }

            public override void OnExit(BaseEnemy entity) => entity.AttackTimer = 0;
        }

        private sealed class DeadState : DefaultEnemyState
        {
            public override void OnEnter(BaseEnemy entity)
            {
                GameInfo.GameCurrency.EnemyKillBonus(entity);
                entity.QueueFree();
            }
        }
    }
}
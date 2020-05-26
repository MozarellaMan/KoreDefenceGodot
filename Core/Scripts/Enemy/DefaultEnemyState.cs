﻿using Godot;
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

        private sealed class GlobalState : DefaultEnemyState
        {
            public override void Update(BaseEnemy entity, float delta)
            {
                if(entity.IsDead())
                    entity.EnemyStateMachine.ChangeState(Dead);
                
                // GD.Print(entity.HasReachedBase());
            }
        }

        private sealed class IdleState : DefaultEnemyState {}

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
                GD.Print("now attacking!");
                entity.AttackBase(100);
            }

        }

        private sealed class DeadState : DefaultEnemyState
        {
            public override void OnEnter(BaseEnemy entity)
            {
                GD.Print("dead!");
            }
        }
        

        public virtual void OnEnter(BaseEnemy entity)
        {
        }

        public virtual void Update(BaseEnemy entity, float delta)
        {
        }

        public virtual void HandleInput(BaseEnemy entity, InputEvent inputEvent)
        {
        }

        public virtual void OnExit(BaseEnemy entity)
        {
        }
    }
}
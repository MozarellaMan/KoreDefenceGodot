using Godot;
using KoreDefenceGodot.Core.Scripts.Engine.Game;
using KoreDefenceGodot.Core.Scripts.Engine.State;

namespace KoreDefenceGodot.Core.Scripts.Tower
{
    public class DefaultTowerState : IState<BaseTower>
    {
        public static readonly DefaultTowerState Global = new GlobalState();
        public static readonly DefaultTowerState Idle = new IdleState();
        public static readonly DefaultTowerState Attacking = new AttackingState();
        public static readonly DefaultTowerState PickedUp = new PickedUpState();
        public static readonly DefaultTowerState Broken = new BrokenState();
        public static readonly DefaultTowerState Buying = new BuyingState();

        public virtual void OnEnter(BaseTower entity)
        {
        }

        public virtual void Update(BaseTower entity, float delta)
        {
        }

        public virtual void Draw(BaseTower entity)
        {
        }

        public virtual void HandleInput(BaseTower entity, InputEvent inputEvent)
        {
        }

        public virtual void OnExit(BaseTower entity)
        {
        }

        private sealed class GlobalState : DefaultTowerState
        {
            public override void HandleInput(BaseTower entity, InputEvent inputEvent)
            {
                if (entity.TowerStateMachine.IsInState(Broken)) return;
                if (!(inputEvent is InputEventMouseButton eventMouseButton) ||
                    !inputEvent.IsActionPressed("picked_up")) return;
                if (GameInfo.GetRect(entity.TowerGun).HasPoint(entity.ToLocal(eventMouseButton.Position)))
                    entity.TowerStateMachine.ChangeState(entity.Purchased ? PickedUp : Buying);
            }
        }

        private sealed class IdleState : DefaultTowerState
        {
            public override void OnEnter(BaseTower entity)
            {
                entity.PlayIdleAnimation();
            }

            public override void Update(BaseTower entity, float delta)
            {
                if (entity.CurrentTarget != null && entity.Purchased)
                {
                    entity.TowerStateMachine.ChangeState(Attacking);
                }
                else
                {
                    if (entity.TowerGun.RotationDegrees < 0)
                        entity.TowerGun.RotationDegrees += 1;
                    if (entity.TowerGun.RotationDegrees > 0)
                        entity.TowerGun.RotationDegrees -= 1;
                }
            }
        }

        private sealed class AttackingState : DefaultTowerState
        {
            public override void OnEnter(BaseTower entity)
            {
                entity.PlayAttackAnimation();
            }

            public override void Update(BaseTower entity, float delta)
            {
                if (entity.CurrentTarget == null)
                {
                    entity.TowerStateMachine.ChangeState(Idle);
                }
                else
                {
                    entity.TrackNextTarget(delta);
                    entity.Shoot(entity.CurrentTarget, delta);
                }
            }
        }

        private sealed class PickedUpState : DefaultTowerState
        {
            public override void OnEnter(BaseTower entity)
            {
                entity.DragStart = entity.Position;
                // TODO : show gui sell menu
            }

            public override void Update(BaseTower entity, float delta)
            {
                var mousePos = entity.GetGlobalMousePosition();
                entity.Update();
                entity.DragTo(mousePos);
            }


            public override void Draw(BaseTower entity)
            {
                entity.DrawAttackRadius();
            }

            public override void HandleInput(BaseTower entity, InputEvent inputEvent)
            {
                if (!(inputEvent is InputEventMouseButton) || !inputEvent.IsActionReleased("picked_up")) return;
                entity.TowerStateMachine.ChangeState(Idle);
            }

            public override void OnExit(BaseTower entity)
            {
                entity.Update();
            }
        }

        private sealed class BrokenState : DefaultTowerState
        {
        }

        private sealed class BuyingState : DefaultTowerState
        {
        }
    }
}
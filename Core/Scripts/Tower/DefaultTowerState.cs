using System.Linq;
using Godot;
using KoreDefenceGodot.Core.Scripts.Engine.Game;
using KoreDefenceGodot.Core.Scripts.Engine.State;
using KoreDefenceGodot.Core.Scripts.Tower.Towers;

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
                if(entity.Locked) return;
                
                // Will only check to be picked up if not the tower is not locked
                
                // Check if tower is broken and filter input
                if (entity.TowerStateMachine.IsInState(Broken)) return;
                if (!(inputEvent is InputEventMouseButton eventMouseButton) ||
                    !inputEvent.IsActionPressed("picked_up")) return;


                if (entity.GetRect().HasPoint(entity.ToLocal(eventMouseButton.Position)))
                    entity.TowerStateMachine.ChangeState(entity.Purchased ? PickedUp : Buying);
            }
        }

        private sealed class IdleState : DefaultTowerState
        {
            public override void OnEnter(BaseTower entity)
            {
                entity.PlayIdleAnimation();
                entity.CurrentTarget = null;
            }

            public override void Update(BaseTower entity, float delta)
            {
                if ((entity.CurrentTarget != null || entity.Targets.Count != 0) && entity.Purchased)
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
                entity.Targets = entity.Targets.Where(enemy => !enemy.IsDead()).ToList();
                entity.CurrentTarget = entity.Targets.Count == 0 ? null : entity.Targets.First();
                if (entity.CurrentTarget == null && entity.Targets.Count == 0)
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
                entity.PlayerCollision.Disabled = true;
                // TODO : Implement and show the shop sell menu when tower is picked up
            }

            public override void Update(BaseTower entity, float delta)
            {
                var mousePos = entity.GetGlobalMousePosition();
                entity.Update();
                entity.DragTo(mousePos);
                entity.ZIndex = 4;
                var canPlace = entity.CanPlaceTower();

                entity.AttackColour = canPlace ? GameInfo.ValidColour : GameInfo.InvalidColour;

                entity.DragStart = canPlace ? entity.Position : entity.DragStart;
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
                entity.ZIndex = 3;
                entity.PlayerCollision.Disabled = false;
                if (!entity.CanPlaceTower()) entity.ResetToDragStart();
            }
        }

        private sealed class BrokenState : DefaultTowerState
        {
        }

        private sealed class BuyingState : DefaultTowerState
        {
            public override void OnEnter(BaseTower entity)
            {
                entity.DragStart = entity.Position;
                entity.PlayerCollision.Disabled = true;
            }

            public override void Update(BaseTower entity, float delta)
            {
                PickedUp.Update(entity,delta);
                entity.ZIndex = 10;
            }

            public override void HandleInput(BaseTower entity, InputEvent inputEvent)
            {
                if (!(inputEvent is InputEventMouseButton) || !inputEvent.IsActionReleased("picked_up")) return;

                var canPlace = entity.CanPlaceTower();

                if (canPlace)
                {
                    var purchased = GameInfo.GameCurrency.PurchaseTower(entity);
                    if (purchased)
                    {
                        entity.TowerStateMachine.ChangeState(Idle);
                    }
                    else
                    {
                        entity.SetForDeletion();
                    }
                }
                else
                {
                    entity.SetForDeletion();
                }

            }

            public override void Draw(BaseTower entity) => PickedUp.Draw(entity);

            public override void OnExit(BaseTower entity) => PickedUp.OnExit(entity);
        }
    }
}
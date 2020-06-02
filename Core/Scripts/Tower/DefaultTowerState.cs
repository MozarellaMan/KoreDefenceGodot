using Godot;
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

        public virtual void HandleInput(BaseTower entity, InputEvent inputEvent)
        {
        }

        public virtual void OnExit(BaseTower entity)
        {
        }

        private sealed class GlobalState : DefaultTowerState
        {
        }

        private sealed class IdleState : DefaultTowerState
        {
        }

        private sealed class AttackingState : DefaultTowerState
        {
        }

        private sealed class PickedUpState : DefaultTowerState
        {
        }

        private sealed class BrokenState : DefaultTowerState
        {
        }

        private sealed class BuyingState : DefaultTowerState
        {
        }
    }
}
using Godot;

namespace KoreDefenceGodot.Core.Scripts.Engine.State
{
    public class NodeStateMachine<TNode,TState> : IStateMachine<TNode, TState> 
        where TState : IState<TNode>
    {
        private TNode Node { get; set; }
        private TState CurrentState { get; set; }
        private TState _previousState;
        /// <summary>
        /// Sometimes you have logic that runs in every state, this is what you would put into the global state.
        /// </summary>
        private TState GlobalState { get; set; }

        public NodeStateMachine(TNode node, TState initState)
        {
            Node = node;
            CurrentState = initState;
        }
        
        // Global state is optional
        public NodeStateMachine(TNode node, TState initState, TState globalState)
        {
            Node = node;
            CurrentState = initState;
            GlobalState = globalState;
        }

        public void Update(float delta)
        {
            GlobalState?.Update(Node,delta);
            CurrentState?.Update(Node,delta);
        }

        public void UpdateInput(InputEvent inputEvent)
        {
            GlobalState?.HandleInput(Node, inputEvent);
            CurrentState?.HandleInput(Node,inputEvent);
        }

        public void ChangeState(TState newState)
        {
            _previousState = CurrentState;
            
            CurrentState?.OnExit(Node);
            CurrentState = newState;
            CurrentState?.OnEnter(Node);
        }

        public bool GoBack()
        {
            if (_previousState == null) return false;
            ChangeState(_previousState);
            return true;
        }

        public bool IsInState(TState state)
        {
            return CurrentState.Equals(state);
        }
    }
}
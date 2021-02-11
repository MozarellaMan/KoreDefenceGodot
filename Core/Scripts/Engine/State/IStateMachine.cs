using Godot;

namespace KoreDefenceGodot.Core.Scripts.Engine.State
{
    public interface IStateMachine<TEntity, in TState> where TState : IState<TEntity>
    {
        /// <summary>
        ///     Update the current state. (called every game update)
        /// </summary>
        /// <param name="delta"> frame delta time</param>
        void Update(float delta);

        /// <summary>
        ///     Update input for the given state
        /// </summary>
        /// <param name="inputEvent">the input event</param>
        void UpdateInput(InputEvent inputEvent);

        /// <summary>
        ///     Change state
        /// </summary>
        /// <param name="newState">the new State</param>
        void ChangeState(TState newState);

        /// <summary>
        ///     Goes back to the previous state
        /// </summary>
        /// <returns>true if there is a previous state, false if there is no previous state to return to</returns>
        bool GoBack();

        /// <summary>
        ///     Checks if the entity is in a certain state
        /// </summary>
        /// <param name="state">the state</param>
        /// <returns>true if the entity is in the given state, false otherwise</returns>
        bool IsInState(TState state);
    }
}
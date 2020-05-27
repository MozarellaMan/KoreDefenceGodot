using Godot;

namespace KoreDefenceGodot.Core.Scripts.Engine.State
{
    /// <summary>
    ///     State interface
    /// </summary>
    /// <typeparam name="TEntity"> the entity this state machine is acting on</typeparam>
    public interface IState<in TEntity>
    {
        /// <summary>
        ///     Action on entering the state
        /// </summary>
        /// <param name="entity"></param>
        void OnEnter(TEntity entity);

        /// <summary>
        ///     Action on state machine update
        /// </summary>
        /// <param name="entity">The entity</param>
        /// <param name="delta">Frame delta time</param>
        void Update(TEntity entity, float delta);

        /// <summary>
        ///     Action on input updates
        /// </summary>
        /// <param name="entity">the entity</param>
        /// <param name="inputEvent">the input event</param>
        void HandleInput(TEntity entity, InputEvent inputEvent);

        /// <summary>
        ///     Action on leaving state
        /// </summary>
        /// <param name="entity">the entity</param>
        void OnExit(TEntity entity);
    }
}
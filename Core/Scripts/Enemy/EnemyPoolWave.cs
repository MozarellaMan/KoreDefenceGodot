namespace KoreDefenceGodot.Core.Scripts.Enemy
{
    public class EnemyPoolWave
    {
        /// <summary>
        ///     The type of enemy to spawn
        /// </summary>
        public EnemyType EnemyType { get; }
        /// <summary>
        ///     The amount of this type of enemy to spawn
        /// </summary>
        public int Amount { get; }
        /// <summary>
        ///     The delay before the next wave of enemies is spawned
        /// </summary>
        public float Delay { get; }
        
        public EnemyPoolWave(EnemyType enemyType, int amount, float delay)
        {
            EnemyType = enemyType;
            Amount = amount;
            Delay = delay;
        }
    }
}
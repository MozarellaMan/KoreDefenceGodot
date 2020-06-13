using Godot;
using KoreDefenceGodot.Core.Scripts.Enemy;

namespace KoreDefenceGodot.Core.Scripts.Engine.Game
{
    /// <summary>
    ///     Level 1 = 1-3
    ///     Level 2 = 4-6
    ///     Level 3 = 5-9
    ///     Level 4 = 10-12
    ///     Level 5 = 13-15
    ///     Level 6 = 16-18
    ///     Level 7 = 19-21
    /// </summary>
    public abstract class WaveSpec : Node
    {
        public static readonly WaveDefinition[] WaveNumbers =
        {
            new WaveDefinition(
                1f,
                new[]
                {
                    new EnemyPoolWave(EnemyType.Koreman, 3, 1)
                }
            ),
            new WaveDefinition(
                0.5f,
                new[]
                {
                    new EnemyPoolWave(EnemyType.Koreman, 5, 1)
                }
            ),
            new WaveDefinition(
                1f,
                new[]
                {
                    new EnemyPoolWave(EnemyType.Koreman, 3, 1),
                    new EnemyPoolWave(EnemyType.Machomaniac, 2, 1)
                }
            ),
            new WaveDefinition(
                0.5f,
                new[]
                {
                    new EnemyPoolWave(EnemyType.Koreman, 4, 4),
                    new EnemyPoolWave(EnemyType.Machomaniac, 3, 1)
                }
            ),
            new WaveDefinition(
                0.2f,
                new[]
                {
                    new EnemyPoolWave(EnemyType.Koreman, 15, 1)
                }
            ),
            new WaveDefinition(
                2f,
                new[]
                {
                    new EnemyPoolWave(EnemyType.Korechanic, 3, 1),
                    new EnemyPoolWave(EnemyType.Machomaniac, 4, 1)
                }
            ),
            new WaveDefinition(
                0.8f,
                new[]
                {
                    new EnemyPoolWave(EnemyType.Koreman, 3, 1),
                    new EnemyPoolWave(EnemyType.Korechanic, 2, 1),
                    new EnemyPoolWave(EnemyType.Machomaniac, 5, 1)
                }
            ),
            new WaveDefinition(
                0.5f,
                new[]
                {
                    new EnemyPoolWave(EnemyType.Korechanic, 1, 1),
                    new EnemyPoolWave(EnemyType.KoreProtector, 3, 1)
                }
            ),
            new WaveDefinition(
                .1f,
                new[]
                {
                    new EnemyPoolWave(EnemyType.KoreProtector, 5, 1),
                    new EnemyPoolWave(EnemyType.Koreman, 5, 1)
                }
            ),
            new WaveDefinition(
                .8f,
                new[]
                {
                    new EnemyPoolWave(EnemyType.Korechanic, 3, 1),
                    new EnemyPoolWave(EnemyType.Machomaniac, 3, 1),
                    new EnemyPoolWave(EnemyType.Koreman, 6, 1),
                    new EnemyPoolWave(EnemyType.KoreProtector, 2, 1)
                }
            ),
            new WaveDefinition(
                .1f,
                new[]
                {
                    new EnemyPoolWave(EnemyType.Korechanic, 2, 1),
                    new EnemyPoolWave(EnemyType.KoreProtector, 6, 1)
                }
            ),
            new WaveDefinition(
                1f,
                new[]
                {
                    new EnemyPoolWave(EnemyType.KoreProtector, 5, 1),
                    new EnemyPoolWave(EnemyType.Volcanologist, 2, 1),
                    new EnemyPoolWave(EnemyType.Koreman, 2, 1)
                }
            ),
            new WaveDefinition(
                .8f,
                new[]
                {
                    new EnemyPoolWave(EnemyType.KoreProtector, 2, 1),
                    new EnemyPoolWave(EnemyType.Machomaniac, 3, 1),
                    new EnemyPoolWave(EnemyType.Volcanologist, 5, 1)
                }
            ),
            new WaveDefinition(
                .1f,
                new[]
                {
                    new EnemyPoolWave(EnemyType.KoreProtector, 8, 1),
                    new EnemyPoolWave(EnemyType.Machomaniac, 5, 1)
                }
            ),
            new WaveDefinition(
                2f,
                new[]
                {
                    new EnemyPoolWave(EnemyType.Korechanic, 3, 1),
                    new EnemyPoolWave(EnemyType.Volcanologist, 5, 1),
                    new EnemyPoolWave(EnemyType.Machomaniac, 10, 1)
                }
            ),
            new WaveDefinition(
                .8f,
                new[]
                {
                    new EnemyPoolWave(EnemyType.KoreProtector, 15, 1)
                }
            ),
            new WaveDefinition(
                0.25f,
                new[]
                {
                    new EnemyPoolWave(EnemyType.KoreProtector, 10, 1),
                    new EnemyPoolWave(EnemyType.Volcanologist, 8, 1),
                    new EnemyPoolWave(EnemyType.Machomaniac, 5, 1)
                }
            ),
            new WaveDefinition(
                .25f,
                new[]
                {
                    new EnemyPoolWave(EnemyType.Korechanic, 1, 1),
                    new EnemyPoolWave(EnemyType.KoreProtector, 5, 1),
                    new EnemyPoolWave(EnemyType.Volcanologist, 18, 1)
                }
            ),
            new WaveDefinition(
                .25f,
                new[]
                {
                    new EnemyPoolWave(EnemyType.KoreProtector, 35, 1)
                }
            ),
            new WaveDefinition(
                .1f,
                new[]
                {
                    new EnemyPoolWave(EnemyType.Volcanologist, 35, 1)
                }
            ),
            new WaveDefinition(
                .25f,
                new[]
                {
                    new EnemyPoolWave(EnemyType.Kore, 1, 1)
                }
            )
        };

        public class WaveDefinition
        {
            public readonly EnemyPoolWave[] EnemyWaves;
            public readonly string[]? Popups;
            public readonly float SpawnDelay;

            public WaveDefinition(float spawnDelay, EnemyPoolWave[] enemyPoolWaves, string[] popups)
            {
                SpawnDelay = spawnDelay;
                EnemyWaves = enemyPoolWaves;
                Popups = popups;
            }

            public WaveDefinition(float spawnDelay, EnemyPoolWave[] enemyPoolWaves)
            {
                (SpawnDelay, EnemyWaves) = (spawnDelay, enemyPoolWaves);
            }
        }
    }
}
using System.Collections.Generic;
using Godot;
using KoreDefenceGodot.Core.Scripts.Enemy;
using KoreDefenceGodot.Core.Scripts.Engine.Tiles;
using KoreDefenceGodot.Core.Scripts.Player;
using Path = KoreDefenceGodot.Core.Scripts.Engine.Tiles.Path;

namespace KoreDefenceGodot.Core.Scripts.Engine.Game
{
    public class GameInfo : Node
    {
        public List<BaseEnemy> EnemyList { get; set; }

        // TODO : Implement towers + tower list
        /// <summary>
        ///     List of all tiles, needed for lava collisions
        /// </summary>
        public Tile[,] TileList { get; set; }

        public Path GamePath { get; set; }

        // TODO Impelement currency
        public Player.Player Player { get; set; }

        // TODO Implement tile finder
        // TODO Implement tower remover?
        public int MoneySpawnCount { get; set; } = 0;

        /// <summary>
        ///     Should be from 1 - 7. Represents each "level" of the game
        /// </summary>
        public static int LevelNumber { get; set; }

        /// <summary>
        ///     Should be from 1-3. Each "level" has 3 "waves"
        /// </summary>
        public static int WaveNumber { get; set; } = 1;

        public PlayerBase PlayerBase { get; set; }

        // flags used for achievements
        public bool HasUsedSlownessFlag { get; set; } = false;
        public bool ExpensiveTowerDestroyed { get; set; } = false;

        /// <summary>
        ///     Sets the raw wave number.
        ///     Finds level number from wave number e.g 20 will return 7,
        ///     Then finds wave number from raw wave number e.g. 20 will return 2, 21 will return 3.
        /// </summary>
        /// <param name="rawWaveNumber"></param>
        public void SetRawWaveNumber(int rawWaveNumber)
        {
            LevelNumber = (rawWaveNumber - 1 - (rawWaveNumber - 1) % 3) / 3 + 1;
            WaveNumber = (rawWaveNumber - 1) % 3 + 1;
        }

        public static int GetRawWaveNumber()
        {
            return LevelNumber * 3 + WaveNumber;
        }
    }
}
using System.Collections.Generic;
using Godot;
using KoreDefenceGodot.Core.Scripts.Enemy;
using KoreDefenceGodot.Core.Scripts.Player;
using Path = KoreDefenceGodot.Core.Scripts.Engine.Tiles.Path;

namespace KoreDefenceGodot.Core.Scripts.Engine.Game
{
    public abstract class Wave : Node2D
    {
        private List<BaseEnemy> _enemies;

        // TODO Timer to keep time of enemy waves
        private EnemyFactory _factory;
        private Path _gamePath;
        private PlayerBase _playerBase;

        public bool WaveOver { get; private set; }
        // TODO UI popups
        // TODO Wave start & end sounds

        public void CreateWave()
        {
            var waveCount = GameInfo.GetRawWaveNumber() - 1;
            var currentWave = WaveSpec.WaveNumbers[waveCount];
            if (currentWave == null) return;
            _factory = GD.Load<PackedScene>("res://Data/Scenes/Enemy/EnemyFactory.tscn")
                .Instance() as EnemyFactory;

            _factory?.Setup(currentWave.SpawnDelay, currentWave.EnemyWaves);
            AddChild(_factory);
        }

        public void Setup(List<BaseEnemy> enemies, Path gamePath, PlayerBase playerBase)
        {
            _enemies = enemies;
            _gamePath = gamePath;
            _playerBase = playerBase;

            // TODO Load wave sound resources
        }

        public void StartWave()
        {
            // TODO Play wave sound start
            CreateWave();

            WaveOver = false;
        }

        public void RunWave()
        {
            float lastEnemySpawnTime = 10; // TODO Implement enemy Timer!
            if (_factory.CanSpawnMoreEnemies())
            {
                if (lastEnemySpawnTime.CompareTo(_factory.SpawnDelay) <= 0) return;
                var newEnemy = _factory.CreateEnemies(_gamePath, _playerBase);
                if (newEnemy != null) _enemies.Add(newEnemy);
                // TODO Restart enemy timer!
            }
            else
            {
                // TODO Play wave end sound
                WaveOver = true;
            }
        }
    }
}
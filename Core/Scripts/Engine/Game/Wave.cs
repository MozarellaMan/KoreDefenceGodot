using System.Collections.Generic;
using Godot;
using KoreDefenceGodot.Core.Scripts.Enemy;
using KoreDefenceGodot.Core.Scripts.Player;
using Path = KoreDefenceGodot.Core.Scripts.Engine.Tiles.Path;

namespace KoreDefenceGodot.Core.Scripts.Engine.Game
{
    public abstract class Wave : Node2D
    {
        // TODO Timer to keep time of enemy waves
        private EnemyFactory _factory;
        private List<BaseEnemy> _enemies;
        private PlayerBase _playerBase;
        private Path _gamePath;
        // TODO UI popups
        // TODO Wave start & end sounds

        public void Setup(List<BaseEnemy> enemies, Path gamePath, PlayerBase playerBase)
        {
            _enemies = enemies;
            _gamePath = gamePath;
            _playerBase = playerBase;
            
            // TODO Load wave sound resources
        }
    }
}
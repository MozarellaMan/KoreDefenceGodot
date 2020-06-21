using Godot;
using KoreDefenceGodot.Core.Scripts.Engine.GUI;
using KoreDefenceGodot.Core.Scripts.Engine.Tiles;
using KoreDefenceGodot.Core.Scripts.Player;
using Path = KoreDefenceGodot.Core.Scripts.Engine.Tiles.Path;

namespace KoreDefenceGodot.Core.Scripts.Engine.Game.LevelManager
{
    public class Level : Node
    {
        private Path _gamePath = null!;
        private PlayerBase? _playerBase;
        private Wave? _gameWave;
        private TileSystem? _tileSystem;

        public void TestInit()
        {
            LoadTilesAndPath();
            UpdateTowerShop();
            SetupNextWave();
        }
        
        public void LoadTilesAndPath(int[,] pathSpec = null!)
        {
            // Instance tile generating object
            _tileSystem = GD.Load<PackedScene>("res://Data/Scenes/Tiles/TileSystem.tscn").Instance() as TileSystem;
            _tileSystem?.Setup(1000, 800, 40);
            AddChild(_tileSystem);

            // Generate Path
            _gamePath = new Path();
            _gamePath.Setup(_tileSystem?.Tiles!, new[] {2, 2, 2, 2, 2, 2, 2, 1, -10, 10, -9},
                new[] {3, -3, 2, -3, 2, -3, 3, 2, 3, 2, 2});

            // Create player base at end of Path
            _playerBase = GD.Load<PackedScene>("res://Data/Scenes/Player/PlayerBase.tscn").Instance() as PlayerBase;
            _playerBase?.Setup(_gamePath.GetEndPoint());
            AddChild(_playerBase);
            _gameWave = GetNode<Node2D>("Wave") as Wave;
        }


        public void SetupNextWave()
        {
            _gameWave?.Setup(_gamePath, _playerBase!);
            _gameWave?.CreateWave();
            GameInfo.GamePath = _gamePath;
        }


        public override void _PhysicsProcess(float delta)
        {
            _gameWave?.RunWave(delta);
        }

        public void UpdateTowerShop()
        {
            GUIManager.SetupTowerShop();
        }
        
    }
}
using Godot;
using KoreDefenceGodot.Core.Scripts.Engine.Game;
using KoreDefenceGodot.Core.Scripts.Engine.Tiles;
using KoreDefenceGodot.Core.Scripts.Player;
using Path = KoreDefenceGodot.Core.Scripts.Engine.Tiles.Path;

namespace KoreDefenceGodot.Core.Scripts
{
    public abstract class MainScene : Node2D
    {
        private const string GameTitle = "Kore Defence";
        private Path _gamePath;
        private PlayerBase _playerBase;
        private Wave _runningWave;
        private TileSystem _tileSystem;

        private void LoadTilesAndPath()
        {
            // Instance tile generating object
            _tileSystem = GD.Load<PackedScene>("res://Data/Scenes/Tiles/TileSystem.tscn").Instance() as TileSystem;
            _tileSystem?.Setup(1000, 800, 40);
            AddChild(_tileSystem);

            // Generate Path
            _gamePath = new Path();
            _gamePath.Setup(_tileSystem.Tiles, new[] {2, 2, 2, 2, 2, 2, 2, 1, -10, 10, -9},
                new[] {3, -3, 2, -3, 2, -3, 3, 2, 3, 2, 2});

            // Create player base at end of Path
            _playerBase = GD.Load<PackedScene>("res://Data/Scenes/Player/PlayerBase.tscn").Instance() as PlayerBase;
            _playerBase?.Setup(_gamePath.GetEndPoint());
            AddChild(_playerBase);
            _runningWave = GetNode<Node2D>("Wave") as Wave;
        }

        public override void _Ready()
        {
            GD.Print("Main Scene ready!");
            LoadTilesAndPath();
            _runningWave.Setup(_gamePath, _playerBase);
            _runningWave.CreateWave();
        }

        public override void _Process(float delta)
        {
            OS.SetWindowTitle($"{GameTitle} FPS: {Godot.Engine.GetFramesPerSecond()}");
        }

        public override void _PhysicsProcess(float delta)
        {
            _runningWave.RunWave(delta);
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            if (!(@event is InputEventKey eventKey)) return;
            if (!eventKey.Pressed || eventKey.Scancode != (int) KeyList.P) return;
            _playerBase?.Damage(1);
        }
    }
}
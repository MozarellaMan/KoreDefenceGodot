using Godot;
using KoreDefenceGodot.Core.Scripts.Engine.Tiles;
using Path = KoreDefenceGodot.Core.Scripts.Engine.Tiles.Path;

namespace KoreDefenceGodot.Core.Scripts
{
	public class MainScene : Node2D
	{
		private string _GameTitle = "Kore Defence";
		private TileSystem _tileSystem;

		public void LoadTiles()
		{
			_tileSystem = GD.Load<PackedScene>("res://Data/Scenes/Tiles/TileSystem.tscn").Instance() as TileSystem;
			
			_tileSystem?.Setup(1000,800,40);
			AddChild(_tileSystem);
			
			Path path = new Path();
			path.Setup(_tileSystem.Tiles, new []{2, 2, 2, 2, 2, 2, 2, 1, -10, 10, -9}, new []{3, -3, 2, -3, 2, -3, 3, 2, 3, 2, 2});
		}
		public override void _Ready()
		{
			GD.Print("Hello World!");
			LoadTiles();
		}

		public override void _Process(float delta)
		{
			OS.SetWindowTitle($"{_GameTitle} FPS: {Godot.Engine.GetFramesPerSecond()}");
		}
	}
}

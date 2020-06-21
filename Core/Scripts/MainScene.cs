using Godot;
using KoreDefenceGodot.Core.Scripts.Engine.Game.LevelManager;

namespace KoreDefenceGodot.Core.Scripts
{
	public abstract class MainScene : Node2D
	{
		private const string GameTitle = "Kore Defence";
		private Level? _game;


		public override void _Ready()
		{
			_game = GetNode("Level") as Level;
			//_game?.TestInit();
		}

		public override void _Process(float delta)
		{
			OS.SetWindowTitle($"{GameTitle} FPS: {Godot.Engine.GetFramesPerSecond()}");
		}
	}
}

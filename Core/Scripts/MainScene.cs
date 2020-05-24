using Godot;

namespace KoreDefenceGodot.Core.Scripts
{
	public class MainScene : Node2D
	{
		private string _GameTitle = "Kore Defence";
		public override void _Ready()
		{
			GD.Print("Hello World!");
		}

		public override void _Process(float delta)
		{
			OS.SetWindowTitle($"{_GameTitle} FPS: {Engine.GetFramesPerSecond()}");
		}
	}
}

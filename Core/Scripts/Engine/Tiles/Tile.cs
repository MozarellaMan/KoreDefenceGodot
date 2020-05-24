using System.Diagnostics;
using Godot;

namespace KoreDefenceGodot.Core.Scripts.Engine.Tiles
{
	public enum TileObject
	{
		Path,
		Cracked,
		Money,
		Lava,
		None
	}
	
	public class Tile : Node2D
	{

		private int _x;
		private int _y;
		[Export]
		private TileObject _occupiedBy;
		private int _tileSize;
		public PackedScene PathResource { get; set; }

		public void Setup(int x, int y, TileObject occupiedBy = TileObject.None, int tileSize = 40)
		{
			_x = x;
			_y = y;
			_occupiedBy = occupiedBy;
			_tileSize = tileSize;
		}

		public override void _Ready()
		{
			Position = new Vector2(_x*40,_y*40);
			switch (_occupiedBy)
			{
				case TileObject.Path:
					break;
				case TileObject.Cracked:
					break;
				case TileObject.Lava:
					break;
				case TileObject.Money:
					break;
			}
		}

		public void Occupy(TileObject obj)
		{
			_occupiedBy = obj;
			GD.Print(_occupiedBy);
		}

		public void OccupyPath(PathType type)
		{
			_occupiedBy = TileObject.Path;
			GD.Print($"{Position} is occupied by {type.ToString()}!");

			var pathSegment = PathResource.Instance();
			pathSegment.GetNode<AnimatedSprite>("Path").Play(type.ToString());
			
			AddChild(pathSegment);
		}
		
	}
}

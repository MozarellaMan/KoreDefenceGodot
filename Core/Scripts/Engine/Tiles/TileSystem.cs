using Godot;

namespace KoreDefenceGodot.Core.Scripts.Engine.Tiles
{
	public abstract class TileSystem : Node2D
	{
		private int _numTilesX;
		private int _numTilesY;
		private PackedScene _pathResource;
		private PackedScene _tileScene;
		public Tile[,] Tiles { get; private set; }

		public void Setup(int screenWidth, int screenHeight, int tileSize)
		{
			_tileScene = GD.Load<PackedScene>("res://Data/Scenes/Tiles/Tile.tscn");
			_pathResource = GD.Load<PackedScene>("res://Data/Scenes/Tiles/PathSegment.tscn");
			_numTilesX = screenWidth / tileSize;
			_numTilesY = screenHeight / tileSize;
		}

		/// <summary>
		///     Create all tiles
		/// </summary>
		public override void _Ready()
		{
			Tiles = new Tile[_numTilesX, _numTilesY];
			for (var x = 0; x < _numTilesX; x++)
			for (var y = 0; y < _numTilesY; y++)
			{
				if (!(_tileScene.Instance() is Tile newTile)) continue; // check null
				newTile.Setup(x, y);
				newTile.PathResource = _pathResource;
				AddChild(newTile);
				Tiles[x, y] = newTile;
			}
		}
	}
}

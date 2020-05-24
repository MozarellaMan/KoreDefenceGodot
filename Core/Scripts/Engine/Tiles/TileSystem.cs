using Godot;

namespace KoreDefenceGodot.Core.Scripts.Engine.Tiles
{
    public class TileSystem : Node2D
    {
        private int _numTilesX;
        private int _numTilesY;
        private PackedScene _tileScene;
        public Tile[,] Tiles { get; set; }
        private PackedScene _pathResource;

        public void Setup(int screenWidth, int screenHeight, int tileSize)
        {
            _tileScene = GD.Load<PackedScene>("res://Data/Scenes/Tiles/Tile.tscn");
            _pathResource = GD.Load<PackedScene>("res://Data/Scenes/Tiles/PathSegment.tscn");
            _numTilesX = screenWidth / tileSize;
            _numTilesY = screenHeight / tileSize;
            
        }

        public override void _Ready()
        {
            GD.Print("hello!");
            Tiles = new Tile[_numTilesX,_numTilesY];
            for (int x = 0; x < _numTilesX; x++)
            {
                for (int y = 0; y < _numTilesY; y++)
                {
                    if (_tileScene.Instance() is Tile newTile)
                    {
                        newTile.Setup(x,y);
                        newTile.PathResource = _pathResource;
                        AddChild(newTile);
                        Tiles[x, y] = newTile;
                    }
                }
            }
        }
    }
}
using Godot;

namespace KoreDefenceGodot.Core.Scripts.Engine.Tiles
{
    public class TileSystem : Node2D
    {
        private int _numTilesX;
        private int _numTilesY;
        private PackedScene _tileScene;
        private Tile[,] _tiles;

        public void Setup(int screenWidth, int screenHeight, int tileSize)
        {
            _tileScene = GD.Load<PackedScene>("res://Data/Scenes/Tiles/Tile.tscn");
            _numTilesX = screenWidth / tileSize;
            _numTilesY = screenHeight / tileSize;
            
        }

        public override void _Ready()
        {
            GD.Print("hello!");
            _tiles = new Tile[_numTilesX,_numTilesY];
            for (int x = 0; x < _numTilesX; x++)
            {
                for (int y = 0; y < _numTilesY; y++)
                {
                    var newTile = _tileScene.Instance() as Tile;
                    newTile?.Setup(x,y);
                    AddChild(newTile);
                    _tiles[x,y] = newTile;
                }
            }
        }
    }
}
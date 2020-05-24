using System.Diagnostics;
using Godot;

namespace KoreDefenceGodot.Core.Scripts.Engine.Tiles
{
    public enum TileObjects
    {
        Cracked,
        Money,
        Lava,
        None
    }
    
    public class Tile : Node2D
    {
        private int _x;
        private int _y;
        private TileObjects _occupiedBy;
        private int _tileSize;

        public void Setup(int x, int y, TileObjects occupiedBy = TileObjects.None, int tileSize = 40)
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
                case TileObjects.Cracked:
                    break;
                case TileObjects.Lava:
                    break;
                case TileObjects.Money:
                    break;
            }
        }
    }
}
using System;
using Godot;

namespace KoreDefenceGodot.Core.Scripts.Engine.Tiles
{
	public enum PathType
	{
		Straight,
		Horizontal,
		DownLeft,
		DownRight,
		LeftDown,
		RightDown,
	}
	public class Path
	{
		private int _tileSize;
		/// <summary>
		/// Represents X and Y coordinates of path corners in pixels
		/// </summary>
		public int[,] PathPoints { get; set; }
		public Tile[,] Tiles { get; set; }
		/// <summary>
		/// Starting position of the Path
		/// </summary>
		private int _curPositionX = -1;
		private int _curPositionY = 4;

		/// <summary>
		/// Create a path from the horizontalParts and verticalPaths arrays
		/// </summary>
		public void Setup(Tile[,] tiles, int[] horizontalPaths, int[] verticalPaths, int tileSize = 40)
		{
			_tileSize = tileSize;
			Tiles = tiles;
			
			// Travel 15 Assets.tiles horizontal, then 8 vertically, then -10 horizontally, etc

			var horzLength = horizontalPaths.Length;
			var vertLength = verticalPaths.Length;

			//e.g a path of 8 movements has 9 points, that's why +1
			PathPoints = new int [horzLength + vertLength + 1, 2];

			for (var i = 0; i < horzLength; i++)
			{
				var newPositionX = _curPositionX + horizontalPaths[i];
				var minX = Math.Min(newPositionX, _curPositionX);
				var maxX = Math.Max(newPositionX, _curPositionX);
				if(newPositionX < 0)
					newPositionX = 1;
				if(newPositionX > 16)
					newPositionX = 15;
				OccupyTileWithPath(Tiles, PathType.Horizontal, minX + 1, maxX - 1, _curPositionY, _curPositionY);

				if (i >= vertLength) continue;
				OccupyTileWithPath(Tiles,GetCorner(horizontalPaths[i],verticalPaths[i], true), newPositionX, newPositionX, _curPositionY, _curPositionY);

				_curPositionX = newPositionX;
				PathPoints[i*2+1,0] = _curPositionX * tileSize;
				PathPoints[i*2+1,1] = _curPositionY * tileSize;

				var newPositionY = _curPositionY + verticalPaths[i];
				var minY = Math.Min(newPositionY, _curPositionY);
				var maxY = Math.Max(newPositionY, _curPositionY);
					
				OccupyTileWithPath(Tiles, PathType.Straight, _curPositionX, _curPositionX, minY + 1, maxY - 1);
				if(newPositionY<=3)
					newPositionY=4;
				if(newPositionY>18)
					newPositionY=17;
				_curPositionY = newPositionY;
				if (i + 1 < horzLength) {
					OccupyTileWithPath(Tiles, GetCorner(horizontalPaths[i+1], verticalPaths[i],false), _curPositionX, _curPositionX, newPositionY, newPositionY);
				} else {
					break;
				}
			}
		}
		
		

		private static void OccupyTileWithPath(Tile[,] tiles, PathType type, int x1, int x2, int y1, int y2)
		{
			for (var x = x1; x <= x2; x++) {
				for (var y = y1; y <= y2; y++) {
					tiles[x,y].OccupyPath(type);
				}
			}
		}

		/// <summary>
		/// Finds correct corner tile
		/// </summary>
		/// <param name="horizontal"></param>
		/// <param name="vertical"></param>
		/// <param name="isTravellingHorizontally"></param>
		/// <returns></returns>
		private static PathType GetCorner(int horizontal, int vertical, bool isTravellingHorizontally)
		{
			if (!isTravellingHorizontally) {
				horizontal = -horizontal;
				vertical = -vertical;
			}

			if (horizontal > 0)
			{
				return vertical > 0 ? PathType.RightDown : PathType.DownLeft;
			}

			return vertical > 0 ? PathType.LeftDown : PathType.DownRight;
		}

		public Vector2 GetEndPoint()
		{
			return new Vector2( _curPositionX * _tileSize, _curPositionY * _tileSize);
		}
	}
}

using Godot;

namespace KoreDefenceGodot.Core.Scripts.Engine.Game
{
	public abstract class GameUtil : Node
	{

		public static void ClearChildren(Node node)
		{
			foreach (Node child in node.GetChildren())
			{
				child.QueueFree();
			}
		}
		
		
		/// <summary>
		///     Get bounding rectangle of an animated sprite
		/// </summary>
		/// <param name="sprite"></param>
		/// <returns></returns>
		public static Rect2 GetRect(AnimatedSprite sprite)
		{
			// rectangle of the current frame of the animated sprite
			var texture = sprite.Frames.GetFrame(sprite.Animation, sprite.Frame);
			return new Rect2(sprite.Position - texture.GetSize() / 2, texture.GetSize());
		}
	}
}

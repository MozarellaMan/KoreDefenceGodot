using System;
using Godot;
using KoreDefenceGodot.Core.Scripts.Enemy;
using KoreDefenceGodot.Core.Scripts.Engine.Game;

namespace KoreDefenceGodot.Core.Scripts.Tower.Towers
{
	public abstract class Catapult : BaseTower
	{
		public bool CanCatapult = false;
		private Vector2[]? _shapePoints;

		public AnimationPlayer Anim = null!;
		public override void _Ready()
		{
			TowerType = TowerType.Catapult;
			Anim = GetNode<AnimationPlayer>("AnimationPlayer");
			
			base._Ready();
			var coneShape = GetNode<CollisionPolygon2D>("Area2D/TowerRange");
			_shapePoints = ConfigureArcPoints(Position,AttackRadius,75,110,32);
			coneShape.Polygon = _shapePoints;
		}

		public override void Shoot(BaseEnemy? enemy, float delta, bool immediate = false)
		{
			base.Shoot(enemy, delta, immediate);
			PlayAttackAnimation();
		}

		public override void DrawAttackRadius()
		{
			DrawColoredPolygon(_shapePoints, AttackColour);
		}

		private static Vector2[] ConfigureArcPoints(Vector2 center,float radius, float angleFrom, float angleTo, int numPoints)
		{
			var points = new Vector2[numPoints + 1];
			points[0] = center;
			for (var pointNo = 0; pointNo < numPoints; ++pointNo)
			{
				var anglePoint = Mathf.Deg2Rad(angleFrom + pointNo * (angleTo - angleFrom) / numPoints - 90);
				points[pointNo + 1] = center + new Vector2(Mathf.Cos(anglePoint), Mathf.Sin(anglePoint)) * radius;
			}
			
			return points;
		}

		protected internal override Rect2 GetRect() => TowerBody.GetRect();

		public override void PlayAttackAnimation()
		{
			Anim.Play("shoot");
		}
	}
}

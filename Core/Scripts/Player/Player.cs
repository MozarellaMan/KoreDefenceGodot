using System.Diagnostics;
using Godot;

namespace KoreDefenceGodot.Core.Scripts.Player
{
	public class Player : KinematicBody2D
	{
		private AnimatedSprite playerSprite;

		[Export] private int _speed = 150;

		private bool _upFlag = false;
		private bool _downFlag = false;
		private bool _leftFlag = false;
		private bool _rightFlag = false;

		private void MovePlayer(float delta)
		{
			var moveX = 0;
			var moveY = 0;
			if (_upFlag)
			{
				moveY += (int)(-_speed * delta);
				playerSprite.Play("walk_up");
			}

			if (_downFlag)
			{
				moveY += (int)(_speed * delta);
				playerSprite.Play("walk_down");
			}

			if (_leftFlag)
			{
				moveX += (int)(-_speed * delta);
				if(moveY == 0) playerSprite.Play("walk_left"); 
			}

			if (_rightFlag)
			{
				moveX += (int)(_speed * delta);
				if(moveY == 0) playerSprite.Play("walk_right");
			}
			
			if(!(_upFlag || _downFlag || _leftFlag || _rightFlag))
				playerSprite.Stop();

			MoveAndCollide(new Vector2(moveX, moveY));
		}

		public override void _Ready()
		{
			playerSprite = GetNode<AnimatedSprite>("AnimatedSprite");
		}

		public override void _Process(float delta)
		{
			if (Input.IsActionPressed("ui_up"))
				_upFlag = true;
			if (Input.IsActionPressed("ui_down"))
				_downFlag = true;
			if (Input.IsActionPressed("ui_right"))
				_rightFlag = true;
			if (Input.IsActionPressed("ui_left"))
				_leftFlag = true;
			
			if (Input.IsActionJustReleased("ui_up"))
				_upFlag = false;
			if (Input.IsActionJustReleased("ui_down"))
				_downFlag = false;
			if (Input.IsActionJustReleased("ui_right"))
				_rightFlag = false;
			if (Input.IsActionJustReleased("ui_left"))
				_leftFlag = false;
			MovePlayer(delta);
		}
	}
}

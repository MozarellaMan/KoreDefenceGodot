using Godot;

namespace KoreDefenceGodot.Core.Scripts.Player
{
	public class Player : KinematicBody2D
	{
		private AnimatedSprite _playerSprite;

		[Export] private int _speed = 150;

		private bool _upFlag;
		private bool _downFlag;
		private bool _leftFlag;
		private bool _rightFlag;

		private void MovePlayer(float delta)
		{
			var moveX = 0;
			var moveY = 0;
			if (_upFlag)
			{
				moveY += (int)(-_speed * delta);
				_playerSprite.Play("walk_up");
			}

			if (_downFlag)
			{
				moveY += (int)(_speed * delta);
				_playerSprite.Play("walk_down");
			}

			if (_leftFlag)
			{
				moveX += (int)(-_speed * delta);
				if(moveY == 0) _playerSprite.Play("walk_left"); 
			}

			if (_rightFlag)
			{
				moveX += (int)(_speed * delta);
				if(moveY == 0) _playerSprite.Play("walk_right");
			}
			
			if(!(_upFlag || _downFlag || _leftFlag || _rightFlag))
				_playerSprite.Stop();

			MoveAndCollide(new Vector2(moveX, moveY));
		}

		public override void _Ready()
		{
			_playerSprite = GetNode<AnimatedSprite>("AnimatedSprite");
		}

		public override void _PhysicsProcess(float delta)
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

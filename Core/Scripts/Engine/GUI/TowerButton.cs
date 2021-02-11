using Godot;
using KoreDefenceGodot.Core.Scripts.Engine.Game;
using KoreDefenceGodot.Core.Scripts.Tower;

namespace KoreDefenceGodot.Core.Scripts.Engine.GUI
{
	public abstract class TowerButton : MarginContainer
	{
		private Label _priceLabel =  null!;
		private TextureButton _button = null!;
		private Label _towerLabel = null!;
		public TowerType? TowerType;
		private ColorRect _background = null!;
		private readonly Color _hoverColour = Color.Color8(46, 46, 46,200);
		private readonly Color _backgroundColour =  new(1, 1, 1,0);
		private readonly Color _normalColor =  new(1, 1, 1);
		private readonly Color _disabledColor = Color.Color8(138, 137, 148);
		private TowerManager _towerManager = null!;
		private Texture _normalTexture = null!;
		private Texture _buyingTexture = null!;
		/// <summary>
		/// 	If true, the button accepts input
		/// </summary>
		private bool _isActive = true;
		/// <summary>
		/// 	If true, the button will be locked, and tower will be hidden with red
		/// </summary>
		public bool Locked = true;

		public void Setup(TowerType type, TowerManager manager) => 
			(TowerType, _towerManager) = (type, manager);


		public override void _Ready()
		{
			_button = GetNode("VBoxContainer").GetNode<TextureButton>("Tower");
			_priceLabel = GetNode("VBoxContainer").GetNode("PriceContainer").GetNode("PriceLabel").GetNode("Info").GetNode<Label>("Price");
			_towerLabel = _button.GetNode("TowerInfo").GetNode("VBoxContainer").GetNode<Label>("TowerLabel");
			_background = GetNode<ColorRect>("ColorRect");
			if (TowerType == null) return;
			_button.TextureNormal = GD.Load<Texture>(TowerType?.IconPath);
			_towerLabel.Text = TowerType?.Name;
			_priceLabel.Text = TowerType?.Cost.ToString();
			_normalTexture = _button.TextureNormal;
			_buyingTexture = _button.TexturePressed;
		}

		public override void _Process(float delta)
		{
			if (Locked) _isActive = !Locked;

			if (!_isActive)
			{
				_button.Disabled = true;
				if (Locked)
				{
					_button.Modulate = Colors.Red;
					_towerLabel.Visible = false;
				}
				else
				{
					_button.Modulate = _disabledColor;
					_towerLabel.Visible = true;
					_towerLabel.Modulate = _disabledColor;
				}
				
			}
			else
			{
				_button.Disabled = false;
				_button.Modulate = _normalColor;
				_towerLabel.Modulate = _normalColor;
			}
		}

		private void OnHover()
		{
			
			_background.Color = _hoverColour;
		}
		
		private void OnHoverEnd()
		{
			_background.Color = _backgroundColour;
		}
		

		private void OnMouseEnter()
		{
			if(_isActive)
				OnHover();
		}


		private void OnMouseExit()
		{
			if(_isActive)
				OnHoverEnd();
		}
		

		private void OnGuiInput(InputEvent @event)
		{
			if (!_isActive) return;
			if (@event.IsActionPressed("picked_up"))
			{
				if (@event is InputEventMouseButton mouseButton && TowerType != null)
				{
					if (GameInfo.GameCurrency.CanAfford(TowerType.Cost))
						_towerManager.CreateTower(TowerType, mouseButton.GlobalPosition);
					else
					{
						_background.Color = GameInfo.InvalidColour;
						_button.TexturePressed = _normalTexture;
					}
				}
			}

			if (!@event.IsActionReleased("picked_up")) return;
			_background.Color = _backgroundColour;
			_button.TexturePressed = _buyingTexture;

		}


		public void DisableButton(bool flag)
		{
			_isActive = !flag;
		}
	}
}












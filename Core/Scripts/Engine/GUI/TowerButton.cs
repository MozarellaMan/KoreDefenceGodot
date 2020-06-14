using Godot;
using KoreDefenceGodot.Core.Scripts.Tower;

namespace KoreDefenceGodot.Core.Scripts.Engine.GUI
{
	public class TowerButton : MarginContainer
	{
		private Label _priceLabel =  null!;
		private TextureButton _button = null!;
		private Label _towerLabel = null!;
		private TowerType? _type;
		private ColorRect _background = null!;
		private readonly Color _hoverColour = Color.Color8(46, 46, 46,200);
		private readonly Color _backgroundColour =  new Color(1, 1, 1,0);

		public void Setup(TowerType type) => _type = type;


		public override void _Ready()
		{
			_button = GetNode("VBoxContainer").GetNode<TextureButton>("Tower");
			_priceLabel = GetNode("VBoxContainer").GetNode("PriceContainer").GetNode("PriceLabel").GetNode("Info").GetNode<Label>("Price");
			_towerLabel = _button.GetNode("TowerInfo").GetNode("VBoxContainer").GetNode<Label>("TowerLabel");
			_background = GetNode<ColorRect>("ColorRect");
			if (_type == null) return;
			_button.TextureNormal = GD.Load<Texture>(_type?.IconPath);
			_towerLabel.Text = _type?.Name;
			_priceLabel.Text = _type?.Cost.ToString();
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
			OnHover();
		}


		private void OnMouseExit()
		{
			OnHoverEnd();
		}

	}
}









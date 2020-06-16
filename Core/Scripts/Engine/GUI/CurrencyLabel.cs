using Godot;
using KoreDefenceGodot.Core.Scripts.Engine.Game;

namespace KoreDefenceGodot.Core.Scripts.Engine.GUI
{
    public class CurrencyLabel : ColorRect
    {
        private Label _label = null!;
        private int _tempNum;

        public override void _Ready()
        {
            _label = GetNode<Label>("Info/Currency");
            _tempNum = GameInfo.GameCurrency.Coins;
            
        }

        public override void _Process(float delta)
        {
            _label.Text = GameInfo.GameCurrency.CurrencyString;
        }
    }
}
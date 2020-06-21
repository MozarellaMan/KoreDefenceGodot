using System.Linq;
using Godot;
using KoreDefenceGodot.Core.Scripts.Engine.Game;
using KoreDefenceGodot.Core.Scripts.Tower;

namespace KoreDefenceGodot.Core.Scripts.Engine.GUI
{
    public class GUIManager : Node
    {
        private MarginContainer _uiScreen = null!;
        private static VBoxContainer _shopList = null!;
        private MarginContainer _infoSection = null!;
        public static Button StartButton = null!;
        private static PackedScene _towerButton = null!;
        private static TowerManager? _towerManager = null!;


        public override void _Ready()
        {
            _uiScreen = GetParent().GetNode("GUILayer").GetNode<MarginContainer>("UIScreen");
            _shopList = _uiScreen.GetNode<VBoxContainer>(
                "HBoxContainer/SideMenu/Shop/Sections/VBoxContainer/ShopItemsScrollContainer/ShopItems");
            _infoSection =
                _uiScreen.GetNode<MarginContainer>("HBoxContainer/SideMenu/Shop/Sections/VBoxContainer/InfoSection");
            _towerButton = GD.Load<PackedScene>("res://Data/Scenes/GUI/ShopElements/TowerButton.tscn");
            _towerManager = GetParent().GetNode("Level").GetNode("TowerManager") as TowerManager;
            StartButton =
                _uiScreen.GetNode<Button>(
                    "HBoxContainer/SideMenu/Shop/Sections/VBoxContainer/InfoSection/VBoxContainer/StartButton");
        }

        public static void SetupTowerShop()
        {
            GameUtil.ClearChildren(_shopList);
            TowerType.Types.ForEach(type =>
            {
                if (!(_towerButton.Instance() is TowerButton button)) return;
                if (_towerManager != null) 
                    button.Setup(type, _towerManager);
                _shopList.AddChild(button);
            });
            foreach (var towerButton in _shopList.GetChildren().OfType<TowerButton>())
            {
                if (towerButton.TowerType != null && GameInfo.GameCurrency.CanAfford(towerButton.TowerType.Cost))
                {
                    towerButton.Locked = false;
                }
            }
        }
        
        
    }
}
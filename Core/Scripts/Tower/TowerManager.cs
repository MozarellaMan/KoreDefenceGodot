using System.Collections.Generic;
using System.Linq;
using Godot;
using KoreDefenceGodot.Core.Scripts.Engine.Game;
using KoreDefenceGodot.Core.Scripts.Tower.Towers;
using static KoreDefenceGodot.Core.Scripts.Tower.TowerType;


namespace KoreDefenceGodot.Core.Scripts.Tower
{
    public class TowerManager : Node2D
    {
        private List<BaseTower> _towers =  null!;
        private readonly Dictionary<TowerType, string?> _towerResources = new Dictionary<TowerType, string?>
        {
            {Catapult, null},
            {TowerType.Firemaster, "res://Data/Scenes/Tower/Firemaster.tscn"},
            {Geyser, null},
            {BlueSunArrow,null},
            {FironSphere, null}
        };
        

        public override void _Ready()
        {
            _towers = new List<BaseTower>();
            foreach (var child in GetChildren())
                if (child is BaseTower tower)
                    _towers.Add(tower);
            GameInfo.TowerList = _towers;
        }

        public override void _Process(float delta)
        {
            _towers = _towers.Where(tower => tower != null).ToList();
        }

        public void BuyTower(TowerType type, Vector2 pos)
        {
            if (_towerResources[type] is null)
            {
                GD.PrintErr("Not implemented!");
                return;
            }

            var newTowerResource = GD.Load<PackedScene>(_towerResources[type]);

            var newTower = type.Enum switch
            {
                TowerEnum.Firemaster => newTowerResource.Instance() as Firemaster,
                _ => null
            };

            if (newTower == null)
            {
                GD.PrintErr("failed to instance tower!");
                return;
            }
            newTower.Setup(DefaultTowerState.Buying);
            AddChild(newTower);
            newTower.Position = pos;
            newTower.Purchased = true;
            _towers.Add(newTower);
            //GD.Print(GetChildren());
        }
    }
}
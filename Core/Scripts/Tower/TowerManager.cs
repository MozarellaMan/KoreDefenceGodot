using System.Collections.Generic;
using System.Linq;
using Godot;
using KoreDefenceGodot.Core.Scripts.Engine.Game;
using KoreDefenceGodot.Core.Scripts.Tower.Towers;
using static KoreDefenceGodot.Core.Scripts.Tower.TowerType;


namespace KoreDefenceGodot.Core.Scripts.Tower
{
    /// <summary>
    ///     Responsible for removing, adding, and updating towers in the game world.
    /// </summary>
    public abstract class TowerManager : Node2D
    {
        private List<BaseTower> _towers =  null!;
        private readonly Dictionary<TowerType, string?> _towerResources = new()
        {
            {TowerType.Catapult, "res://Data/Scenes/Tower/Catapult.tscn"},
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
            // filter and remove deleted towers
            _towers.Where(tower => tower.IsToBeDeleted).ToList().ForEach(tower => tower.QueueFree());
            _towers = _towers.Where(tower => tower.IsToBeDeleted == false).ToList();
        }

        /// <summary>
        ///     Lock/Unlock all the towers managed by the tower manager
        /// </summary>
        /// <param name="flag">true to lock all towers, false to unlock</param>
        public void LockTowers(bool flag)
        {
            _towers.ForEach( tower => tower.Locked = flag);
        }

        /// <summary>
        ///     Creates a tower of the given type at the given position
        /// </summary>
        /// <param name="type">the type of tower</param>
        /// <param name="pos">the tower position</param>
        public void CreateTower(TowerType type, Vector2 pos)
        {
            if (_towerResources[type] is null)
            {
                GD.PrintErr("Not implemented!");
                return;
            }

            var newTowerResource = GD.Load<PackedScene>(_towerResources[type]);

            BaseTower? newTower = type.Enum switch
            {
                TowerEnum.Catapult => newTowerResource.Instance() as Catapult,
                TowerEnum.Firemaster => newTowerResource.Instance() as Firemaster,
                // TODO : Implement Geyser Tower 
                // TODO : Implement Blue Sun Arrow Tower 
                // TODO : Implement Firon Tower 
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
            GameInfo.TowerList = _towers;
        }
    }
}
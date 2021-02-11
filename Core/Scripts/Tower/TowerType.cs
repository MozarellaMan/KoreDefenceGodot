using System.Collections.Generic;

namespace KoreDefenceGodot.Core.Scripts.Tower
{
    public class TowerType
    {
        public enum TowerEnum
        {
            Catapult,
            Firemaster,
            Geyser,
            BlueSunArrow,
            Firon
        }
        
        public static readonly TowerType Catapult = new (
            50,
            250,
            1.9f,
            300,
            1,
            "res://Data/Assets/towers/icons/catapult.png",
            "Rock Catapult",
            "res://Data/Scenes/Tower/Projectiles/CatapultRock.tscn",
            "An advanced device dating back to 4th century BC. You have re-purposed and upgraded it to handle the heat of volcanic rocks infused with Firon. It also includes a selfie function.",
            TowerEnum.Catapult
        );

        public static readonly TowerType Firemaster = new(
            10,
            500,
            5f,
            100,
            1,
            "res://Data/Assets/towers/icons/firemaster.png",
            "Firemaster 3000",
            "res://Data/Scenes/Tower/Projectiles/FiremasterBullet.tscn",
            "After seeing the premiere of critically acclaimed 'Death Robots: 3000', you made this authentic replica of a the movie prop. However, due to legal issues, you were forced to destroy it and create a generic non branded version. The original design's twin barrels and 360-degree movement were retained.",
            TowerEnum.Firemaster
        );

        public static readonly TowerType Geyser = new(
            40,
            900,
            1f,
            960,
            100,
            "res://Data/Assets/towers/icons/geyser.png",
            "Geyser Tower",
            "",
            "The immense Firon energy in the environment ended up creating powerful geysers which you realised could be controlled and manipulate for both awesomely destructive (and recreational) use.",
            TowerEnum.Geyser
        );

        public static readonly TowerType BlueSunArrow = new(
            250,
            1750,
            .5f,
            350,
            1,
            "res://Data/Assets/towers/icons/bluesun.png",
            "Blue Sun Arrow",
            "",
            "Time. Space. Light.\nThere are no rules anymore. All boundaries are breaking down in the wake of an infinite future. The only thing that has stood the test of time - Love, Values and of course, High Accuracy Long Range Weapons.",
            TowerEnum.BlueSunArrow
        );

        public static readonly TowerType FironSphere = new(
            175,
            3500,
            .75f,
            200,
            1,
            "res://Data/Assets/towers/icons/firon.png",
            "Firon Sphere",
            "",
            "Your ultimate creation.\nBillions of Zircon in R&D, and many tears and breakdowns led to this. This is your magnum opus. Concentrated, powerful, piercing Firon energy. Handle with care. ",
            TowerEnum.Firon
            );

        public static readonly List<TowerType> Types = new() {Catapult, Firemaster, Geyser, BlueSunArrow, FironSphere};


        private TowerType(int damage, int cost, float fireRate, int attackRadius, int collateral, string iconPath,
            string name, string projectilePath, string description, TowerEnum @enum)
        {
            Damage = damage;
            Cost = cost;
            FireRate = fireRate;
            AttackRadius = attackRadius;
            Collateral = collateral;
            IconPath = iconPath;
            Name = name;
            ProjectilePath = projectilePath;
            Description = description;
            Enum = @enum;
        }

        public int Damage { get; }
        public int Cost { get; }
        public float FireRate { get; }
        public int AttackRadius { get; }
        public int Collateral { get; }
        public string IconPath { get; }
        public string Name { get; }
        public string ProjectilePath { get; }
        private string Description { get; }
        
        public TowerEnum Enum { get; }
    }
}
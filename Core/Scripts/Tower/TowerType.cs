using System.Collections.Generic;

namespace KoreDefenceGodot.Core.Scripts.Tower
{
    public class TowerType
    {
        
        public static readonly TowerType Catapult = new TowerType(
            50,
            250,
            1.5f,
            300,
            1,
            "res://Data/Assets/towers/icons/catapult.png",
            "Rock Catapult",
            "",
            "An advanced device dating back to 4th century BC. You have re-purposed and upgraded it to handle the heat of volcanic rocks infused with Firon. It also includes a selfie function."
        );

        public static readonly TowerType Firemaster = new TowerType(
            10,
            500,
            5f,
            100,
            1,
            "res://Data/Assets/towers/icons/firemaster.png",
            "Firemaster 3000",
            "res://Data/Scenes/Tower/Projectiles/FiremasterBullet.tscn",
            "After seeing the premiere of critically acclaimed 'Death Robots: 3000', you made this authentic replica of a the movie prop. However, due to legal issues, you were forced to destroy it and create a generic non branded version. The original design's twin barrels and 360-degree movement were retained."
        );

        public static readonly TowerType Geyser = new TowerType(
            40,
            900,
            1f,
            960,
            100,
            "res://Data/Assets/towers/icons/geyser.png",
            "Geyser Tower",
            "",
            "The immense Firon energy in the environment ended up creating powerful geysers which you realised could be controlled and manipulate for both awesomely destructive (and recreational) use."
        );

        public static readonly TowerType BlueSunArrow = new TowerType(
            250,
            1750,
            .5f,
            350,
            1,
            "res://Data/Assets/towers/icons/bluesun.png",
            "Blue Sun Arrow",
            "",
            "Time. Space. Light.\nThere are no rules anymore. All boundaries are breaking down in the wake of an infinite future. The only thing that has stood the test of time - Love, Values and of course, High Accuracy Long Range Weapons."
        );

        public static readonly TowerType FironSphere = new TowerType(
            175,
            3500,
            .75f,
            200,
            1,
            "res://Data/Assets/towers/icons/firon.png",
            "Firon Sphere",
            "",
            "Your ultimate creation.\nBillions of Zircon in R&D, and many tears and breakdowns led to this. This is your magnum opus. Concentrated, powerful, piercing Firon energy. Handle with care. "
        );

        public static readonly List<TowerType> Types = new List<TowerType> {Catapult, Firemaster, Geyser, BlueSunArrow, FironSphere};


        private TowerType(int damage, int cost, float fireRate, int attackRadius, int collateral, string iconPath,
            string name, string projectilePath, string description)
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
        }

        public int Damage { get; }
        public int Cost { get; }
        public float FireRate { get; }
        public int AttackRadius { get; }
        public int Collateral { get; }
        public string IconPath { get; }
        public string Name { get; }
        public string ProjectilePath { get; }
        public string Description { get; }
    }
}
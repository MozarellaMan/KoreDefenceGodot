namespace KoreDefenceGodot.Core.Scripts.Enemy
{
    public class EnemyType
    {
        public int Health { get; }
        public float Speed { get; }
        public int Damage { get; }
        public float AttackRate { get; }
        public string Name { get; }
        public string Description { get; }

        private EnemyType(string name, int health, float speed, int damage, float attackRate, string description)
        {
            Name = name;
            Health = health;
            Speed = speed;
            Damage = damage;
            AttackRate = attackRate;
            Description = description;
        }
        
        public static readonly EnemyType Koreman = new EnemyType("koreman", 400, 130, 5, .5f, "No one really knows where Kore finds her hired mercenaries. However, sources say that the interview process involves piranhas, unchained lions and a live studio audience.");
        public static readonly EnemyType KoreProtector = new EnemyType("koreprotector",2750, 90, 12, .5f, "Half human, half-rhino - they were created with a fierce dedication to protecting Kore and her security forces. A side-effect of this protective instinct has made them very effective at stopping bullying and harassment within the force ranks - many are actually HR representatives.");
        public static readonly EnemyType Korechanic = new EnemyType("korechanic", 1500, 120, 12, .5f, "Unappreciated and overworked - Kore hires her engineers from the most prestigious universities around the world. They spend years looking at complicated, labyrinthine and often contradictory requirements specifications. As you can imagine, this has made them quite irritable.");
        public static readonly EnemyType Volcanologist = new EnemyType("volcanologist",3500, 80, 12, .5f, "Kore 'found' some innocent volcanologists trialling new heat resistant suits connected to a monitoring device. They were promptly 'hired' and given 'research upgrades'. It is not currently known what lives inside the suits.");
        public static readonly EnemyType Machomaniac = new EnemyType("machomaniac",1200, 85, 12, .5f, "Some of the Koremen decided to start working on themselves. This resulted in many of them becoming gym rats, high and crazed from Kore made protein shakes. When they're not making gains, they're making blood stains.");
        public static readonly EnemyType Kore = new EnemyType("kore",50000,60,24,.5f,"kore herself");
    }
    
}
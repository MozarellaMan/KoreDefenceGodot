using System;
using KoreDefenceGodot.Core.Scripts.Enemy;
using KoreDefenceGodot.Core.Scripts.Tower;

namespace KoreDefenceGodot.Core.Scripts.Engine.Game
{
    // Currency is a bit of a misnomer, I think "Funds" would be better in this context
    // but I am keeping the name to correspond to the original Java code.
    public class Currency
    {
        public int Coins
        {
            get => _coins;
            private set
            {
                _coins = value;
                CurrencyString = $"Zircon: {_coins}";
            }
        }

        public string CurrencyString = "Zircon: ";
        private int _coins;

        public Currency(int initialCoins = 0) => Coins = initialCoins;

        /// <summary>
        ///     Whether the player can afford a specific tower
        /// </summary>
        /// <param name="t">Tower the user is trying to purchase</param>
        /// <returns>True if the user can afford the tower</returns>
        public bool CanAfford(BaseTower t) => t.TowerType.Cost <= Coins;

        /// <summary>
        ///     Whether the player has a certain amount of currency
        /// </summary>
        /// <param name="cost">the amount of currency to check</param>
        /// <returns>true if it can be afforded</returns>
        public bool CanAfford(int cost) => cost <= Coins;

        public bool PurchaseTower(BaseTower tower)
        {
            if (!CanAfford(tower)) return false;
            // TODO : Play purchasing sound when tower is purchased
            Coins -= tower.TowerType.Cost;
            tower.Purchased = true;
            return true;
        }


        public void EnemyKillBonus(BaseEnemy enemy)
        {
            var enemyMultiplier = enemy.EnemyType.Health / 10;
            
            // ensures a maximum of 100 zircon for a enemy death
            enemyMultiplier = Math.Min(100, enemyMultiplier);
            
            Coins += enemyMultiplier;
        }

        public void AddCoins(int amount)
        {
            Coins += amount;
            // TODO: Update tower shop display with new values 
        }
    }
}
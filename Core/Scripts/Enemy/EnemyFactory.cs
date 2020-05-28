using System;
using System.Linq;
using Godot;
using KoreDefenceGodot.Core.Scripts.Player;
using static KoreDefenceGodot.Core.Scripts.Enemy.EnemyType;

namespace KoreDefenceGodot.Core.Scripts.Enemy
{
	public class EnemyFactory : Node2D
	{
		/// <summary>
		/// 	Maximum number of enemies
		/// </summary>
		private int _enemyLimit;
		/// <summary>
		/// 	Current number of enemies spawned
		/// </summary>
		/// <returns></returns>
		private int _numEnemies;
		/// <summary>
		/// 	Defines what enemies will appear in this wave, the types and frequency
		/// </summary>
		private EnemyPoolWave[] _enemyPool;

		private float _spawnDelay;
		private float _previousSpawnDelay;

		public void  Setup(float spawnDelay, EnemyPoolWave[] enemyPool)
		{
			_spawnDelay = spawnDelay;
			_enemyPool = enemyPool;
			// add all wave enemy amounts to get total enemies
			_previousSpawnDelay = spawnDelay;
		}

		private void CreateEnemy(Path gamePath, PlayerBase playerBase, EnemyType type)
		{
			if (_numEnemies < _enemyLimit)
			{
				_numEnemies++;
				if (type == EnemyType.Koreman)
				{
					
				}
			}
		}
	}
}

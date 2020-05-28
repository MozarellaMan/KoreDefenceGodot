using System;
using System.Linq;
using Godot;
using KoreDefenceGodot.Core.Scripts.Player;
using Path = KoreDefenceGodot.Core.Scripts.Engine.Tiles.Path;

namespace KoreDefenceGodot.Core.Scripts.Enemy
{
	public abstract class EnemyFactory : Node2D
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

		public float SpawnDelay { get; private set; }
		private float _previousSpawnDelay;

		public void  Setup(float spawnDelay, EnemyPoolWave[] enemyPool)
		{
			SpawnDelay = spawnDelay;
			_enemyPool = enemyPool;
			// add all wave enemy amounts to get total 
			_enemyLimit = enemyPool.Select(wave => wave.Amount).Sum();
			_previousSpawnDelay = spawnDelay;
			
		}

		private BaseEnemy CreateEnemy(Path gamePath, PlayerBase playerBase, EnemyType type)
		{
			if (_numEnemies < _enemyLimit)
			{
				_numEnemies++;
				return LoadEnemy(gamePath, playerBase, type);
			}

			GD.PrintErr("Enemy limit reached!");
			return null;
		}

		public BaseEnemy CreateEnemies(Path gamePath, PlayerBase playerBase)
		{
			var cumulativePoolTotal = 0;
			if (_numEnemies >= _enemyLimit) return null;
			foreach (var poolWave in _enemyPool)
			{
				cumulativePoolTotal += poolWave.Amount;
				// e.g. if numEnemies = 0 and the number of enemies in the first pool is 1,
				if (_numEnemies >= cumulativePoolTotal) continue;
				// checks if last enemy in pool
				SpawnDelay = _numEnemies == cumulativePoolTotal - 1 ? poolWave.Delay : _previousSpawnDelay;

				return CreateEnemy(gamePath, playerBase, poolWave.EnemyType);
			}
			
			return null;
		}

		public bool CanSpawnMoreEnemies()
		{
			return _numEnemies < _enemyLimit;
		}

		private static BaseEnemy LoadEnemy(Path gamePath, PlayerBase playerBase, EnemyType type)
		{
			var newEnemy = GD.Load<PackedScene>("res://Data/Scenes/Enemy/Enemy.tscn")
				.Instance() as BaseEnemy;
			
			newEnemy?.Setup(gamePath,playerBase,type);

			return newEnemy;
		}
	}
}

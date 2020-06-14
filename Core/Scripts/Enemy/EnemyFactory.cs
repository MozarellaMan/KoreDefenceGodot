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
		///     Defines what enemies will appear in this wave, the types and frequency
		/// </summary>
		private EnemyPoolWave[] _enemyPool = null!;

		/// <summary>
		///     Current number of enemies spawned
		/// </summary>
		/// <returns></returns>
		private int _numEnemies;

		private float _previousSpawnDelay;

		/// <summary>
		///     Maximum number of enemies
		/// </summary>
		public int EnemyLimit { get; private set; }

		public float SpawnDelay { get; private set; }

		public void Setup(float spawnDelay, EnemyPoolWave[] enemyPool)
		{
			SpawnDelay = spawnDelay;
			_enemyPool = enemyPool;
			// add all wave enemy amounts to get total 
			EnemyLimit = enemyPool.Select(wave => wave.Amount).Sum();
			_previousSpawnDelay = spawnDelay;
		}

		private BaseEnemy? CreateEnemy(Path gamePath, PlayerBase playerBase, EnemyType type)
		{
			if (_numEnemies < EnemyLimit)
			{
				_numEnemies++;
				return LoadEnemy(gamePath, playerBase, type);
			}

			GD.PrintErr("Enemy limit reached!");
			return null;
		}

		public BaseEnemy? CreateEnemies(Path gamePath, PlayerBase playerBase)
		{
			var cumulativePoolTotal = 0;
			if (_numEnemies >= EnemyLimit) return null;
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
			return _numEnemies < EnemyLimit;
		}

		private static BaseEnemy LoadEnemy(Path gamePath, PlayerBase playerBase, EnemyType type)
		{
			var newEnemy = GD.Load<PackedScene>("res://Data/Scenes/Enemy/Enemy.tscn")
				.Instance() as BaseEnemy;

			newEnemy?.Setup(gamePath, playerBase, type);

			return newEnemy ?? throw new Exception("Enemy not instantiated!");
		}
	}
}

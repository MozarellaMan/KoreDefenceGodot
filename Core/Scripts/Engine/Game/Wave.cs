using System.Diagnostics;
using Godot;
using KoreDefenceGodot.Core.Scripts.Enemy;
using KoreDefenceGodot.Core.Scripts.Player;
using Path = KoreDefenceGodot.Core.Scripts.Engine.Tiles.Path;

namespace KoreDefenceGodot.Core.Scripts.Engine.Game
{
	public abstract class Wave : Node2D
	{
		private float _elapsedTime;
		private EnemyFactory _factory = null!;
		private Path _gamePath = null!;
		private PlayerBase _playerBase = null!;

		public bool WaveOver { get; private set; }
		// TODO UI popups
		// TODO Wave start & end sounds

		public void CreateWave()
		{
			var waveCount = GameInfo.GetRawWaveNumber() - 1;
			var currentWave = WaveSpec.WaveNumbers[waveCount];
			if (currentWave == null) return;
			_factory = (GD.Load<PackedScene>("res://Data/Scenes/Enemy/EnemyFactory.tscn")
				.Instance() as EnemyFactory)!;

			_factory?.Setup(currentWave.SpawnDelay, currentWave.EnemyWaves);
			AddChild(_factory);
		}

		public void Setup(Path gamePath, PlayerBase playerBase)
		{
			_gamePath = gamePath;
			_playerBase = playerBase;
			// TODO Load wave sound resources
		}

		public override void _Process(float delta)
		{
			if (_factory.GetChildCount() == 0)
				CreateWave();
		}

		public void StartWave()
		{
			// TODO Play wave sound start
			CreateWave();

			WaveOver = false;
		}

		public void RunWave(float delta)
		{
			_elapsedTime += delta; // keeps wave spawn time
			Debug.Assert(_factory != null, nameof(_factory) + " != null");
			Debug.Assert(_gamePath != null, nameof(_gamePath) + " != null");
			Debug.Assert(_playerBase != null, nameof(_gamePath) + "!= null");
			if (_factory!.CanSpawnMoreEnemies())
			{
				if (_elapsedTime < _factory.SpawnDelay) return;
				if (_gamePath != null && _playerBase != null)
				{
					var newEnemy = _factory.CreateEnemies(_gamePath, _playerBase);
					if (newEnemy != null) _factory.AddChild(newEnemy);
				}

				_elapsedTime = 0;
			}
			else
			{
				// TODO Play wave end sound
				WaveOver = true;
			}
		}
	}
}

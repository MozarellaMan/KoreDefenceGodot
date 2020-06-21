using Godot;
using KoreDefenceGodot.Core.Scripts.Engine.GUI;
using KoreDefenceGodot.Core.Scripts.Engine.State;

namespace KoreDefenceGodot.Core.Scripts.Engine.Game.LevelManager
{
    public class LevelState : IState<Level>
    {
        public virtual void OnEnter(Level entity)
        {
        }

        public virtual void Update(Level entity, float delta)
        {
        }

        public virtual void Draw(Level entity)
        {
        }

        public virtual void HandleInput(Level entity, InputEvent inputEvent)
        {
        }

        public virtual void OnExit(Level entity)
        {
        }
        
        public static readonly LevelState Global = new GlobalState();
        public static readonly LevelState PreWave = new PreWaveState();
        public static readonly LevelState RunningWave = new RunningWaveState();
        public static readonly LevelState Win = new WinState();
        public static readonly LevelState Lose = new LoseState();

        private sealed class GlobalState : LevelState
        {
            public override void OnEnter(Level entity)
            {
                entity.LoadTilesAndPath();
                GUIManager.SetupTowerShop();
            }

            public override void Update(Level entity, float delta)
            {
                if(entity.PlayerBase != null && entity.PlayerBase.IsDead())
                    entity.LevelStateMachine.ChangeState(Lose);
            }
        }

        private sealed class PreWaveState : LevelState
        {
            public override void OnEnter(Level entity)
            {
                GUIManager.EnableTowerShop();
                entity.TowerManager?.LockTowers(false);
            }

            public override void Update(Level entity, float delta)
            {
                if (entity.StartButton != null && entity.StartButton.Pressed)
                {
                    entity.LevelStateMachine.ChangeState(RunningWave);
                }
            }
        }

        private sealed class RunningWaveState : LevelState
        {
            
            public override void OnEnter(Level entity)
            {
                GUIManager.DisableTowerShop();
                entity.TowerManager?.LockTowers(true);
                entity.SetupNextWave();
                GD.Print($"Wave {entity.CurrentWave} starting...");
            }

            public override void Update(Level entity, float delta)
            {
                entity.RunWave(delta);
                if(entity.CurrentWaveCompleted())
                    entity.LevelStateMachine.ChangeState(PreWave);
            }

            public override void OnExit(Level entity)
            {
                GD.Print($"Wave {entity.CurrentWave} ending... Good job!");
            }
        }
        private sealed class WinState : LevelState {}
        private sealed class LoseState : LevelState {}
    }
}
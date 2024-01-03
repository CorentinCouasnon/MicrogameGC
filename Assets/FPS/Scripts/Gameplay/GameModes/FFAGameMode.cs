﻿using Unity.FPS.Game;

namespace FPS.Scripts.Gameplay.GameModes
{
    public class FFAGameMode : GameMode
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            
            EventManager.AddListener<PlayerDeathEvent>(OnPlayerDeath);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            
            EventManager.RemoveListener<PlayerDeathEvent>(OnPlayerDeath);
        }

        protected override void OnAllObjectivesCompleted(AllObjectivesCompletedEvent evt)
        {
            base.OnAllObjectivesCompleted(evt);
            
            var gameOverEvent = Events.GameOverEvent;
            gameOverEvent.Win = true;
            EventManager.Broadcast(gameOverEvent);
        }

        void OnPlayerDeath(PlayerDeathEvent evt)
        {
            var gameOverEvent = Events.GameOverEvent;
            gameOverEvent.Win = false;
            EventManager.Broadcast(gameOverEvent);
        }
    }
}
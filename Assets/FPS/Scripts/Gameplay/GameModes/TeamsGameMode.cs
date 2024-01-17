﻿using System.Collections.Generic;
using Unity.FPS.Game;

namespace FPS.Scripts.Gameplay.GameModes
{
    public class TeamsGameMode : GameMode
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
            
            EndGameClientRpc(new List<Actor>());
        }

        void OnPlayerDeath(PlayerDeathEvent evt)
        {
            EndGameClientRpc(new List<Actor>());
        }
    }
}
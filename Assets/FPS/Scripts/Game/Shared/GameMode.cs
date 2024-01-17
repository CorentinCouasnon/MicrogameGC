﻿using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Unity.FPS.Game
{
    public class GameMode : NetworkBehaviour
    {
        [SerializeField] Transform _transform;

        protected virtual void OnEnable()
        {
            EventManager.AddListener<AllObjectivesCompletedEvent>(OnAllObjectivesCompleted);
        }

        protected virtual void OnDisable()
        {
            EventManager.RemoveListener<AllObjectivesCompletedEvent>(OnAllObjectivesCompleted);
        }

        protected virtual void OnAllObjectivesCompleted(AllObjectivesCompletedEvent evt) { }

        void SetActiveChildren(bool active)
        {
            foreach (GameObject child in _transform)
            {
                child.SetActive(active);
            }
        }
        
        [ContextMenu("Test end game")]
        public void TestLose()
        {
            EndGameClientRpc(new List<Actor>());
        }

        [ClientRpc]
        public void EndGameClientRpc(List<Actor> winners)
        {
            var evt = Events.GameOverEvent;
            evt.Winners = winners;
            EventManager.Broadcast(evt);
        }
    }
}
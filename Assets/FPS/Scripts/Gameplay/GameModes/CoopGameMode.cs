using System.Collections.Generic;
using System.Linq;
using Unity.FPS.Game;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FPS.Scripts.Gameplay.GameModes
{
    public class CoopGameMode : GameMode
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            
            EventManager.AddListener<AllPlayerDeadEvent>(OnAllPlayerDead);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            
            EventManager.RemoveListener<AllPlayerDeadEvent>(OnAllPlayerDead);
        }

        protected override void OnAllObjectivesCompleted(AllObjectivesCompletedEvent evt)
        {
            base.OnAllObjectivesCompleted(evt);

            // switch scene
            // NetworkManager.Singleton.SceneManager.LoadScene("Boss_Room", LoadSceneMode.Additive);
            // reset objectives
            //return;
            
            EndGameClientRpc(-1);
        }

        void OnAllPlayerDead(AllPlayerDeadEvent evt)
        {
            EndGameClientRpc(-1);
        }
    }
}
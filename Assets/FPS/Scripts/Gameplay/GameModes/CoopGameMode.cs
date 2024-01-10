using Unity.FPS.Game;
using Unity.Netcode;
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
            
            var gameOverEvent = Events.GameOverEvent;
            gameOverEvent.Win = true;
            EventManager.Broadcast(gameOverEvent);
        }

        void OnAllPlayerDead(AllPlayerDeadEvent evt)
        {
            var gameOverEvent = Events.GameOverEvent;
            gameOverEvent.Win = false;
            EventManager.Broadcast(gameOverEvent);
        }
    }
}
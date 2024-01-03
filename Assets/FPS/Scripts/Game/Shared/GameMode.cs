using UnityEngine;

namespace Unity.FPS.Game
{
    public class GameMode : MonoBehaviour
    {
        protected virtual void OnEnable()
        {
            EventManager.AddListener<AllObjectivesCompletedEvent>(OnAllObjectivesCompleted);
        }

        protected virtual void OnDisable()
        {
            EventManager.RemoveListener<AllObjectivesCompletedEvent>(OnAllObjectivesCompleted);
        }

        protected virtual void OnAllObjectivesCompleted(AllObjectivesCompletedEvent evt) { }
    }
}
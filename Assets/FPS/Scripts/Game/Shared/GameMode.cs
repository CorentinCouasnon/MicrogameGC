using FPS.Scripts.Game.Managers;
using UnityEngine;

namespace Unity.FPS.Game
{
    public class GameMode : MonoBehaviour
    {
        [SerializeField] Transform _transform;
        [SerializeField] Mode _mode;

        protected virtual void Awake()
        {
            if (GameModeManager.Instance == null)
                return;
            
            if (GameModeManager.Instance.Mode == _mode)
                SetActiveChildren(true);
        }
        
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
    }
}
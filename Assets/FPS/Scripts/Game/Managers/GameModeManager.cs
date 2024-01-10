using System;
using System.Collections.Generic;
using UnityEngine;

namespace FPS.Scripts.Game.Managers
{
    public class GameModeManager : MonoBehaviour
    {
        public static GameModeManager Instance { get; private set; }
        
        public Mode Mode { get; set; }

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    public enum Mode
    {
        FFA,
        Coop,
    }
}
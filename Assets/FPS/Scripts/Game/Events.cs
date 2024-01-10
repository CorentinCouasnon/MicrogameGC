using UnityEngine;

namespace Unity.FPS.Game
{
    // The Game Events used across the Game.
    // Anytime there is a need for a new event, it should be added here.

    public static class Events
    {
        public static ObjectiveUpdateEvent ObjectiveUpdateEvent = new ObjectiveUpdateEvent();
        public static AllObjectivesCompletedEvent AllObjectivesCompletedEvent = new AllObjectivesCompletedEvent();
        public static AllPlayerDeadEvent AllPlayerDeadEvent = new AllPlayerDeadEvent();
        public static GameOverEvent GameOverEvent = new GameOverEvent();
        public static PlayerDeathEvent PlayerDeathEvent = new PlayerDeathEvent();
        public static PlayerEnteredCaptureAreaEvent PlayerEnteredCaptureAreaEvent = new PlayerEnteredCaptureAreaEvent();
        public static PlayerCaptureAreaStayEvent PlayerCaptureAreaStayEvent = new PlayerCaptureAreaStayEvent();
        public static PlayerExitedCaptureAreaEvent PlayerExitedCaptureAreaEvent = new PlayerExitedCaptureAreaEvent();
        public static EnemyKillEvent EnemyKillEvent = new EnemyKillEvent();
        public static PickupEvent PickupEvent = new PickupEvent();
        public static AmmoPickupEvent AmmoPickupEvent = new AmmoPickupEvent();
        public static DamageEvent DamageEvent = new DamageEvent();
        public static DisplayMessageEvent DisplayMessageEvent = new DisplayMessageEvent();
    }

    public class ObjectiveUpdateEvent : GameEvent
    {
        public Objective Objective;
        public string DescriptionText;
        public string CounterText;
        public bool IsComplete;
        public string NotificationText;
    }

    public class AllObjectivesCompletedEvent : GameEvent { }
    
    public class AllPlayerDeadEvent : GameEvent { }

    public class GameOverEvent : GameEvent
    {
        public bool Win;
    }

    public class PlayerDeathEvent : GameEvent { }

    public class PlayerEnteredCaptureAreaEvent : GameEvent
    {
        public Actor Actor;
    }

    public class PlayerExitedCaptureAreaEvent : GameEvent
    {
        public Actor Actor;
    }

    public class PlayerCaptureAreaStayEvent : GameEvent
    {
        public Actor Actor;
        public float DeltaTime;
    }

    public class EnemyKillEvent : GameEvent
    {
        public GameObject Enemy;
        public int RemainingEnemyCount;
    }

    public class PickupEvent : GameEvent
    {
        public GameObject Pickup;
    }

    public class AmmoPickupEvent : GameEvent
    {
        public WeaponController Weapon;
    }

    public class DamageEvent : GameEvent
    {
        public GameObject Sender;
        public float DamageValue;
    }

    public class DisplayMessageEvent : GameEvent
    {
        public string Message;
        public float DelayBeforeDisplay;
    }
}
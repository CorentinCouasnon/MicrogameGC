using System;
using Unity.FPS.Game;
using UnityEngine;

namespace Unity.FPS.Gameplay
{
    public class CaptureArea : MonoBehaviour
    {
        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent<Actor>(out var actor))
            {
                var actorEnteredEvent = Events.PlayerEnteredCaptureAreaEvent;
                actorEnteredEvent.Actor = actor; 
                EventManager.Broadcast(actorEnteredEvent);
            }
        }

        void OnTriggerStay(Collider other)
        {
            if (other.gameObject.TryGetComponent<Actor>(out var actor))
            {
                var areaStayEvent = Events.PlayerCaptureAreaStayEvent;
                areaStayEvent.Actor = actor; 
                areaStayEvent.DeltaTime = Time.fixedDeltaTime; 
                EventManager.Broadcast(areaStayEvent);
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.gameObject.TryGetComponent<Actor>(out var actor))
            {
                var actorExitedEvent = Events.PlayerExitedCaptureAreaEvent;
                actorExitedEvent.Actor = actor; 
                EventManager.Broadcast(actorExitedEvent);
            }
        }
    }
}
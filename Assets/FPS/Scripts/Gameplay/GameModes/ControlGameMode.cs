using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.FPS.Game;
using UnityEngine;

namespace FPS.Scripts.Gameplay.GameModes
{
    public class ControlGameMode : GameMode
    {
        [SerializeField] float _timerInSeconds;
                
        [SerializeField] float _minimumTimeBeforeCounting;
        [SerializeField] int _pointPerSecond;

        List<CaptureAffiliationData> _affiliationsData = new List<CaptureAffiliationData>();
        List<CaptureActorData> _actorsData = new List<CaptureActorData>();

        void Awake()
        {
            Invoke(nameof(OnTimeOver), _timerInSeconds);
        }

        IEnumerator Start()
        {
            var waitForFixedUpdate = new WaitForFixedUpdate();

            while (true)
            {
                yield return waitForFixedUpdate;
                ComputeAffiliationPoints();
                DebugData();
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            
            EventManager.AddListener<PlayerEnteredCaptureAreaEvent>(OnPlayerEnteredCaptureArea);
            EventManager.AddListener<PlayerCaptureAreaStayEvent>(OnPlayerCaptureAreaStay);
            EventManager.AddListener<PlayerExitedCaptureAreaEvent>(OnPlayerExitedCaptureArea);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            
            EventManager.RemoveListener<PlayerEnteredCaptureAreaEvent>(OnPlayerEnteredCaptureArea);
            EventManager.RemoveListener<PlayerCaptureAreaStayEvent>(OnPlayerCaptureAreaStay);
            EventManager.RemoveListener<PlayerExitedCaptureAreaEvent>(OnPlayerExitedCaptureArea);
        }

        void OnTimeOver()
        {
            var max = _affiliationsData.Max(affiliation => affiliation.Points);
            var winner = _affiliationsData.First(affiliation => affiliation.Points == max);
            EndGameClientRpc(winner.Affiliation);
        }

        void OnPlayerEnteredCaptureArea(PlayerEnteredCaptureAreaEvent evt)
        {
            if (GetActorData(evt.Actor) == null)
            {
                _actorsData.Add(new CaptureActorData { Actor = evt.Actor });
            }

            if (GetAffilitationDataFromId(evt.Actor.Affiliation) == null)
            {
                _affiliationsData.Add(new CaptureAffiliationData { Affiliation = evt.Actor.Affiliation });
            }

            Debug.Log(_affiliationsData.Count);

            var actorData = GetActorData(evt.Actor);
            var affiliationData = GetAffilitationDataFromId(evt.Actor.Affiliation);

            if (!affiliationData.Actors.Contains(actorData.Actor))
            {
                affiliationData.Actors.Add(actorData.Actor);
            }

            actorData.IsInside = true;
        }
        
        void OnPlayerCaptureAreaStay(PlayerCaptureAreaStayEvent evt)
        {
            var actorData = GetActorData(evt.Actor);
            actorData.StayInSeconds += evt.DeltaTime;
        }
        
        void OnPlayerExitedCaptureArea(PlayerExitedCaptureAreaEvent evt)
        {
            var actorData = GetActorData(evt.Actor);
            actorData.IsInside = false;
            actorData.StayInSeconds = 0f;
        }

        void ComputeAffiliationPoints()
        {
            var insideActors = _actorsData.Where(actorData => actorData.IsInside).ToList();
            
            // if more than 2 affiliations inside same point, do nothing
            if (insideActors.Select(actorData => actorData.Actor.Affiliation).Distinct().Count() >= 2)
                return;
            
            // if any player stayed for the minimum time before counting, start counting
            if (insideActors.Any(actorData => actorData.StayInSeconds >= _minimumTimeBeforeCounting))
            {
                var affilitationData = GetAffilitationDataFromId(insideActors.First().Actor.Affiliation);
                affilitationData.Points += _pointPerSecond * Time.fixedDeltaTime * Math.Max(1, insideActors.Count - 1);
            }
        }

        void DebugData()
        {
            foreach (var affiliationData in _affiliationsData)
            {
                Debug.Log(affiliationData);
            }

            foreach (var actorData in _actorsData)
            {
                Debug.Log(actorData);
            }
        }

        CaptureAffiliationData GetAffilitationDataFromId(int affilitationId)
        {
            return _affiliationsData.FirstOrDefault(affiliationData => affiliationData.Affiliation == affilitationId);
        }

        CaptureActorData GetActorData(Actor actor)
        {
            return _actorsData.FirstOrDefault(actorData => actorData.Actor == actor);
        }
        
        class CaptureAffiliationData
        {
            public int Affiliation { get; set; }
            public List<Actor> Actors { get; set; } = new List<Actor>();
            public float Points { get; set; }

            public override string ToString()
            {
                return $"{nameof(Affiliation)}: {Affiliation}, {nameof(Actors)}: {Actors}, {nameof(Points)}: {Points}";
            }
        }

        class CaptureActorData
        {
            public Actor Actor { get; set; }
            public bool IsInside { get; set; }
            public float StayInSeconds { get; set; }

            public override string ToString()
            {
                return $"{nameof(Actor)}: {Actor}, {nameof(IsInside)}: {IsInside}, {nameof(StayInSeconds)}: {StayInSeconds}";
            }
        }
    }
}
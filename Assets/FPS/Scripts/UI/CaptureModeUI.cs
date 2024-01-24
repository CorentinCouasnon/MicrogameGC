using System;
using System.Collections.Generic;
using FPS.Scripts.Gameplay.GameModes;
using TMPro;
using Unity.Netcode;
using UnityEngine;

namespace Unity.FPS.UI
{
    public class CaptureModeUI : NetworkBehaviour
    {
        [SerializeField] RectTransform _affiliationContainer;
        [SerializeField] CaptureModeEntry _entryPrefab;
        [SerializeField] TextMeshProUGUI _timerText;
        [SerializeField] ControlGameMode _controlGameMode;

        Dictionary<int, (Color, CaptureModeEntry)> _affiliationsEntries = new Dictionary<int, (Color, CaptureModeEntry)>();

        List<Color> _colors = new List<Color>
        {
            Color.red,
            Color.blue,
            Color.green,
            Color.cyan,
            Color.magenta,
            Color.yellow,
            Color.black,
        };

        public override void OnNetworkSpawn()
        {
            if (!IsHost)
                return;

            _controlGameMode.StepCompleted += OnStepCompleted;
        }

        void OnDisable()
        {
            if (!IsHost)
                return;
            
            _controlGameMode.StepCompleted -= OnStepCompleted;
        }

        void OnStepCompleted(float timer, List<ControlGameMode.CaptureAffiliationData> data)
        {
            UpdateTimerClientRpc(timer);

            foreach (var entry in data)
            {
                UpdateEntryClientRpc(entry.Affiliation, entry.Points);
            }
        }

        [ClientRpc]
        void UpdateTimerClientRpc(float timer)
        {
            _timerText.text = $"{timer:#00}";
        }
        
        [ClientRpc]
        void UpdateEntryClientRpc(int affiliationId, float score)
        {
            if (!_affiliationsEntries.TryGetValue(affiliationId, out _))
            {
                var newEntry = Instantiate(_entryPrefab, _affiliationContainer);
                _affiliationsEntries.Add(affiliationId, (_colors[_affiliationsEntries.Count % _colors.Count], newEntry));
                newEntry.SetBackground(_colors[(_affiliationsEntries.Count - 1) % _colors.Count]);
                newEntry.SetAffiliation(affiliationId);
            }

            var affiliation = _affiliationsEntries[affiliationId];
            affiliation.Item2.SetScore(score);
        }
    }
}
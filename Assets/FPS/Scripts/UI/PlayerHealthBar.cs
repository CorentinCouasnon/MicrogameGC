using Unity.FPS.Game;
using Unity.FPS.Gameplay;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace Unity.FPS.UI
{
    public class PlayerHealthBar : NetworkBehaviour
    {
        [Tooltip("Image component dispplaying current health")]
        public Image HealthFillImage;

        [SerializeField] PlayerCharacterController playerCharacterController;
        [SerializeField] Health m_PlayerHealth;

        void Update()
        {
            if (!IsOwner) return;

            // update health bar value
            HealthFillImage.fillAmount = m_PlayerHealth.CurrentHealth / m_PlayerHealth.MaxHealth;
        }
    }
}
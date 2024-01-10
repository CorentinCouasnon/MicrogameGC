using Unity.FPS.Game;
using Unity.Netcode;
using UnityEngine;

namespace Unity.FPS.AI
{
    public class FollowPlayer : NetworkBehaviour
    {
        Transform m_PlayerTransform;
        Vector3 m_OriginalOffset;

        public override void OnNetworkSpawn()
        {
            ActorsManager actorsManager = FindObjectOfType<ActorsManager>();
            if (actorsManager != null)
                m_PlayerTransform = actorsManager.Player.transform;
            else
            {
                enabled = false;
                return;
            }

            m_OriginalOffset = transform.position - m_PlayerTransform.position;
        }

        void LateUpdate()
        {
            transform.position = m_PlayerTransform.position + m_OriginalOffset;
        }
    }
}
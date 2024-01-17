using Unity.Netcode;
using UnityEngine;

public class UnSpawnWFX : NetworkBehaviour
{
    float time;

    public override void OnNetworkSpawn()
    {
        time = GetComponent<ParticleSystem>().main.duration;
    }

    private void Update()
    {
        time -= Time.deltaTime;
        if (time <= 0 && IsServer)
        {
            GetComponent<NetworkObject>().Despawn();
        }
    }
}

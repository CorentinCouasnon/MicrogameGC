using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SpawnPlayer : NetworkBehaviour
{
    [SerializeField] private GameObject prefabPlayer;
    [SerializeField] private List<Transform> _spawnPoints;

    private static List<GameObject> _players = new List<GameObject>();

    public override void OnNetworkSpawn()
    {
        ulong localClientId = NetworkManager.Singleton.LocalClientId;
        SpawnPlayerServerRpc(localClientId);
    }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnPlayerServerRpc(ulong clientID)
    {
        int count = (int)clientID;
        GameObject player = Instantiate(prefabPlayer, _spawnPoints[count].position, Quaternion.identity);
        _players.Add(player);
        player.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientID, true);
    }
}

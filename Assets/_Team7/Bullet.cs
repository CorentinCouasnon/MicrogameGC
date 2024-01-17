using System;
using Unity.FPS.Game;
using Unity.Netcode;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    [SerializeField] float damage = 10f;
    [SerializeField] GameObject vfx;
    [SerializeField] float forceShoot = 200;
    public override void OnNetworkSpawn()
    {
        GetComponent<Rigidbody>().AddForce(forceShoot * transform.forward);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent<Damageable>(out Damageable dmg))
        {
            dmg.InflictDamageClientRpc(damage);
        }

        SpawnVFXServerRpc();

        if (IsServer)
        {
            GetComponent<NetworkObject>().Despawn();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void SpawnVFXServerRpc(ServerRpcParams serverRpcParams = default)
    {
        GameObject newVfx = Instantiate(vfx, transform.position, transform.rotation);
        newVfx.transform.eulerAngles = new Vector3(newVfx.transform.eulerAngles.x, newVfx.transform.eulerAngles.y + 180, newVfx.transform.eulerAngles.z);
        newVfx.GetComponent<NetworkObject>().Spawn();
    }
}   

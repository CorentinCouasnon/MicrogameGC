using Unity.FPS.Game;
using Unity.Netcode;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    [SerializeField] float damage = 10f;
    public override void OnNetworkSpawn()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent<Damageable>(out Damageable dmg))
        {
            dmg.InflictDamageClientRpc(damage);
        }

        GetComponent<NetworkObject>().Despawn();
    }

}

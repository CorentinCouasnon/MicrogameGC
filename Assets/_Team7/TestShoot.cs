using Unity.FPS.Game;
using Unity.Netcode;
using UnityEngine;

public class TestShoot : NetworkBehaviour
{
    [SerializeField] GameObject objTest;
    [SerializeField] ProjectileBase ProjectilePrefab;

    void Update()
    {
        if (!IsOwner) return;

        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("try");
            SpawnBulletServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void SpawnBulletServerRpc()
    {
        Debug.Log("Shoot");
        GameObject objectTest = Instantiate(objTest, transform.position, Quaternion.identity);
        NetworkObject networkObject = objectTest.GetComponent<NetworkObject>();
        networkObject.Spawn(true);

        Vector3 shotDirection = GetShotDirectionWithinSpread(WeaponMuzzle);
        ProjectileBase newProjectile = Instantiate(ProjectilePrefab, WeaponMuzzle.position,
            Quaternion.LookRotation(shotDirection));
        newProjectile.GetComponent<NetworkObject>().Spawn();
        newProjectile.Shoot(this);
    }
}

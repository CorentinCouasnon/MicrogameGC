using Unity.FPS.Game;
using Unity.Netcode;
using UnityEngine;

public class TestShoot : NetworkBehaviour
{
    [SerializeField] GameObject objTest;
    public ProjectileBase ProjectilePrefab;

    [SerializeField] GameObject cameraGO;
    [SerializeField] GameObject weapon;
    [SerializeField] GameObject weaponShootPoint;



    public override void OnNetworkSpawn()
    {

    }

    void Update()
    {
        if (!IsOwner) return;

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            SpawnBulletServerRpc();
        }
        weapon.transform.forward = cameraGO.transform.forward;
    }

    [ServerRpc(RequireOwnership = false)]
    void SpawnBulletServerRpc(ServerRpcParams serverRpcParams = default)
    {
        Vector3 newPosTest = new Vector3(weaponShootPoint.transform.position.x, weaponShootPoint.transform.position.y, weaponShootPoint.transform.position.z);
        GameObject newProjectileTest = Instantiate(objTest, newPosTest, weapon.transform.rotation);
        newProjectileTest.GetComponent<NetworkObject>().Spawn();
    }
}

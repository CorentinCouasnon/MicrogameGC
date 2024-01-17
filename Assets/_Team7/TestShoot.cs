using Unity.FPS.Game;
using Unity.Netcode;
using UnityEngine;

public class TestShoot : NetworkBehaviour
{
    [SerializeField] GameObject objTest;
    public ProjectileBase ProjectilePrefab;

    GameObject cameraGO;
    GameObject weapon;

    [SerializeField] float forceShoot = 200;

    public override void OnNetworkSpawn()
    {
        cameraGO = transform.Find("Main Camera").gameObject;
        weapon = cameraGO.transform.Find("Weapon_Blaster").gameObject;   
    }

    void Update()
    {
        if (!IsOwner) return;

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            SpawnBulletServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void SpawnBulletServerRpc(ServerRpcParams serverRpcParams = default)
    {
        Vector3 newPosTest = new Vector3(weapon.gameObject.transform.position.x, weapon.gameObject.transform.position.y, weapon.gameObject.transform.position.z);
        GameObject newProjectileTest = Instantiate(objTest
                                                    , newPosTest
                                                   , cameraGO.transform.rotation);
        newProjectileTest.GetComponent<NetworkObject>().Spawn();
        newProjectileTest.GetComponent<Rigidbody>().AddForce(forceShoot * cameraGO.transform.forward);
    }
}

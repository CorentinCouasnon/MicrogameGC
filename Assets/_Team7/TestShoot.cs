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
        //if (!IsOwner) return;

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
        // TEST  //////
        Debug.Log("salut");
        Vector3 newPosTest = new Vector3(weapon.gameObject.transform.position.x, weapon.gameObject.transform.position.y, weapon.gameObject.transform.position.z);
        GameObject newProjectileTest = Instantiate(objTest
                                                    , newPosTest
                                                   , cameraGO.transform.rotation);
        newProjectileTest.GetComponent<NetworkObject>().Spawn();
        newProjectileTest.GetComponent<Rigidbody>().AddForce(forceShoot * cameraGO.transform.forward);

        // TIR FPS  //////
        //ulong clientId = serverRpcParams.Receive.SenderClientId;
        //if (NetworkManager.ConnectedClients.ContainsKey(clientId))
        //{
        //    var client = NetworkManager.ConnectedClients[clientId];

        //    Vector3 newPos = new Vector3(weaponController.gameObject.transform.position.x, weaponController.gameObject.transform.position.y, weaponController.gameObject.transform.position.z);
        //    ProjectileBase newProjectile = Instantiate(ProjectilePrefab
        //                                                , newPos
        //                                                , cameraGO.transform.rotation);
        //    NetworkObject networkProj = newProjectile.GetComponent<NetworkObject>();
        //    networkProj.Spawn();
        //    newProjectile.Shoot(weaponController);
        //    SpawnBulletClientRpc(networkProj.NetworkObjectId);
        //}
    }

    [ClientRpc]
    void SpawnBulletClientRpc(ulong id)
    {
        NetworkObject netObj = NetworkManager.Singleton.SpawnManager.SpawnedObjects[id];
    }
}

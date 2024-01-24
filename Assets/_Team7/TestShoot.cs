using Unity.FPS.Game;
using Unity.FPS.Gameplay;
using Unity.Netcode;
using UnityEngine;

public class TestShoot : NetworkBehaviour
{
    [SerializeField] GameObject objTest;
    public ProjectileBase ProjectilePrefab;

    [SerializeField] GameObject cameraGO;
    [SerializeField] GameObject weapon;
    [SerializeField] GameObject weaponShootPoint;

    AudioSource audioSource;
    public AudioClip shootSfx;

    public float timerShootMax = 1;
    float timerShoot = 0;

    Actor actor;

    public override void OnNetworkSpawn()
    {
        audioSource = GetComponent<AudioSource>();
        actor = GetComponent<Actor>();
    }

    void Update()
    {
        if (!IsOwner) return;

        if (actor.isDead) return;

        timerShoot -= Time.deltaTime;
        if (Input.GetMouseButtonDown(0) && timerShoot <= 0)
        {
            playSoundServerRpc();
            shootServerRpc();
        }
        weapon.transform.forward = cameraGO.transform.forward;
    }

    [ServerRpc(RequireOwnership = false)]
    void shootServerRpc(ServerRpcParams serverRpcParams = default)
    {
        Vector3 newPosTest = new Vector3(weaponShootPoint.transform.position.x, weaponShootPoint.transform.position.y, weaponShootPoint.transform.position.z);
        GameObject newProjectileTest = Instantiate(objTest, newPosTest, weapon.transform.rotation);
        newProjectileTest.GetComponent<NetworkObject>().Spawn();
        shootClientRpc();
    }

    [ClientRpc]
    void shootClientRpc()
    {
        timerShoot = timerShootMax;

    }

    [ServerRpc]
    void playSoundServerRpc()
    {
        playSoundClientRpc();
    }

    [ClientRpc]
    void playSoundClientRpc()
    {
        audioSource.PlayOneShot(shootSfx);
    }
}

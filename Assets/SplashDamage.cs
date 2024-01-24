using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using Unity.FPS.Game;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class SplashDamage : MonoBehaviour
{
    bool DoOnce = true;
    [SerializeField]
    float Damage = 40f;
    private void Start()
    {
        StartCoroutine(Destroyobj());
    }
    private void OnTriggerEnter(Collider other)
    {
        Damageable damageable = other.GetComponent<Damageable>();

        if (damageable)
        {
            if (DoOnce)
            {
                //Debug.Log(damageable.gameObject);
                DoOnce = false;
                damageable.InflictDamageClientRpc(Damage);
                // Debug.Break();
            }
            ////Don't touch the player
            //if (collider.TryGetComponent<NetworkObject>(out NetworkObject networkObject))
            //{
            //    if (networkObject.IsOwner) return;
            //}

        }
    }
    IEnumerator Destroyobj() 
    {
        for (float i = 0; i < 10f; i+=0.2f)
        {
            transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f);
            yield return new WaitForSeconds(0.01f);
        }
        GetComponent<NetworkObject>().Despawn();
        Destroy(gameObject);
        yield return null;
    }
    // point damage
   
}

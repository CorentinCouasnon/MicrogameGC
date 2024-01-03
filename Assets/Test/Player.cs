using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour
{
    void Update()
    {
        if (!IsOwner) return;

        if (Input.GetKeyDown(KeyCode.Z))
        {
            transform.position += transform.up;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            transform.position -= transform.up;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            transform.position -= transform.right;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            transform.position += transform.right;
        }
    }
}

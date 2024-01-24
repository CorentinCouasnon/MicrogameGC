using Unity.FPS.Game;
using Unity.FPS.Gameplay;
using Unity.Netcode;
using UnityEngine;

public class Respawn : NetworkBehaviour
{
    float timeStay = 0;
    [SerializeField] Actor actor;
    [SerializeField] PlayerCharacterController playerCharacterController;

    private void OnTriggerStay(Collider other)
    {
        if(other.TryGetComponent<Actor>(out Actor actorTrigger))
        {
            if(actorTrigger.Affiliation == actor.Affiliation && actorTrigger != actor)
            {
                timeStay += Time.deltaTime;
            }
            if(timeStay >= 3)
            {
                timeStay = 0;
                playerCharacterController.Respawn();
            }
        }    
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SixFPTrashParticle : MonoBehaviour
{
    [SerializeField] GameObject trashDestroyParticle;
    //0 is left, 1 is middle, 2 is right
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] float distanceCheck = 1.0f;

    CombatMove minigame;

    void Start()
    {
        minigame = FindObjectOfType<CombatMove>();
    }

    public void SpawnDestroyParticle(Transform collisionObject)
    {
        float xDistance = transform.position.x - collisionObject.position.x;
        Transform spawnPosition = null; 
        
        //Left 
        if (xDistance > distanceCheck)
        {
            spawnPosition = spawnPoints[0]; 
        }
        //Middle
        else if (xDistance < distanceCheck && xDistance > -distanceCheck)
        {
            spawnPosition = spawnPoints[1];
        }
        //Right
        else if (xDistance < -distanceCheck)
        {
            spawnPosition = spawnPoints[2];
        }

        if (spawnPosition != null)
        {
            Instantiate(trashDestroyParticle, spawnPosition.position, trashDestroyParticle.transform.rotation, minigame.transform); 
        }
    }
}

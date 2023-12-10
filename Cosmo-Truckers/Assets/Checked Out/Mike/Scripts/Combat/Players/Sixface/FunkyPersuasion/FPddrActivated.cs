using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPddrActivated : MonoBehaviour
{
    [SerializeField] float twoPointDistance;
    [SerializeField] float onePointDistance;
    [SerializeField] bool trashZone = false;
    [SerializeField] GameObject dancePose;
    [SerializeField] Transform dancePoseSpawn;
    [SerializeField] Sprite dancePoseSprite; 

    FunkyPersuasion miniGame;

    private void Start()
    {
        miniGame = FindObjectOfType<FunkyPersuasion>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(trashZone && collision.tag == "PlayerHurtable")
        {
            miniGame.Score--;
            Debug.Log(miniGame.Score);

            SixFPTrashParticle particleSpawner = GetComponent<SixFPTrashParticle>();

            if (particleSpawner != null)
            {
                particleSpawner.SpawnDestroyParticle(collision.transform); 
            }

            Destroy(collision.gameObject);
        }
        else if(collision.tag == "PlayerHurtable")
        {
            float distance = Vector2.Distance(collision.transform.position, transform.position);
            if(distance <= twoPointDistance)
            {
                Destroy(collision.gameObject);
                miniGame.Score += 2;
                //Debug.Log("Perfect! Score: " + miniGame.Score);

                //Spawn dance pose on max success
                GameObject thisDancePose = Instantiate(dancePose, dancePoseSpawn.position, Quaternion.identity, miniGame.transform);
                thisDancePose.GetComponent<SixDancePose>().StartDancePose(dancePoseSprite);
            }
            else if(distance <= onePointDistance)
            {
                Destroy(collision.gameObject);
                miniGame.Score += 1;
                //Debug.Log("Good! Score: " + miniGame.Score);
            }
        }
    }
}

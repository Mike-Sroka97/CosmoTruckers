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
    [SerializeField] GameObject fpText;
    [SerializeField] float fpTextSpawnOffset = 1f;

    List<string> collidedArrows = new List<string>();
    Collider2D myCollider; 
    FunkyPersuasion miniGame;

    private void Start()
    {
        miniGame = FindObjectOfType<FunkyPersuasion>();
        myCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SixFPArrow arrow = collision.GetComponent<SixFPArrow>();

        if (arrow != null)
        {
            int score = 1;

            if (trashZone)
            {
                score = 0;
                arrow.ArrowDecreaseScore(miniGame);

                SixFPTrashParticle particleSpawner = GetComponent<SixFPTrashParticle>();

                if (particleSpawner != null)
                {
                    particleSpawner.SpawnDestroyParticle(collision.transform);
                }
            }
            else
            {
                float distance = Vector2.Distance(collision.transform.position, transform.position);
                myCollider.enabled = false;

                if (distance <= twoPointDistance)
                {
                    score = 2;
                    //Spawn dance pose on max success
                    GameObject thisDancePose = Instantiate(dancePose, dancePoseSpawn.position, Quaternion.identity, miniGame.transform);
                    thisDancePose.GetComponent<SixDancePose>().StartDancePose(dancePoseSprite);
                }
                else if (distance <= onePointDistance)
                {
                    score = 1;
                }

                Debug.Log(myCollider.gameObject.name); 
                arrow.ArrowIncreaseScore(miniGame, score);
            }

            Vector3 fpSpawnLocation = new Vector3(collision.transform.position.x, transform.position.y + fpTextSpawnOffset, transform.position.z); 
            GameObject thisSuccessText = Instantiate(fpText, fpSpawnLocation, Quaternion.identity, miniGame.transform);
            thisSuccessText.GetComponent<SixFPText>().StartText(score); 
        }
        else if (arrow == null)
        {
            Debug.Log("Error: arrow is null"); 
        }
    }
}

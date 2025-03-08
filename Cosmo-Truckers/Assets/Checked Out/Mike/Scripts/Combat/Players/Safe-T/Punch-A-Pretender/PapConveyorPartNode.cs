using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PapConveyorPartNode : MonoBehaviour
{
    [SerializeField] GameObject goodParticles; 

    bool badZone;
    PunchAPretender minigame;

    private void Start()
    {
        minigame = FindObjectOfType<PunchAPretender>();

        if(transform.name == "BadZone")
        {
            badZone = true;
        }
        else
        {
            badZone = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!badZone && collision.CompareTag("PlayerAttack"))
        {
            minigame.Score++;
            minigame.CheckSuccess(); 

            if (goodParticles != null)
            {
                Instantiate(goodParticles, transform.position, goodParticles.transform.rotation, null);
            }

            Debug.Log(minigame.Score);
            gameObject.SetActive(false);
        }
    }
}

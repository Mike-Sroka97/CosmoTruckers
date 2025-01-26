using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPPpickup : PlayerPickup
{
    [SerializeField] SPPpickup otherPickup;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (givesScore)
            {
                minigame.Score += score;
                minigame.CheckSuccess();
            }

            if (givesAugmentScore)
            {
                minigame.AugmentScore += score;
                minigame.CheckAugmentSuccess();
            }

            if (collectParticle != null)
            {
                Instantiate(collectParticle, transform.position, collectParticle.transform.rotation, minigame.transform);
            }

            otherPickup.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BriberyPickup : PlayerPickup
{
    [SerializeField] int heal = 5;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (collectParticle != null)
                Instantiate(collectParticle, transform.position, collectParticle.transform.rotation, minigame.gameObject.transform);

            PlayerCharacter lowestHealthCharacter = PlayerVesselManager.Instance.PlayerVessels[0].MyCharacter;

            foreach (PlayerVessel player in PlayerVesselManager.Instance.PlayerVessels)
                if (player.MyCharacter.CurrentHealth / player.MyCharacter.Health < lowestHealthCharacter.CurrentHealth / lowestHealthCharacter.Health)
                    lowestHealthCharacter = player.MyCharacter;

            lowestHealthCharacter.TakeHealing(heal);

            Destroy(gameObject);
        }
    }
}

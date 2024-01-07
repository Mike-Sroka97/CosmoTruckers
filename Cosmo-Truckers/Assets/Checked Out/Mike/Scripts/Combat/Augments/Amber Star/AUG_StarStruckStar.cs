using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AUG_StarStruckStar : TrackPlayerDeath
{
    [SerializeField] GameObject starExplosion;
    [SerializeField] float minY = -5.3f;
    [SerializeField] int healAmount;

    bool dying = false;
    AUG_StarStruckSpawner starSpawner;

    private void Update()
    {
        TrackYPosition();
    }

    private void TrackYPosition()
    {
        if (transform.position.y <= minY && !dying)
        {
            dying = true;
            GameObject star = Instantiate(starExplosion, transform);
            //star.transform.parent = null;
            GetComponentInChildren<SpriteRenderer>().enabled = false;
            GetComponent<MoveForward>().enabled = false;
            Destroy(transform.parent.gameObject, 1f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!TrackingDamage)
            return;

        if (collision.tag == "Player" && collision.GetComponent<PlayerBody>())
        {
            Player player = collision.transform.GetComponent<PlayerBody>().Body;

            if (!player.iFrames)
            {
                Death(player);
                HealEnemy();
                player.TakeDamage();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!TrackingDamage)
            return;

        if (collision.tag == "Player" && collision.GetComponent<PlayerBody>())
        {
            Player player = collision.transform.GetComponent<PlayerBody>().Body;

            if (!player.iFrames)
            {
                Death(player);
                HealEnemy();
                player.TakeDamage();
            }
        }
    }

    private void HealEnemy()
    {
        starSpawner = FindObjectOfType<AUG_StarStruckSpawner>();

        if(starSpawner.AliveEnemies.Count > 0)
        {
            int random = Random.Range(0, starSpawner.AliveEnemies.Count);

            starSpawner.AliveEnemies[random] = (starSpawner.AliveEnemies[random].Item1, starSpawner.AliveEnemies[random].Item2 + healAmount);
        }
    }
}

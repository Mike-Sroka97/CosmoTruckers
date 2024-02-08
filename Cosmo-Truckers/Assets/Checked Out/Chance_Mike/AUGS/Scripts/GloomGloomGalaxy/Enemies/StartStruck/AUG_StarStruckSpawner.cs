using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AUG_StarStruckSpawner : Augment
{
    [SerializeField] GameObject star;
    [SerializeField] float firstStarDelay = 5f;

    public List<(Enemy, int)> AliveEnemies = new List<(Enemy, int)>();
    const float xClamp = 5.9f;
    const float minSpawn = 5.0f;
    float spawnSpeed;
    bool firstStar;

    public override void Activate(DebuffStackSO stack = null)
    {
        base.Activate(stack);
        spawnSpeed = minSpawn - StatusEffect;

        //find aliveEnemies
         foreach(Enemy enemy in EnemyManager.Instance.Enemies)
            if (!enemy.Dead)
                AliveEnemies.Add((enemy, 0));

        firstStar = true;

        StartCoroutine(SpawnStar());
    }

    public override void StopEffect()
    {
        foreach((Enemy, int) enemy in AliveEnemies)
        {
            if(!enemy.Item1.Dead)
            {
                Enemy tempEnemy = enemy.Item1;
                tempEnemy.TakeHealing(enemy.Item2);
            }
        }
        AliveEnemies.Clear();
        StopAllCoroutines();

        Destroy(gameObject);
    }

    IEnumerator SpawnStar()
    {
        if(firstStar)
        {
            yield return new WaitForSeconds(firstStarDelay);
            firstStar = false;
        }

        yield return new WaitForSeconds(spawnSpeed);
        float randomX = Random.Range(-xClamp, xClamp);
        GameObject tempStar;

        if (randomX >= 0)
        {
            tempStar = Instantiate(star, new Vector3(randomX, transform.position.y, transform.position.z), star.transform.rotation);
            tempStar.transform.eulerAngles = new Vector3(0, 0, 225);
        }
        else
        {
            tempStar = Instantiate(star, new Vector3(randomX, transform.position.y, transform.position.z), star.transform.rotation);
        }

        tempStar.transform.parent = CombatManager.Instance.GetMiniGame.transform; // this.gameObject.transform;

        StartCoroutine(SpawnStar());
    }
}

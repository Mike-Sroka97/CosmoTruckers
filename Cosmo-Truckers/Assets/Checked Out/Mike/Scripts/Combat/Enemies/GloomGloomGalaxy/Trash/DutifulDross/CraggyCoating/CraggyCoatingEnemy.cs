using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraggyCoatingEnemy : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float moveSpeed;
    [SerializeField] ParticleSystem starTrail;
    [SerializeField] float waitToStartTrail = 1.25f;
    [SerializeField] float spawnRangeY = 4.25f;
    [SerializeField] float spawnRangeX = 6.5f;
    float timer;

    CraggyCoating minigame;

    private void Start()
    {
        RandomizeStartPosition();

        transform.right = target.position - transform.position;
        minigame = FindObjectOfType<CraggyCoating>();
    }

    private void Update()
    {
        if (timer < waitToStartTrail)
        {
            timer += Time.deltaTime;
            starTrail.gameObject.SetActive(false);
        }
        else
            starTrail.gameObject.SetActive(true);

        MoveMe();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("PlayerAttack"))
        {
            UnchildParticle();
            Destroy(gameObject);
        }
        else if(collision.name == "GoodGuy")
        {
            collision.gameObject.GetComponentInChildren<AdvancedFrameAnimation>().SwitchToEmotion(); 
            minigame.AugmentScore++;

            UnchildParticle(); 
            Destroy(gameObject);
        }
    }

    private void RandomizeStartPosition()
    {
        int quadrant = Random.Range(0, 4);
        float randomY = 0;
        float randomX = 0;

        switch (quadrant)
        {
            case 0:
                randomY = Random.Range(-spawnRangeY, spawnRangeY);
                randomX = -spawnRangeX;
                break;
            case 1:
                randomY = Random.Range(-spawnRangeY, spawnRangeY);
                randomX = spawnRangeX;
                break;
            case 2:
                randomY = -spawnRangeY;
                randomX = Random.Range(-spawnRangeX, spawnRangeX);
                break;
            case 3:
                randomY = spawnRangeY;
                randomX = Random.Range(-spawnRangeX, spawnRangeX);
                break;
            default:
                break;
        }

        transform.localPosition = new Vector3(randomX, randomY, transform.localPosition.z);
    }

    void UnchildParticle()
    {
        starTrail.gameObject.transform.parent = GameObject.FindObjectOfType<CombatMove>().transform;
        var main = starTrail.main;
        main.stopAction = ParticleSystemStopAction.Destroy;
    }

    private void MoveMe()
    {
        transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
    }
}

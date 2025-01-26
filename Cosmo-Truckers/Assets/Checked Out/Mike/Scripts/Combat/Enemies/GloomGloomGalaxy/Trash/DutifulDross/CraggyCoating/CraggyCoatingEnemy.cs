using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraggyCoatingEnemy : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float moveSpeed;
    [SerializeField] ParticleSystem starTrail;
    [SerializeField] float waitToStartTrail = 1.25f;
    float timer;

    CraggyCoating minigame;

    private void Start()
    {
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
        if(collision.tag == "PlayerAttack")
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CometkazeBall : MonoBehaviour
{
    [SerializeField] float shrinkDistance;
    [SerializeField] float shrinkSpeed;
    [SerializeField] float moveSpeed;
    [SerializeField] float destroySize;
    [SerializeField] ParticleSystem starTrail;
    
    [SerializeField] float waitToStartTrail = 1.25f;
    float timer; 

    [HideInInspector] public bool Move = false;


    private void Update()
    {
        if (timer < waitToStartTrail)
        {
            timer += Time.deltaTime;
            starTrail.gameObject.SetActive(false);
        }
        else
            starTrail.gameObject.SetActive(true);

        if (!Move)
            return;

        Shrink();
        MoveMe();        
    }

    private void Shrink()
    {
        if(Vector3.Distance(transform.position, Vector3.zero) <= shrinkDistance)
        {
            transform.localScale -= new Vector3(shrinkSpeed, shrinkSpeed, shrinkSpeed);
            if(transform.localScale.x <= destroySize)
            {
                starTrail.gameObject.transform.parent = GameObject.FindObjectOfType<CombatMove>().transform;
                var main = starTrail.main;
                main.stopAction = ParticleSystemStopAction.Destroy;

                Destroy(gameObject);
            }
        }
    }

    private void MoveMe()
    {
        transform.position = Vector3.MoveTowards(transform.position, Vector3.zero, moveSpeed * Time.deltaTime);
    }
}

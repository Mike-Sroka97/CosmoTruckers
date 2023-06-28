using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedSproutFlower : MonoBehaviour
{
    [SerializeField] float showThornsDelay;
    [SerializeField] float thornAttackDelay;

    [SerializeField] float showThornsYScale;
    [SerializeField] float thornAttackYScale;

    [SerializeField] float showThornsSpeed;
    [SerializeField] float thornAttackSpeed;

    [SerializeField] bool lastFlower = false;

    [HideInInspector] public bool TrackTime = false;

    float currentTime = 0;
    Transform thorns;
    bool thornsShown = false;
    bool thornsAttacking = false;
    bool attacking = false;

    SeedSprout minigame;

    private void Start()
    {
        minigame = FindObjectOfType<SeedSprout>();
        thorns = transform.Find("FlowerHurt");
    }

    private void Update()
    {
        UpdateTime();
    }

    private void UpdateTime()
    {
        if(!thornsShown && TrackTime)
        {
            currentTime += Time.deltaTime;
            if(currentTime >= showThornsDelay)
            {
                currentTime = 0;
                thornsShown = true;
                StartCoroutine(ThornsMovement(showThornsYScale, showThornsSpeed));
            }
        }
        else if(thornsAttacking)
        {
            currentTime += Time.deltaTime;
            if(currentTime >= thornAttackDelay)
            {
                currentTime = 0;
                thornsAttacking = false;
                StartCoroutine(ThornsMovement(thornAttackYScale, thornAttackSpeed));
            }
        }
    }

    private IEnumerator ThornsMovement(float yScale, float speed)
    {
        while(thorns.localScale.y < yScale)
        {
            thorns.localScale += new Vector3(0, speed * Time.deltaTime, 0);
            yield return new WaitForSeconds(Time.deltaTime);
        }

        if(!attacking)
        {
            attacking = true;
            thornsAttacking = true;
        }
        else if(!lastFlower)
        {
            minigame.NextFlower();
        }
    }
}

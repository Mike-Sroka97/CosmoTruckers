using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SeedSproutFlower : MonoBehaviour
{
    [SerializeField] float showThornsDelay;
    [SerializeField] float thornAttackDelay;

    [SerializeField] float showThornsYOffset;
    [SerializeField] float thornAttackYOffset;

    [SerializeField] float showThornsSpeed;
    [SerializeField] float thornAttackSpeed;

    [SerializeField] AnimationClip bulbOpen; 

    [SerializeField] bool lastFlower = false;
    [HideInInspector] public bool TrackTime = false;

    Animator myAnimator; 
    float currentTime = 0;
    Transform thorns;
    bool thornsShown = false;
    bool thornsAttacking = false;
    bool attacking = false;
    Vector3 showThornsPosition, thornAttackPosition; 

    SeedSprout minigame;

    private void Start()
    {
        minigame = FindObjectOfType<SeedSprout>();
        thorns = transform.Find("FlowerHurt");
        myAnimator = GetComponent<Animator>(); 

        showThornsPosition = new Vector3(thorns.position.x, thorns.position.y - showThornsYOffset, thorns.position.z);
        thornAttackPosition = new Vector3(thorns.position.x, thorns.position.y - thornAttackYOffset, thorns.position.z);
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
                myAnimator.Play(bulbOpen.name); 
                StartCoroutine(ThornsMovement(showThornsPosition, showThornsSpeed));
            }
        }
        else if(thornsAttacking)
        {
            currentTime += Time.deltaTime;
            if(currentTime >= thornAttackDelay)
            {
                currentTime = 0;
                thornsAttacking = false;
                StartCoroutine(ThornsMovement(thornAttackPosition, thornAttackSpeed));
            }
        }
    }

    private IEnumerator ThornsMovement(Vector3 newPosition, float speed)
    {
        while(thorns.position.y > newPosition.y)
        {
            thorns.position -= new Vector3(0, speed * Time.deltaTime, 0);
            yield return new WaitForSeconds(Time.deltaTime);
        }

        thorns.position = newPosition; 

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

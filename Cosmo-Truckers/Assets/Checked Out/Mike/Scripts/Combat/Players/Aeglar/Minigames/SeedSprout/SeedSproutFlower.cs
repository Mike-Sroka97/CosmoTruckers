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
    Vector3 thornsStartPosition; 

    SeedSprout minigame;

    private void Start()
    {
        minigame = FindObjectOfType<SeedSprout>();
        thorns = transform.Find("FlowerHurt");
        //Make sure it's off so it doesn't show up outside of INA. Thorns are larger than mask
        thorns.gameObject.SetActive(false); 

        myAnimator = GetComponent<Animator>(); 
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
                //Turn thorns on and get position
                thorns.gameObject.SetActive(true);
                thornsStartPosition = thorns.position;

                currentTime = 0;
                thornsShown = true;
                myAnimator.Play(bulbOpen.name);
                StartCoroutine(ThornsMovement(showThornsYOffset, showThornsSpeed));
            }
        }
        else if(thornsAttacking)
        {
            currentTime += Time.deltaTime;
            if(currentTime >= thornAttackDelay)
            {
                currentTime = 0;
                thornsAttacking = false;
                StartCoroutine(ThornsMovement(thornAttackYOffset, thornAttackSpeed));
            }
        }
    }

    private IEnumerator ThornsMovement(float yOffset, float speed)
    {
        Vector3 newPosition = new Vector3(thorns.position.x, thornsStartPosition.y - yOffset, thorns.position.z);

        while (thorns.position.y > newPosition.y)
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

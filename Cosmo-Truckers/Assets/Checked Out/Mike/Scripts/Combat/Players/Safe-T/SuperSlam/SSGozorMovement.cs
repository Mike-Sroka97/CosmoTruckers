using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSGozorMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float distanceCheck;

    //Damaged stuff
    public List<SpriteRenderer> mySprites;
    [SerializeField] float flashDuration;
    [SerializeField] int numberOfFlashes;
    [SerializeField] SSGun[] guns;
    [SerializeField] Collider2D[] collidersToDisable;

    Transform point0;
    Transform point1;
    float originalMoveSpeed;
    bool movingTowardPoint1 = true;
    bool minigameStarted = false;

    private void Start()
    {
        originalMoveSpeed = moveSpeed;
        moveSpeed = 0;
        point0 = transform.parent.Find("GozorPoint (0)");
        point1 = transform.parent.Find("GozorPoint (1)");
    }

    private void Update()
    {
        MoveMe();
    }

    public void StartMinigame()
    {
        if(!minigameStarted)
        {
            minigameStarted = true;
            moveSpeed = originalMoveSpeed;
            Debug.Log("MoveSpeed: " + moveSpeed + " OG: " + originalMoveSpeed);
        }
    }

    private void MoveMe()
    {
        if(movingTowardPoint1)
        {
            transform.position = Vector3.MoveTowards(transform.position, point1.position, moveSpeed * Time.deltaTime);
            if(Vector3.Distance(transform.position, point1.position) < distanceCheck)
            {
                movingTowardPoint1 = !movingTowardPoint1;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, point0.position, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, point0.position) < distanceCheck)
            {
                movingTowardPoint1 = !movingTowardPoint1;
            }
        }
    }

    public IEnumerator FlashMe()
    {
        moveSpeed = 0;
        ToggleGozor(false);

        int currentFlash = 0;
        while(currentFlash < numberOfFlashes)
        {
            foreach (SpriteRenderer sprite in mySprites)
            {
                sprite.enabled = false;
            }
            yield return new WaitForSeconds(flashDuration);

            foreach (SpriteRenderer sprite in mySprites)
            {
                sprite.enabled = true;
            }
            yield return new WaitForSeconds(flashDuration);

            currentFlash++;
        }

        moveSpeed = originalMoveSpeed;
        ToggleGozor(true);
    }

    private void ToggleGozor(bool toggle)
    {
        foreach (SSGun gun in guns)
        {
            gun.enabled = toggle;
        }
        foreach(Collider2D collider in collidersToDisable)
        {
            collider.enabled = toggle;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class SSGozorMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float distanceCheck;

    //Damaged stuff
    public List<SpriteRenderer> mySprites;
    [SerializeField] float flashDuration;
    [SerializeField] int numberOfFlashes;
    [SerializeField] SSGun[] guns;
    public List<Collider2D> collidersToDisable;
    [SerializeField] private float firstTimeLaserWait = 2f; 

    int hitNumber = 0; 
    Transform point0;
    Transform point1;
    float originalMoveSpeed;
    bool movingTowardPoint1 = true;
    bool minigameStarted = false;
    
    [HideInInspector] public AudioDevice myAudioDevice; 
    [HideInInspector] public bool trackTime = false;

    private void Start()
    {
        originalMoveSpeed = moveSpeed;
        moveSpeed = 0;
        point0 = transform.parent.Find("GozorPoint (0)");
        point1 = transform.parent.Find("GozorPoint (1)");
        myAudioDevice = GetComponentInChildren<AudioDevice>();
    }

    private void Update()
    {
        if (!trackTime)
            return;

        MoveMe();
    }

    public void StartMinigame()
    {
        if(!minigameStarted)
        {
            minigameStarted = true;
            moveSpeed = originalMoveSpeed;
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

    public IEnumerator FlashMe(bool firstTime)
    {
        StopLaserSounds(); 

        if (hitNumber == 0)
        {
            moveSpeed = 0;
        }
        else
        {
            moveSpeed = originalMoveSpeed / 2.0f; 
        }

        ToggleGozor(false);
        hitNumber++; 

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

        if (firstTime)
        {
            FirstTimeToggle(); 
        }
        else
        {
            ToggleGozor(true);
        }
    }

    private void ToggleGozor(bool toggle)
    {
        foreach (Collider2D collider in collidersToDisable)
        {
            collider.enabled = toggle;
        }

        foreach (SSGun gun in guns)
        {
            gun.SetLaserState(toggle);
        }
    }

    private void FirstTimeToggle()
    {
        foreach (Collider2D collider in collidersToDisable)
        {
            collider.enabled = true;
        }

        StartCoroutine(FirstTimeLasers());
    }

    private IEnumerator FirstTimeLasers()
    {
        yield return new WaitForSeconds(firstTimeLaserWait);

        foreach (SSGun gun in guns)
        {
            gun.SetLaserState(true);
        }
    }

    /// <summary>
    /// Stops all laser sounds
    /// </summary>
    private void StopLaserSounds()
    {
        // Stop sounds
        myAudioDevice.StopSound("LaserCharge");
        myAudioDevice.StopSound("LaserFire");
        myAudioDevice.StopSound("LaserLoop");
    }

    public void EarlyEndMinigame(Material offMaterial)
    {
        StopLaserSounds(); 

        foreach (SpriteRenderer sprite in mySprites)
        {
            sprite.material = offMaterial;
        }

        foreach (Collider2D collider in collidersToDisable)
        {
            collider.enabled = false;
        }

        foreach (SSGun gun in guns)
        {
            gun.gameObject.SetActive(false); 
        }
    }
}

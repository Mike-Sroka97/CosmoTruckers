using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargeIronClock : MonoBehaviour
{
    [SerializeField] float minRotationSpeed;
    [SerializeField] float maxRotationSpeed;
    [SerializeField] float scoreWaitTime = 1f;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform barrel;
    [SerializeField] SpriteRenderer gunSpriteRender;
    [SerializeField] Sprite firedSprite;
    [SerializeField] GameObject smokeParticles;
    [SerializeField] GameObject gunBreakParticles;
    Transform trigger;

    [Header("Gloom Guy")]
    SpriteRenderer gloomGuy;
    [SerializeField] GameObject gloomGuyFader; 
    [SerializeField] Sprite happySprite, sadSprite; 

    CombatMove minigame;
    float rotationSpeed; //randomize
    float currentDegreesRotated = 0;
    float currentTime = 0;
    float waitBeforeEnding = 2f; 

    bool spinning = true;
    bool trackTime = false;
    public bool PlayerFired = false;
    private float rotationTime;
    float beforeWaitTime; 

    private void Start()
    {
        minigame = FindObjectOfType<CombatMove>();
        rotationSpeed = Random.Range(minRotationSpeed, maxRotationSpeed);
        trigger = GameObject.Find("EnemyGunTrigger").transform; 
        gloomGuy = GameObject.Find("GloomGuy").GetComponent<SpriteRenderer>();
        
        beforeWaitTime = scoreWaitTime / 3.0f;
        rotationTime = 360f / rotationSpeed;

        //direction correction
        rotationSpeed = -rotationSpeed;
    }

    private void Update()
    {
        TrackTime();
        RotateMe();
    }

    private void TrackTime()
    {
        if (!trackTime)
            return;

        currentTime += Time.deltaTime;

        if(currentTime >= scoreWaitTime)
        {
            if(!PlayerFired)
            {
                Instantiate(bullet, barrel.position, barrel.rotation, minigame.transform);
                Instantiate(smokeParticles, barrel.position, barrel.rotation, minigame.transform);
                gunSpriteRender.sprite = firedSprite;
                gloomGuy.sprite = happySprite; 
                trigger.localEulerAngles = new Vector3(0f, 0f, -35f);
                trigger.localPosition = new Vector3(trigger.localPosition.x, -0.325f, trigger.localPosition.z);

                // Max Score = Most Damage to Player
                minigame.Score += 6; 

                StartCoroutine(WaitBeforeEnding());
            }

            trackTime = false;
        }
    }

    private void RotateMe()
    {
        if (!spinning)
            return;

        transform.Rotate(new Vector3(0, 0, rotationSpeed * Time.deltaTime));
        currentDegreesRotated += rotationSpeed * Time.deltaTime;
        rotationTime -= Time.deltaTime; 

        if(currentDegreesRotated <= -360)
        {
            spinning = false;
            transform.eulerAngles = new Vector3(0, 0, 0);
            trackTime = true;
        }
    }

    public void Fire()
    {
        float scoreTime; 

        if((currentTime <= 0 || currentTime > scoreWaitTime) && rotationTime > beforeWaitTime)
        {
            scoreTime = 0;
            gloomGuy.sprite = happySprite;
            GameObject tempFader = Instantiate(gloomGuyFader, gloomGuy.transform.position, gloomGuy.transform.rotation, minigame.transform);
            tempFader.GetComponent<SpawnFadeObject>().StartFading(happySprite); 
        }
        else
        {
            if (currentTime > 0)
                scoreTime = currentTime;
            else
                scoreTime = scoreWaitTime; 

        }

        //max 6
        while (scoreTime > 0)
        {
            minigame.Score += 1;
            scoreTime -= .1f;
        }

        StartCoroutine(WaitBeforeEnding()); 
    }

    public bool TooEarly()
    {
        if (rotationTime > beforeWaitTime)
            return true;
        else
            return false; 
    }

    public void GloomGuySad()
    {
        Instantiate(gunBreakParticles, barrel.position, barrel.rotation, minigame.transform);
        gloomGuy.sprite = sadSprite;
        GameObject tempFader = Instantiate(gloomGuyFader, gloomGuy.transform.position, gloomGuy.transform.rotation, minigame.transform);
        tempFader.GetComponent<SpawnFadeObject>().StartFading(sadSprite, 0);
    }

    private IEnumerator WaitBeforeEnding()
    {
        yield return new WaitForSeconds(waitBeforeEnding);
        minigame.CheckSuccess(true);
    }
}

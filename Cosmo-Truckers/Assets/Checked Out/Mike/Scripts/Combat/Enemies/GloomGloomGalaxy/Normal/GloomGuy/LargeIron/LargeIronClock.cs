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
    Transform trigger;

    [Header("Gloom Guy")]
    SpriteRenderer gloomGuy;
    [SerializeField] Sprite happySprite, sadSprite; 

    CombatMove minigame;
    float rotationSpeed; //randomize
    float currentDegreesRotated = 0;
    float currentTime = 0;

    bool spinning = true;
    bool trackTime = false;
    public bool PlayerFired = false;

    private void Start()
    {
        minigame = FindObjectOfType<CombatMove>();
        rotationSpeed = Random.Range(minRotationSpeed, maxRotationSpeed);
        trigger = GameObject.Find("EnemyGunTrigger").transform; 
        gloomGuy = GameObject.Find("GloomGuy").GetComponent<SpriteRenderer>();

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
                GameObject bulletTemp = Instantiate(bullet, barrel.position, barrel.rotation, minigame.transform);
                GameObject spokeParticle = Instantiate(smokeParticles, barrel.position, barrel.rotation, minigame.transform);
                gunSpriteRender.sprite = firedSprite;
                gloomGuy.sprite = happySprite; 
                trigger.localEulerAngles = new Vector3(0f, 0f, -35f);
                trigger.localPosition = new Vector3(trigger.localPosition.x, -0.325f, trigger.localPosition.z); 
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

        if(currentTime <= 0 || (currentTime > scoreWaitTime && currentDegreesRotated > -350f))
        {
            scoreTime = 0;
            gloomGuy.sprite = happySprite;
        }
        else
        {
            scoreTime = currentTime;
        }

        CombatMove spell = FindObjectOfType<CombatMove>();

        //max 6
        while (scoreTime > 0)
        {
            spell.Score += 1;
            scoreTime -= .1f;
        }
    }

    public void GloomGuySad()
    {
        gloomGuy.sprite = sadSprite;
    }
}

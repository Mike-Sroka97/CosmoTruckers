using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolidShlurping : CombatMove
{
    [Space(20)]
    [Header("Minigame Specifics")]
    [SerializeField] float alternateDelay;
    [SerializeField] float blinkBeforeAlternateTime;
    [SerializeField] Transform tongue;
    [SerializeField] float tongueMoveSpeed;
    [SerializeField] float tongueMinY;
    [SerializeField] float tongueMaxY;
    [SerializeField] float pause;
    [SerializeField] float maxTime;
    [SerializeField] int damagePerAug;

    SolidShlurpingPlatforms[] platforms;
    Rigidbody2D tongueBody;
    float currentAlternateTime = 3; //sue me
    float currentScoreTime = 0;
    bool firstSide = true;
    bool blinkStarted = false; 

    private void Start()
    {
        tongueBody = tongue.GetComponent<Rigidbody2D>();

        GenerateLayout();
        platforms = FindObjectsOfType<SolidShlurpingPlatforms>();
        platforms[1].gameObject.SetActive(false);
    }

    public override void StartMove()
    {
        base.StartMove();
        GetComponentInChildren<SolidShlurpingFollow>().Initialize();

        SolidShlurpingSalivaSpawn[] spawns = GetComponentsInChildren<SolidShlurpingSalivaSpawn>();
        foreach (SolidShlurpingSalivaSpawn spawn in spawns)
            spawn.enabled = true;
    }

    protected override void Update()
    {
        base.Update();
        TrackTongueTime();
    }

    private void TrackTongueTime()
    {
        if (!trackTime)
            return;

        currentAlternateTime += Time.deltaTime;

        if (currentAlternateTime >= blinkBeforeAlternateTime && !blinkStarted)
        {
            blinkStarted = true; 

            if (firstSide)
            {
                platforms[0].StartBlinking(); 
            }
            else
            {
                platforms[1].StartBlinking();
            }
        }

        if(currentAlternateTime >= alternateDelay)
        {
            StartCoroutine(Alternate());
        }
    }

    IEnumerator Alternate()
    {
        trackTime = false;
        currentAlternateTime = 0; 
        tongueBody.velocity = new Vector2(0, -tongueMoveSpeed);

        foreach (SolidShlurpingPlatforms platform in platforms)
        {
            if(platform.gameObject.activeInHierarchy)
            {
                platform.PhaseOut();
            }
            platform.gameObject.SetActive(true);
        }

        while (tongue.localPosition.y > tongueMinY)
        {
            yield return null;
        }

        tongue.localPosition = new Vector3(tongue.localPosition.x, tongueMinY, tongue.localPosition.z);
        tongueBody.velocity = Vector2.zero;

        yield return new WaitForSeconds(pause);

        tongueBody.velocity = new Vector2(0, tongueMoveSpeed);

        while(tongue.localPosition.y < tongueMaxY)
        {
            yield return null;
        }

        tongue.localPosition = new Vector3(tongue.localPosition.x, tongueMaxY, tongue.localPosition.z);
        tongueBody.velocity = Vector2.zero;

        if (firstSide)
        {
            platforms[0].gameObject.SetActive(false);
        }
        else
        {
            platforms[1].gameObject.SetActive(false);
        }

        blinkStarted = false;
        firstSide = !firstSide;
        trackTime = true;
    }

    public override void EndMove()
    {
        if (FindObjectOfType<InaPractice>())
            return;

        MoveEnded = true;

        int healing = CalculateScore();
        int damage = CombatManager.Instance.CharactersSelected[0].GetAUGS.Count * damagePerAug;

        DealDamageOrHealing(CombatManager.Instance.CharactersSelected[0], healing); //Heal target
        CombatManager.Instance.CharactersSelected[0].RemoveAmountOfAugments(999, 0); //Remove Debuffs

        //1 being base damage
        float DamageAdj = 1;

        //Damage on players must be divided by 100 to multiply the final
        DamageAdj = (float)CombatManager.Instance.GetCurrentCharacter.Stats.Damage / 100;

        CombatManager.Instance.CharactersSelected[1].TakeDamage((int)(damage * DamageAdj + CombatManager.Instance.GetCurrentCharacter.FlatDamageAdjustment), pierces);
    }
}

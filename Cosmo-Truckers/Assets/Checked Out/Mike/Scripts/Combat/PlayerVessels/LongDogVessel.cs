using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LongDogVessel : PlayerVessel
{
    [SerializeField] GameObject[] LoadedBullets;
    [SerializeField] GameObject[] ReserveBullets;
    [SerializeField] Sprite[] bulletTypeSprites;

    LongDogMana mana;
    LongDogCharacter character;

    public override void Initialize(PlayerCharacter player)
    {
        base.Initialize(player);
        mana = MyMana.GetComponent<LongDogMana>();
        character = MyCharacter.GetComponent<LongDogCharacter>();
        DisplayBullets();
    }

    public void DisplayBullets()
    {
        ClearBullets();

        for(int i = 0; i < mana.LoadedBullets.Count; i++)
        {
            LoadedBullets[i].SetActive(true);
            
            if(mana.LoadedBullets[i] == 0)
                LoadedBullets[i].GetComponent<Image>().sprite = bulletTypeSprites[0];
            else if(mana.LoadedBullets[i] == 1)
                LoadedBullets[i].GetComponent<Image>().sprite = bulletTypeSprites[1];
            else
                LoadedBullets[i].GetComponent<Image>().sprite = bulletTypeSprites[2];
        }

        for (int i = 0; i < mana.ReserveBullets.Count; i++)
        {
            ReserveBullets[i].SetActive(true);

            if (mana.ReserveBullets[i] == 0)
                ReserveBullets[i].GetComponent<Image>().sprite = bulletTypeSprites[0];
            else if (mana.ReserveBullets[i] == 1)
                ReserveBullets[i].GetComponent<Image>().sprite = bulletTypeSprites[1];
            else
                ReserveBullets[i].GetComponent<Image>().sprite = bulletTypeSprites[2];
        }
    }

    private void ClearBullets()
    {
        foreach (GameObject bullet in LoadedBullets)
            bullet.SetActive(false);
        foreach (GameObject bullet in ReserveBullets)
            bullet.SetActive(false);   
    }

    public IEnumerator LongDogDamageHealingEffect(List<int> damageHealingValues, EnumManager.CombatOutcome outcome, bool damage = true)
    {
        CombatManager.Instance.CommandsExecuting++;
        float finalStarAnimationWaitTime = 0f;
        int totalDamageHealing = 0; 

        List<string> text = new List<string>();
        foreach (int i in damageHealingValues)
        {
            totalDamageHealing += i; 
            text.Add(i.ToString()); 
        }

        int currentHealthDisplay = MyCharacter.CurrentHealth;
        int currentShieldDisplay = MyCharacter.Shield;

        if (currentHealthDisplay < 0)
            currentHealthDisplay = 0;

        // Use this as a temp var for bubble shield purposes
        int damageValuesToIterate = damageHealingValues.Count; 

        // If this is a multi-hit damage attack with a bubble shield: display 0 dmg, deal the dmg that removes the bubble, and then continue
        if (damage && MyCharacter.BubbleShielded && damageValuesToIterate > 1)
        {
            // Spawn a combat star with "0" as the number and -1 sorting layer so future stars don't overlap
            finalStarAnimationWaitTime = MyCharacter.SpawnCombatStar(outcome, "0", -1);

            // Allow the stars to wait a small period of time between spawning each one
            yield return new WaitForSeconds(CombatManager.Instance.CombatStarSpawnWaitTime);

            // Make sure the turn doesn't end prematurely
            CombatManager.Instance.CommandsExecuting++;

            // Call the TakeDamage method to destroy the bubble shield
            MyCharacter.SwitchCombatOutcomes(EnumManager.CombatOutcome.Damage, damageHealingValues[0], piercing: false);

            // Wait until damage is done 
            while (CombatManager.Instance.CommandsExecuting > 1)
                yield return null;

            // Make sure to subtract number of hits so we don't call an additional hit later
            damageValuesToIterate--;
        }

        // Spawn combat stars like normal
        for (int i = 0; i < damageValuesToIterate; i++)
        {
            finalStarAnimationWaitTime = MyCharacter.SpawnCombatStar(outcome, text[i], i);

            // Allow the stars to wait a small period of time between spawning each one
            yield return new WaitForSeconds(CombatManager.Instance.CombatStarSpawnWaitTime);

            // Deal damage / healing to the health bars themselves after combat stars have beens spawned
            if (damage)
            {
                // Subtract damage healing amount for every hit to character's "current" shield / health
                if (currentShieldDisplay > 0)
                {
                    currentShieldDisplay -= damageHealingValues[i];

                    // If current shield is now below 0, take that remaining amount from health and then set it to 0
                    if (currentShieldDisplay < 0)
                    {
                        currentHealthDisplay += currentShieldDisplay;
                        currentShieldDisplay = 0;
                    }
                }
                else { currentHealthDisplay -= damageHealingValues[i]; }

                // If "current" character shield is greater than 0 or the text isn't 0, update the shield text
                if (currentShieldDisplay > 0 || (currentShieldDisplay == 0 && currentShield.text != "0"))
                {
                    AdjustShieldDisplay(currentShieldDisplay);
                }
                else
                {
                    AdjustCurrentHealthDisplay(currentHealthDisplay);
                }
            }
            else
            {
                currentHealthDisplay += damageHealingValues[i];
                AdjustCurrentHealthDisplay(currentHealthDisplay);
            }
        }

        // Wait for the final star to finish animating before actually dealing damage
        yield return new WaitForSeconds(finalStarAnimationWaitTime);

        if (outcome == EnumManager.CombatOutcome.MultiDamage)
        {
            MyCharacter.GetComponent<LongDogCharacter>().LongDogTakeMultiHitDamage(damageHealingValues, false);
        }
        else if (outcome == EnumManager.CombatOutcome.MultiHealing)
        {
            MyCharacter.GetComponent<LongDogCharacter>().LongDogTakeMultiHitHealing(damageHealingValues, false);
        }
        else
        {
            MyCharacter.SwitchCombatOutcomes(outcome, totalDamageHealing, piercing: false);
        }
    }
}

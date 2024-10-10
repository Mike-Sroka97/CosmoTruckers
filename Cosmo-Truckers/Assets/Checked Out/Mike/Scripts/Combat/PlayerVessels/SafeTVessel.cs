using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SafeTVessel : PlayerVessel
{
    [SerializeField] Image[] angerNodesLeft;
    [SerializeField] Image[] angerNodesRight;
    [SerializeField] Image angerFace;
    [SerializeField] Sprite[] pipSprites;
    [SerializeField] Sprite[] angerFaceSprites;

    SafeTMana safeTMana;

    const int ragePip = 3;

    public override void ManaTracking()
    {
        //reset mana
        foreach (Image node in angerNodesLeft)
            node.sprite = pipSprites[0];
        foreach (Image node in angerNodesRight)
            node.sprite = pipSprites[0];

        //set current mana
        safeTMana = MyMana.GetComponent<SafeTMana>();
        int totalAnger = safeTMana.GetCurrentAnger();
        int totalRage = safeTMana.GetCurrentRage();

        for (int i = 0; i < totalAnger; i++)
        {
            angerNodesLeft[i].sprite = pipSprites[totalRage + 1];
            angerNodesRight[i].sprite = pipSprites[totalRage + 1];
            angerFace.sprite = angerFaceSprites[totalRage];
        }
    }

    public override IEnumerator DamageHealingEffect(bool damage, int damageHealingAmount, EnumManager.CombatOutcome outcome, int numberOfHits = 1)
    {
        CombatManager.Instance.CommandsExecuting++;

        float finalStarAnimationWaitTime = 0f;
        string text = damageHealingAmount.ToString();

        int currentHealthDisplay = MyCharacter.CurrentHealth;
        int currentShieldDisplay = MyCharacter.Shield;

        // If this is a multi-hit damage attack with a bubble shield: display 0 dmg, deal the dmg that removes the bubble, and then continue
        if (damage && MyCharacter.BubbleShielded && numberOfHits > 1)
        {
            // Spawn a combat star with "0" as the number and -1 sorting layer so future stars don't overlap
            finalStarAnimationWaitTime = CallSpawnCombatStar(outcome, "0", -1);

            // Allow the stars to wait a small period of time between spawning each one
            yield return new WaitForSeconds(CombatManager.Instance.CombatStarSpawnWaitTime);

            // Make sure the turn doesn't end prematurely
            CombatManager.Instance.CommandsExecuting++;

            // Call the TakeDamage method to destroy the bubble shield
            MyCharacter.SwitchCombatOutcomes(EnumManager.CombatOutcome.Damage, damageHealingAmount, piercing: false);

            // Wait until damage is done 
            while (CombatManager.Instance.CommandsExecuting > 1)
                yield return null;

            // Make sure to subtract number of hits so we don't call an additional hit later
            numberOfHits--;
        }

        // Spawn combat stars like normal
        for (int i = 0; i < numberOfHits; i++)
        {
            finalStarAnimationWaitTime = CallSpawnCombatStar(outcome, text, i);

            // Allow the stars to wait a small period of time between spawning each one
            yield return new WaitForSeconds(CombatManager.Instance.CombatStarSpawnWaitTime);

            if (!safeTMana)
                safeTMana = MyMana.GetComponent<SafeTMana>();

            // Deal damage / healing to the health bars themselves after combat stars have beens spawned
            if (damage)
            {
                safeTMana.SetCurrentAnger(1); // Give Safe-T mana if he is damaged

                // Subtract damage healing amount for every hit to character's "current" shield / health
                if (currentShieldDisplay > 0)
                {
                    currentShieldDisplay -= damageHealingAmount;

                    // If current shield is now below 0, take that remaining amount from health and then set it to 0
                    if (currentShieldDisplay < 0)
                    {
                        currentHealthDisplay += currentShieldDisplay;
                        currentShieldDisplay = 0;
                    }
                }
                else { currentHealthDisplay -= damageHealingAmount; }

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
                currentHealthDisplay += damageHealingAmount;
                AdjustCurrentHealthDisplay(currentHealthDisplay);
            }
        }

        // Wait for the final star to finish animating before actually dealing damage
        yield return new WaitForSeconds(finalStarAnimationWaitTime);

        MyCharacter.SwitchCombatOutcomes(outcome, damageHealingAmount, piercing: false, numberOfHits);
    }
}

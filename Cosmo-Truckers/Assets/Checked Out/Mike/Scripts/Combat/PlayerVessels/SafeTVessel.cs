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

    protected override IEnumerator DamageHealingEffect(bool damage, int damageHealingAmount, int numberOfHits = 1)
    {
        int currentCharacterHealth = myCharacter.CurrentHealth;

        for (int i = 0; i < numberOfHits; i++)
        {
            if(!safeTMana)
                safeTMana = MyMana.GetComponent<SafeTMana>();

            safeTMana.SetCurrentAnger(1);

            if(currentCharacterHealth > myCharacter.Health - (damageHealingAmount * numberOfHits - (damageHealingAmount * (i + 1))))
            {
                int newShield = int.Parse(currentShield.text) - damageHealingAmount;
                currentShield.text = newShield.ToString();
            }
            else
            {
                currentHealth.text = (currentCharacterHealth + (damageHealingAmount * numberOfHits - (damageHealingAmount * (i + 1)))).ToString();
                TrackShield();
            }

            if (damage)
                damageHealingText.color = damageColor;
            else
                damageHealingText.color = healingColor;

            while (damageHealingText.color.a > 0)
            {
                damageHealingText.transform.position += new Vector3(moveSpeed * Time.deltaTime, moveSpeed * Time.deltaTime, 0);
                damageHealingText.color -= new Color(0, 0, 0, fadeSpeed * Time.deltaTime);
                yield return null;
            }

            damageHealingText.transform.localPosition = Vector3.zero;

            if (int.Parse(shieldText.text) <= 0)
            {
                TrackShield();
            }
        }
    }
}

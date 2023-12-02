using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LongDogVessel : PlayerVessel
{
    [SerializeField] GameObject[] LoadedBullets;
    [SerializeField] GameObject[] ReserveBullets;

    LongDogMana mana;
    LongDogCharacter character;

    public override void Initialize(PlayerCharacter player)
    {
        base.Initialize(player);
        mana = MyMana.GetComponent<LongDogMana>();
        character = myCharacter.GetComponent<LongDogCharacter>();
        DisplayBullets();
    }

    public void DisplayBullets()
    {
        ClearBullets();

        for(int i = 0; i < mana.LoadedBullets.Count; i++)
        {
            LoadedBullets[i].SetActive(true);
            //handle sprite based on type of bullet
        }

        for (int i = 0; i < mana.ReserveBullets.Count; i++)
        {
            ReserveBullets[i].SetActive(true);
            //handle sprite based on type of bullet
        }
    }

    private void ClearBullets()
    {
        foreach (GameObject bullet in LoadedBullets)
            bullet.SetActive(false);
        foreach (GameObject bullet in ReserveBullets)
            bullet.SetActive(false);   
    }

    public IEnumerator LongDogDamageHealingEffect(List<int> damageValues, bool damage = true)
    {
        int currentCharacterHealth = myCharacter.CurrentHealth;
        shieldText.text = currentShield.ToString();

        if (currentCharacterHealth < 0)
            currentCharacterHealth = 0;

        int currentTotalDamageDisplayed = 0;
        int totalDamage = 0;
        foreach (int damageInstance in damageValues)
            totalDamage += damageInstance;

        currentHealth.text = currentCharacterHealth.ToString();

        float maxHealth = myCharacter.Health;
        float healthRatio = (float)currentCharacterHealth / maxHealth;
        currentHealthBar.fillAmount = healthRatio;

        for (int i = 0; i < damageValues.Count; i++)
        {
            currentTotalDamageDisplayed += damageValues[i];

            if (damage)
            {
                if (currentCharacterHealth > myCharacter.Health - (totalDamage - currentTotalDamageDisplayed))
                {
                    int newShield = int.Parse(currentShield.text) - damageValues[i];
                    currentShield.text = newShield.ToString();
                }
                else
                {
                    currentHealth.text = (currentCharacterHealth + (totalDamage - currentTotalDamageDisplayed)).ToString();
                    TrackShield();
                }
            }
            else
            {
                currentHealth.text = (currentCharacterHealth + (totalDamage - currentTotalDamageDisplayed)).ToString();
            }

            if (damage)
                damageHealingText.color = damageColor;
            else
                damageHealingText.color = healingColor;

            damageHealingText.text = damageValues[i].ToString();

            while (damageHealingText.color.a > 0)
            {
                damageHealingText.transform.position += new Vector3(moveSpeed * Time.deltaTime, moveSpeed * Time.deltaTime, 0);
                damageHealingText.color -= new Color(0, 0, 0, fadeSpeed * Time.deltaTime);
                yield return null;
            }

            damageHealingText.transform.localPosition = Vector3.zero;

            if (shieldGO.activeInHierarchy && int.Parse(shieldText.text) <= 0)
            {
                TrackShield();
            }
        }
    }
}

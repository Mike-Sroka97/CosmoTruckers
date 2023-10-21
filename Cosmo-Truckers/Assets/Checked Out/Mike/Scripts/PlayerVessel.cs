using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerVessel : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI currentHealth;
    [SerializeField] TextMeshProUGUI maxHealth;
    [SerializeField] GameObject shieldGO;
    [SerializeField] Image characterImage;
    [SerializeField] TextMeshProUGUI currentShield;
    [SerializeField] Image currentHealthBar;

    [Space(20)]
    [Header("Special Effects")]
    [SerializeField] protected TextMeshProUGUI damageHealingText;
    [SerializeField] protected Color damageColor;
    [SerializeField] protected Color healingColor;
    [SerializeField] protected float fadeSpeed;
    [SerializeField] protected float moveSpeed;

    protected PlayerCharacter myCharacter;
    [HideInInspector] public Mana MyMana;

    public void Initialize(PlayerCharacter player)
    {
        //set player
        myCharacter = player;
        myCharacter.MyVessel = this;

        //set image
        characterImage.sprite = myCharacter.VesselImage;

        //set shield
        currentShield.text = "0";
        shieldGO.SetActive(false);

        //set health
        maxHealth.text = myCharacter.Health.ToString();
        currentHealth.text = myCharacter.CurrentHealth.ToString();

        //assign vessel to mana
        MyMana = myCharacter.GetComponent<Mana>();
        MyMana.SetVessel(this);
    }

    public void AdjustCurrentHealthDisplay(int newHealth, int damageHealingAmount, bool damage = true)
    {
        damageHealingText.text = damageHealingAmount.ToString();
        currentHealth.text = newHealth.ToString();

        float currentHealthValue = myCharacter.CurrentHealth;
        float maxHealth = myCharacter.Health;
        float healthRatio = currentHealthValue / maxHealth;
        currentHealthBar.fillAmount = healthRatio;

        StartCoroutine(DamageHealingEffect(damage, damageHealingAmount));
    }

    public void AdjustMultiHitHealthDisplay(int newHealth, int damageHealingAmount, int numberOfHits, bool damage = true)
    {
        damageHealingText.text = damageHealingAmount.ToString();
        currentHealth.text = newHealth.ToString();

        float currentHealthValue = myCharacter.CurrentHealth;
        float maxHealth = myCharacter.Health;
        float healthRatio = currentHealthValue / maxHealth;
        currentHealthBar.fillAmount = healthRatio;

        StartCoroutine(DamageHealingEffect(damage, damageHealingAmount, numberOfHits));
    }

    protected virtual IEnumerator DamageHealingEffect(bool damage, int damageHealingAmount, int numberOfHits = 1)
    {
        int currentCharacterHealth = myCharacter.CurrentHealth;

        for (int i = 0; i < numberOfHits; i++)
        {
            currentHealth.text = (currentCharacterHealth + (damageHealingAmount * numberOfHits - (damageHealingAmount * (i + 1)))).ToString();

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
        }
    }

    public virtual void ManaTracking() { }
}

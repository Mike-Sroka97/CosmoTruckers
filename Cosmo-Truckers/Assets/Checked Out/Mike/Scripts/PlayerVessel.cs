using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerVessel : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI currentHealth;
    [SerializeField] TextMeshProUGUI maxHealth;
    [SerializeField] protected GameObject shieldGO;
    [SerializeField] Image characterImage;
    [SerializeField] protected TextMeshProUGUI currentShield;
    [SerializeField] protected Image currentHealthBar;
    [SerializeField] Image currentShieldBar;

    [Space(20)]
    [Header("Special Effects")]
    [SerializeField] protected TextMeshProUGUI damageHealingText;
    [SerializeField] protected TextMeshProUGUI shieldText;
    [SerializeField] protected Color damageColor;
    [SerializeField] protected Color healingColor;
    [SerializeField] protected Color shieldColor;

    protected const float fadeSpeed = 2f;
    protected const float moveSpeed = 0.5f;

    protected PlayerCharacter myCharacter;
    [HideInInspector] public Mana MyMana;

    public virtual void Initialize(PlayerCharacter player)
    {
        //set player
        myCharacter = player;
        myCharacter.MyVessel = this;

        //set image
        characterImage.sprite = myCharacter.VesselImage;

        //set shield
        shieldText.color = shieldColor;
        TrackShield();

        //set health
        maxHealth.text = myCharacter.Health.ToString();
        currentHealth.text = myCharacter.CurrentHealth.ToString();

        //assign vessel to mana
        MyMana = myCharacter.GetComponent<Mana>();
        MyMana.SetVessel(this);
    }

    public void AdjustFill()
    {
        maxHealth.text = myCharacter.Health.ToString();
        currentHealth.text = myCharacter.CurrentHealth.ToString();

        float newMaxHealth = myCharacter.Health;
        float healthRatio = myCharacter.CurrentHealth / newMaxHealth;

        currentHealthBar.fillAmount = healthRatio;
    }

    public void AdjustCurrentHealthDisplay(int newHealth, int damageHealingAmount, bool damage = true)
    {
        if (newHealth < 0)
            newHealth = 0;

        damageHealingText.text = damageHealingAmount.ToString();
        currentHealth.text = newHealth.ToString();

        float maxHealth = myCharacter.Health;
        float healthRatio = newHealth / maxHealth;
        currentHealthBar.fillAmount = healthRatio;

        StartCoroutine(DamageHealingEffect(damage, damageHealingAmount));
    }

    public void AdjustMultiHitHealthDisplay(int newHealth, int damageHealingAmount, int numberOfHits, bool damage = true)
    {
        if (newHealth < 0)
            newHealth = 0;

        damageHealingText.text = damageHealingAmount.ToString();
        currentHealth.text = newHealth.ToString();

        float maxHealth = myCharacter.Health;
        float healthRatio = newHealth / maxHealth;
        currentHealthBar.fillAmount = healthRatio;

        StartCoroutine(DamageHealingEffect(damage, damageHealingAmount, numberOfHits));
    }

    protected virtual IEnumerator DamageHealingEffect(bool damage, int damageHealingAmount, int numberOfHits = 1)
    {
        int currentCharacterHealth = myCharacter.CurrentHealth;
        shieldText.text = currentShield.ToString();

        for (int i = 0; i < numberOfHits; i++)
        {
            if(damage)
            {
                if (currentCharacterHealth > myCharacter.Health - (damageHealingAmount * numberOfHits - (damageHealingAmount * (i + 1))))
                {
                    int newShield = int.Parse(currentShield.text) - damageHealingAmount;
                    currentShield.text = newShield.ToString();
                }
                else
                {
                    currentHealth.text = (currentCharacterHealth + (damageHealingAmount * numberOfHits - (damageHealingAmount * (i + 1)))).ToString();
                    TrackShield();
                }
            }
            else
            {
                currentHealth.text = (currentCharacterHealth + (damageHealingAmount * numberOfHits - (damageHealingAmount * (i + 1)))).ToString();
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

            if (shieldGO.activeInHierarchy && int.Parse(shieldText.text) <= 0)
            {
                TrackShield();
            }
        }
    }

    protected void TrackShield()
    {
        currentShield.text = myCharacter.Shield.ToString();

        float currentShieldValue = myCharacter.Shield;
        float shieldRatio = currentShieldValue / 60; //60 is max shields
        currentShieldBar.fillAmount = shieldRatio;

        if (myCharacter.Shield <= 0)
            shieldGO.SetActive(false);
    }

    public void AdjustShieldDisplay(int newShield, int shieldAmount)
    {
        shieldGO.SetActive(true);
        currentShield.text = myCharacter.Shield.ToString();

        float currentShieldValue = myCharacter.Shield;
        float shieldRatio = currentShieldValue / 60; //60 is max shields
        currentShieldBar.fillAmount = shieldRatio;

        StartCoroutine(ShieldEffect(shieldAmount));
    }

    protected virtual IEnumerator ShieldEffect(int shieldAmount)
    {
        int currentShield = myCharacter.Shield;

        shieldText.text = currentShield.ToString();

        while (shieldText.color.a > 0)
        {
            shieldText.transform.position += new Vector3(moveSpeed * Time.deltaTime, moveSpeed * Time.deltaTime, 0);
            shieldText.color -= new Color(0, 0, 0, fadeSpeed * Time.deltaTime);
            yield return null;
        }

        shieldText.transform.localPosition = Vector3.zero;
    }

    public void ManuallySetShield(int shield)
    {
        currentShield.text = myCharacter.Shield.ToString();
        currentShieldBar.fillAmount = shield / 60;
        TrackShield();
    }

    public virtual void ManaTracking() { }
}

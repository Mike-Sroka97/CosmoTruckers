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
    [SerializeField] Sprite aliveSprite;
    [SerializeField] Sprite deadSprite;
    [SerializeField] Material[] playerOutlineMaterials;

    [Space(20)]
    [Header("Special Effects")]
    [SerializeField] protected TextMeshProUGUI damageHealingText;
    [SerializeField] protected TextMeshProUGUI shieldText;
    [SerializeField] protected Color damageColor;
    [SerializeField] protected Color healingColor;
    [SerializeField] protected Color shieldColor;

    protected const float fadeSpeed = 2f;
    protected const float moveSpeed = 0.5f;

    public PlayerCharacter MyCharacter;
    [HideInInspector] public Mana MyMana;
    Transform myINAvessel;

    public virtual void Initialize(PlayerCharacter player)
    {
        //set player
        MyCharacter = player;
        MyCharacter.MyVessel = this;

        //set image
        characterImage.sprite = MyCharacter.VesselImage;

        //set vessel outline
        characterImage.material = playerOutlineMaterials[MyCharacter.PlayerNumber - 1];

        //set shield
        TrackShield();

        //set health
        maxHealth.text = MyCharacter.Health.ToString();
        currentHealth.text = MyCharacter.CurrentHealth.ToString();

        //assign vessel to mana
        MyMana = MyCharacter.GetComponent<Mana>();
        MyMana.SetVessel(this);

        //assign INA vessel
        myINAvessel = FindObjectOfType<INAcombat>().transform.Find("Health Vessels").GetChild(MyCharacter.PlayerNumber - 1).transform;
        UpdateHealthText();

        MyCharacter.HealthChangeEvent.AddListener(UpdateHealthText);
    }

    public void UpdateHealthText()
    {
        myINAvessel.GetComponentInChildren<TextMeshPro>().text = MyCharacter.CurrentHealth.ToString();

        if(MyCharacter.CurrentHealth > 0)
            myINAvessel.Find("PlayerIcon").GetComponent<SpriteRenderer>().sprite = aliveSprite;
        else
            myINAvessel.Find("PlayerIcon").GetComponent<SpriteRenderer>().sprite = deadSprite;
    }

    public void SetINAvesselSprite(bool alive)
    {
        if (alive)
            myINAvessel.Find("PlayerIcon").GetComponent<SpriteRenderer>().sprite = aliveSprite;
        else
            myINAvessel.Find("PlayerIcon").GetComponent<SpriteRenderer>().sprite = deadSprite;
    }

    public void AdjustFill()
    {
        maxHealth.text = MyCharacter.Health.ToString();
        currentHealth.text = MyCharacter.CurrentHealth.ToString();

        float newMaxHealth = MyCharacter.Health;
        float healthRatio = MyCharacter.CurrentHealth / newMaxHealth;

        currentHealthBar.fillAmount = healthRatio;
    }

    protected void AdjustPlayerIcon(int health)
    {
        if (health == 0)
            characterImage.sprite = deadSprite;
        else
            characterImage.sprite = aliveSprite;
    }

    public void AdjustCurrentHealthDisplay(int newHealth, int damageHealingAmount, bool damage = true)
    {
        if (newHealth < 0)
            newHealth = 0;

        AdjustPlayerIcon(newHealth);

        damageHealingText.text = damageHealingAmount.ToString();
        currentHealth.text = newHealth.ToString();

        float maxHealth = MyCharacter.Health;
        float healthRatio = newHealth / maxHealth;
        currentHealthBar.fillAmount = healthRatio;

        StartCoroutine(DamageHealingEffect(damage, damageHealingAmount));
    }

    public void AdjustMultiHitHealthDisplay(int newHealth, int damageHealingAmount, int numberOfHits, bool damage = true)
    {
        if (newHealth < 0)
            newHealth = 0;

        AdjustPlayerIcon(newHealth);

        damageHealingText.text = damageHealingAmount.ToString();
        currentHealth.text = newHealth.ToString();

        float maxHealth = MyCharacter.Health;
        float healthRatio = newHealth / maxHealth;
        currentHealthBar.fillAmount = healthRatio;

        StartCoroutine(DamageHealingEffect(damage, damageHealingAmount, numberOfHits));
    }

    protected virtual IEnumerator DamageHealingEffect(bool damage, int damageHealingAmount, int numberOfHits = 1)
    {
        int currentCharacterHealth = MyCharacter.CurrentHealth;
        shieldText.text = currentShield.ToString();

        for (int i = 0; i < numberOfHits; i++)
        {
            if(damage)
            {
                if (currentCharacterHealth > MyCharacter.Health - (damageHealingAmount * numberOfHits - (damageHealingAmount * (i + 1))))
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

            if (int.Parse(currentShield.text) <= 0)
            {
                TrackShield();
            }
        }
    }

    protected void TrackShield()
    {
        currentShield.text = MyCharacter.Shield.ToString();

        float currentShieldValue = MyCharacter.Shield;
        float shieldRatio = currentShieldValue / 60; //60 is max shields
        currentShieldBar.fillAmount = shieldRatio;
    }

    public void AdjustShieldDisplay(int newShield, int shieldAmount)
    {
        currentShield.text = MyCharacter.Shield.ToString();

        float currentShieldValue = MyCharacter.Shield;
        float shieldRatio = currentShieldValue / 60; //60 is max shields
        currentShieldBar.fillAmount = shieldRatio;

        StartCoroutine(ShieldEffect(shieldAmount));
    }

    protected virtual IEnumerator ShieldEffect(int shieldAmount)
    {
        shieldText.text = shieldAmount.ToString();
        shieldText.color = shieldColor;

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
        MyCharacter.BubbleShielded = false;
        currentShield.text = MyCharacter.Shield.ToString();
        currentShieldBar.fillAmount = shield / 60;
        TrackShield();
    }

    public virtual void ManaTracking() { }

    private void OnDestroy()
    {
        MyCharacter.HealthChangeEvent.RemoveListener(UpdateHealthText);
    }
}

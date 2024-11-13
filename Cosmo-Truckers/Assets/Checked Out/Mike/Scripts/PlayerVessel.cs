using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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

    protected const float fadeSpeed = 2f;
    protected const float moveSpeed = 0.5f;

    public PlayerCharacter MyCharacter;
    [HideInInspector] public Mana MyMana;
    Transform myINAvessel;
    public Transform CombatStarSpawn { get; private set; }

    /// <summary>
    /// For local assistance when multiple damage/healing visual effects are happening simultaneously
    /// Wait for the first call to be finished, then progress to the next, etc
    /// Exists for multi-target functionality. Prevents each character in combat for waiting for another to be done when iterating through their own visuals
    /// </summary>
    protected int LocalCommandsExecuting = 0;

    //don't laugh
    public Image GetCharacterImage() { return characterImage; }

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

    public void AdjustCurrentHealthDisplay(int newHealth)
    {
        float maxHealth = MyCharacter.Health;

        if (newHealth < 0)
            newHealth = 0;
        else if (newHealth > maxHealth)
            newHealth = (int)maxHealth; 

        AdjustPlayerIcon(newHealth);

        //damageHealingText.text = damageHealingAmount.ToString();
        currentHealth.text = newHealth.ToString();

        float healthRatio = newHealth / maxHealth;
        currentHealthBar.fillAmount = healthRatio;
    }

    public virtual IEnumerator DamageHealingEffect(bool damage, int damageHealingAmount, EnumManager.CombatOutcome outcome, int numberOfHits = 1)
    {
        CombatManager.Instance.CommandsExecuting++;

        // Fixes multilple damage/healing effect calls spawning it at same time. Can't use CommandsExecuting because it would mess up Multi-Target attacks
        while (LocalCommandsExecuting > 0)
            yield return null;

        LocalCommandsExecuting++;

        float finalStarAnimationWaitTime = 0f;
        string text = damageHealingAmount.ToString();

        int currentHealthDisplay = MyCharacter.CurrentHealth;
        int currentShieldDisplay = MyCharacter.Shield; 

        // If this is a multi-hit damage attack with a bubble shield: display 0 dmg, deal the dmg that removes the bubble, and then continue
        if (damage && MyCharacter.BubbleShielded && numberOfHits > 1)
        {
            // Spawn a combat star with "0" as the number and -1 sorting layer so future stars don't overlap
            finalStarAnimationWaitTime = CallSpawnCombatStar(outcome, "0", MyCharacter.CombatStarsCurrentLayer);
            MyCharacter.CombatStarsCurrentLayer++;

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

            // Deal damage / healing to the health bars themselves after combat stars have beens spawned
            if (damage)
            {
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

                    AdjustShieldDisplay(currentShieldDisplay);
                }
                else { currentHealthDisplay -= damageHealingAmount; }

                // If "current" character shield is greater than 0 or the text isn't 0, update the shield text
                if (currentShieldDisplay == 0)
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

        LocalCommandsExecuting--; 

        MyCharacter.SwitchCombatOutcomes(outcome, damageHealingAmount, piercing: false, numberOfHits);
    }

    protected void TrackShield()
    {
        currentShield.text = MyCharacter.Shield.ToString();

        float currentShieldValue = MyCharacter.Shield;
        float shieldRatio = currentShieldValue / 60; //60 is max shields
        currentShieldBar.fillAmount = shieldRatio;
    }

    public void AdjustShieldDisplay(int shieldAmount)
    {
        currentShield.text = shieldAmount.ToString();

        float shieldRatio = (float)shieldAmount / 60; //60 is max shields
        currentShieldBar.fillAmount = shieldRatio;
    }

    public virtual IEnumerator ShieldEffect(int shieldAmount)
    {
        CombatManager.Instance.CommandsExecuting++;

        // Fixes multilple damage/healing effect calls spawning it at same time. Can't use CommandsExecuting because it would mess up Multi-Target attacks
        while (LocalCommandsExecuting > 0)
            yield return null;

        LocalCommandsExecuting++;

        string text = shieldAmount.ToString();
        
        float finalStarAnimationWaitTime = CallSpawnCombatStar(EnumManager.CombatOutcome.Shield, text, MyCharacter.CombatStarsCurrentLayer);
        MyCharacter.CombatStarsCurrentLayer++;

        AdjustShieldDisplay(shieldAmount);

        LocalCommandsExecuting--;

        // Wait for the final star to finish animating before actually dealing 
        yield return new WaitForSeconds(finalStarAnimationWaitTime);

        MyCharacter.SwitchCombatOutcomes(EnumManager.CombatOutcome.Shield, shieldAmount, piercing: false);
    }

    /// <summary>
    /// Spawns the combat star and returns the time for its animation to be over
    /// </summary>
    /// <returns></returns>
    /// <summary>
    /// Spawns the combat star and returns the time for its animation to be over
    /// </summary>
    /// <returns></returns>
    public float CallSpawnCombatStar(EnumManager.CombatOutcome outcome, string text, int spawnLayer)
    {
        // Determine if InCombat or outside so CombatStar is spawned properly
        Transform combatStarSpawn;
        if (CombatManager.Instance.InCombat)
        {
            if (MyCharacter.CombatStarSpawn != null)
                combatStarSpawn = MyCharacter.CombatStarSpawn;
            else
            {
                Debug.LogError($"{MyCharacter.name} has a null Combat Star Spawn! Using their position instead!");
                combatStarSpawn = MyCharacter.transform;
            }
        }
        else
        {
            if (CombatStarSpawn != null)
                combatStarSpawn = CombatStarSpawn;
            else
            {
                Debug.LogError($"{gameObject.name} has a null Combat Star Spawn! Using its position instead!");
                combatStarSpawn = transform;
            }
        }

        return CombatManager.Instance.SpawnCombatStar(outcome, text, spawnLayer, combatStarSpawn);
    }

    public void ManuallySetShield(int shield)
    {
        currentShield.text = shield.ToString();
        currentShieldBar.fillAmount = (float)shield / 60;
    }

    public virtual void ManaTracking() { }

    private void OnDestroy()
    {
        MyCharacter.HealthChangeEvent.RemoveListener(UpdateHealthText);
    }
}

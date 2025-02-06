using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.TextCore.Text;

public abstract class Character : MonoBehaviour
{
    [SerializeField] protected EnemyPassiveBase passiveMove;
    [SerializeField] protected AugmentStackSO[] passiveAugments;
    [SerializeField] protected List<AugmentStackSO> AUGS = new List<AugmentStackSO>();
    public List<AugmentStackSO> AugmentsToRemove = new List<AugmentStackSO>();
    [SerializeField] protected int maxShield = 60;
    private int shield = 0;
    public List<AugmentStackSO> GetAUGS { get => AUGS; }
    public CharacterStats Stats;
    public int Health;
    public SpriteRenderer[] TargetingSprites;
    public SpriteRenderer[] ShieldSprites;
    public Sprite MiniAugSprite;
    public Sprite BigAugSprite;
    [SerializeField] Material shieldedMaterial;
    [SerializeField] Material bubbleShieldMaterial;
    const float spriteRotationSpeed = -500f;
    public int CombatSpot;
    public int FlatDamageAdjustment = 0;
    public int FlatHealingAdjustment = 0;
    public UnityEvent HealthChangeEvent = new UnityEvent();
    public UnityEvent ShieldChangeEvent = new UnityEvent();
    public UnityEvent BubbleShieldBrokenEvent = new UnityEvent();
    public UnityEvent DieEvent = new UnityEvent();
    public UnityEvent AugmentCountChangeEvent = new UnityEvent();
    public bool Stunned = false;
    private SpriteRenderer stunnedRenderer;
    public bool Tireless = false;
    private SpriteRenderer tirelessRenderer;

    protected const float fadeSpeed = 2;
    protected const float moveSpeed = 0.5f;
    protected const int maxAugs = 6;

    /// <summary>
    /// Each character should track their own combat stars layer
    /// </summary>
    [HideInInspector] public int CombatStarsCurrentLayer = 0; 

    /// <summary>
    /// For local assistance when multiple damage/healing visual effects are happening simultaneously
    /// Wait for the first call to be finished, then progress to the next, etc
    /// Exists for multi-target functionality. Prevents each character in combat for waiting for another to be done when iterating through their own visuals
    /// </summary>
    protected int LocalCommandsExecuting = 0; 

    //Augment stuffs
    TextMeshProUGUI augmentText;
    bool augmenting = false;

    public Transform CombatStarSpawn { get; private set; }

    public int CurrentHealth
    {
        get
        {
            return currentHealth;
        }
        set
        {
            HealthChangeEvent.Invoke();

            if (currentHealth + value > Health)
                currentHealth = Health;
            else if (currentHealth + value < 0)
                currentHealth = 0;
            else
                currentHealth += value;
        }
    }

    private int currentHealth;
    public int Shield
    {
        get
        {
            return shield;
        }
        set
        {
            ShieldChangeEvent.Invoke();

            if (shield + value > maxShield)
            {
                shield = maxShield;
                AdjustShieldMaterial(true);
            }
            else if (shield + value <= 0)
            {
                shield = 0;
                AdjustShieldMaterial(false);
            }
            else
            {
                shield += value;
                AdjustShieldMaterial(true);
            }
        }
    }

    public bool BubbleShielded = false;

    public bool Dead;

    protected TurnOrder turnOrder;
    protected SpriteRenderer myRenderer;

    [SerializeField] int spaceTaken = 1;
    public int GetSpaceTaken { get => spaceTaken; }

    protected virtual void OnDisable()
    {
        HealthChangeEvent.RemoveAllListeners();
        ShieldChangeEvent.RemoveAllListeners();
        BubbleShieldBrokenEvent.RemoveAllListeners();
        DieEvent.RemoveAllListeners();
    }

    public virtual void StartCombatEffect() { }
    public virtual void EndCombatEffect() { }

    public virtual void TakeDamage(int damage, bool defensePiercing = false)
    {
        if (Dead)
            return;

        if (!defensePiercing)
            damage = AdjustAttackDamage(damage);

        if (passiveMove && passiveMove.GetPassiveType == EnemyPassiveBase.PassiveType.OnDamage)
            passiveMove.Activate(CurrentHealth);

        if(BubbleShielded)
        {
            AdjustBubbleShield();
        }
        else if (Shield > 0)
        {
            //calculate overrage damage
            int overageDamage = damage - Shield;

            Shield = -damage;

            if (overageDamage > 0)
                CurrentHealth = -overageDamage;
        }
        else CurrentHealth = -damage;

        if (CurrentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
        //See if any AUGS trigger on Damage (Spike shield)
        else
        {
            AugmentsToRemove.Clear();

            foreach (AugmentStackSO aug in AUGS)
                if (aug.OnDamage && damage > 0)
                    aug.GetAugment().Trigger();

            foreach (AugmentStackSO augment in AugmentsToRemove)
                CleanUpAUGs();
        }

        // After damage is done, subtract Command Executing
        CombatManager.Instance.CommandsExecuting--;

        // For Boss Move purposes
        if (CombatManager.Instance.CommandsExecuting < 0)
            CombatManager.Instance.CommandsExecuting = 0; 
    }

    public virtual void TakeMultiHitDamage(int damage, int numberOfHits, bool defensePiercing = false)
    {
        if (Dead)
            return;

        for (int i = 0; i < numberOfHits; i++)
        {
            if (!defensePiercing)
                damage = AdjustAttackDamage(damage);

            if (passiveMove && passiveMove.GetPassiveType == EnemyPassiveBase.PassiveType.OnDamage)
                passiveMove.Activate(CurrentHealth);

            if(BubbleShielded)
            {
                AdjustBubbleShield();
            }
            else if (Shield > 0)
            {
                //calculate overrage damage
                int overageDamage = damage - Shield;

                Shield = -damage; 

                if (overageDamage > 0)
                    CurrentHealth = -overageDamage;
            }
            else CurrentHealth = -damage;

            if (CurrentHealth <= 0)
            {
                Die();
            }
            //See if any AUGS trigger on Damage (Spike shield)
            else
            {
                AugmentsToRemove.Clear();

                foreach (AugmentStackSO aug in AUGS)
                {
                    if (aug.OnDamage && damage > 0)
                    {
                        aug.GetAugment().Trigger();
                    }
                }

                foreach (AugmentStackSO augment in AugmentsToRemove)
                    CleanUpAUGs(); 
            }
        }

        // After damage is done, subtract Command Executing
        CombatManager.Instance.CommandsExecuting--;

        // For Boss Move purposes
        if (CombatManager.Instance.CommandsExecuting < 0)
            CombatManager.Instance.CommandsExecuting = 0;
    }

    protected void AdjustAugs(bool add, AugmentStackSO stack)
    {
        if (add)
            AUGS.Add(stack);
        else
            AUGS.Remove(stack);

        AugmentCountChangeEvent.Invoke();
    }

    public virtual void TakeHealing(int healing, bool ignoreVigor = false)
    {
        if (Dead)
            return;

        if(!ignoreVigor)
            healing = AdjustAttackHealing(healing);

        CurrentHealth = healing;

        // After healing is done, subtract Command Executing
        CombatManager.Instance.CommandsExecuting--;
    }

    public virtual void TakeMultiHitHealing(int healing, int numberOfHeals, bool ignoreVigor = false)
    {
        if (Dead)
        {
            // If character is dead, subtract Command Executing
            CombatManager.Instance.CommandsExecuting--;
            return;
        }

        for (int i = 0; i < numberOfHeals; i++)
        {
            if (!ignoreVigor)
                healing = AdjustAttackHealing(healing);

            CurrentHealth = healing;
        }

        // After healing, subtract Command Executing
        CombatManager.Instance.CommandsExecuting--;
    }

    public virtual void TakeShielding(int shieldAmount)
    {
        if (Dead)
        {
            // If character is dead, subtract Command Executing
            CombatManager.Instance.CommandsExecuting--;
            return;
        }

        if (Shield + shieldAmount > maxShield)
            Shield = maxShield;
        else
            Shield = shieldAmount;

        // After taking shield damage, subtract Command Executing
        CombatManager.Instance.CommandsExecuting--;
    }

    // Coroutine needs to be called on object that isn't inactive
    public void SingleHealThenDamage(int currentHealing, int currentDamage, bool piercing = true)
    {
        StartCoroutine(SingleHealingThenDamage(currentHealing, currentDamage, piercing));
    }

    // Heal, wait for healing combat star, and then deal damage
    private IEnumerator SingleHealingThenDamage(int currentHealing, int currentDamage, bool piercing = true)
    {
        // Heal first and spawn healing star
        TakeHealing(currentHealing, piercing);

        // Wait until healing has started spawning in visual effects
        while (CombatManager.Instance.CommandsExecuting < 1)
            yield return null;

        // Wait until the star has been actually spawned in
        yield return new WaitForSeconds(0.25f); 

        // Take damage and spawn damage star after healing is done
        TakeDamage(currentDamage, piercing); //pierce defense cause technically healing
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
        Transform combatStarSpawn;

        if (CombatStarSpawn != null)
            combatStarSpawn = CombatStarSpawn;
        else
        {
            Debug.LogError($"{gameObject.name} has a null Combat Star Spawn! Using its position instead!");
            combatStarSpawn = transform;
        }

        return CombatManager.Instance.SpawnCombatStar(outcome, text, spawnLayer, combatStarSpawn);
    }

    public void AdjustBubbleShield(bool active = false)
    {
        if(active == false)
        {
            BubbleShieldBrokenEvent.Invoke();
            BubbleShieldBrokenEvent.RemoveAllListeners();
        }

        BubbleShielded = active;
        AdjustShieldMaterial(Shield > 0);
    }

    public int AdjustAttackDamage(int damage)
    {
        int newDamage = damage;

        //No adjustment if defense is zero
        if (Stats.Defense > 0)
        {
            float floatDamage = damage - (damage * Stats.Defense / 100);
            newDamage = (int)Math.Floor(floatDamage);
        }
        else if (Stats.Defense < 0)
        {
            float floatDamage = damage + (damage * Mathf.Abs(Stats.Defense) / 100);
            newDamage = (int)Math.Ceiling(floatDamage);
        }

        return newDamage;
    }

    public int AdjustAttackHealing(int healing)
    {
        int newHealing = healing;

        //No adjustment if vigor is zero
        float floatHealing = healing * Stats.Vigor / 100;
        newHealing = (int)Math.Floor(floatHealing);

        return newHealing;
    }

    private void AdjustShieldMaterial(bool shielded)
    {
        if(BubbleShielded)
        {
            foreach (SpriteRenderer renderer in ShieldSprites)
            {
                renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 1);
                renderer.material = bubbleShieldMaterial;
            }
        }
        else
        {
            if (shielded)
                foreach (SpriteRenderer renderer in ShieldSprites)
                {
                    renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 1);
                    renderer.material = shieldedMaterial;
                }
            else
                foreach (SpriteRenderer renderer in ShieldSprites)
                {
                    renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 0);
                    renderer.material = shieldedMaterial;
                }
        }
    }

    public void Stun(bool stunned)
    {
        //find renderer if you haven't already
        if (stunnedRenderer == null)
            stunnedRenderer = transform.Find("StunSprite").GetComponent<SpriteRenderer>();

        //Overrides tireless status
        if (Tireless)
            Tireless = false;

        //update sprite
        stunnedRenderer.enabled = stunned;
        Stunned = stunned;

        //if this is a player, update their vessel
        if (GetComponent<PlayerCharacter>())
            GetComponent<PlayerCharacter>().MyVessel.UpdateHealthText();
    }

    public virtual void Energize(bool energize)
    {
        //find renderer if you haven't already
        if (tirelessRenderer == null)
            tirelessRenderer = transform.Find("TirelessSprite").GetComponent<SpriteRenderer>();

        //Overrides stunned status
        if (Stunned)
            Stunned = false;

        //update sprite
        tirelessRenderer.enabled = energize;
        Tireless = energize;

        //if this is a player, update their vessel
        if (GetComponent<PlayerCharacter>())
            GetComponent<PlayerCharacter>().MyVessel.UpdateHealthText();
    }

    public virtual void Die()
    {
        DieEvent.Invoke();
        
        //play death animation
        Dead = true;
        GetComponent<CharacterStats>().enabled = false;
        foreach (SpriteRenderer renderer in TargetingSprites)
            renderer.enabled = false;
        foreach (SpriteRenderer renderer in ShieldSprites)
            renderer.enabled = false;
        turnOrder.RemoveFromSpeedList(GetComponent<CharacterStats>());
        turnOrder.DetermineTurnOrder();

        //Remove AUGs
        AugmentsToRemove.Clear();

        foreach (AugmentStackSO aug in AUGS)
            if (aug.RemoveOnDeath)
                AugmentsToRemove.Add(aug);
        foreach (AugmentStackSO aug in AugmentsToRemove)
            AdjustAugs(false, aug);

        // An additional subtract to Commands Executing for Die
        CombatManager.Instance.CommandsExecuting--;
    }

    public virtual void Resurrect(int newHealth, bool ignoreVigor = false)
    {
        if (!ignoreVigor)
            newHealth = AdjustAttackHealing(newHealth);

        if (newHealth >= Health)
            newHealth = Health;

        CurrentHealth = newHealth;
        Dead = false;
        GetComponent<CharacterStats>().enabled = true;
        foreach (SpriteRenderer renderer in TargetingSprites)
            renderer.enabled = true;
        turnOrder.AddToSpeedList(GetComponent<CharacterStats>());
        turnOrder.DetermineTurnOrder();
    }
    public void AddAugmentStack(AugmentStackSO stack, int stacksToAdd = 1, bool test = false)
    {
        if (stacksToAdd == 0) return;

        StartCoroutine(DisplayAugment(stack));

        foreach (AugmentStackSO aug in AUGS)
        {
            if (string.Equals(aug.AugmentName, stack.AugmentName))
            {
                if (aug.CurrentStacks < aug.MaxStacks)
                    aug.CurrentStacks += stacksToAdd;
                if (stack.StartUp || stack.StatChange || test)
                    aug.AugmentEffect();
                return;
            }
        }

        AugmentStackSO tempAUG = Instantiate(stack, transform);
        tempAUG.CurrentStacks = stacksToAdd;
        tempAUG.MyCharacter = this;

        AdjustAugs(true, tempAUG);

        if (stack.StartUp || stack.StatChange || test)
            tempAUG.AugmentEffect();
    }

    /// <summary>
    /// ONLY USE IF THIS AUGMENT IS NOT ON CHARACTER
    /// </summary>
    /// <param name="stack"></param>
    /// <param name="stacksToAdd"></param>
    /// <param name="test"></param>
    /// <returns></returns>
    public AugmentStackSO AddAugmentStackAndReturnReference(AugmentStackSO stack, int stacksToAdd = 1, bool test = false)
    {
        StartCoroutine(DisplayAugment(stack));

        AugmentStackSO tempAUG = Instantiate(stack, transform);
        tempAUG.CurrentStacks = stacksToAdd;
        tempAUG.MyCharacter = this;

        AdjustAugs(true, tempAUG);

        tempAUG.AugmentEffect();

        return tempAUG;
    }

    //Since SOs can't start the coroutine
    public void CallDisplayAugment(AugmentStackSO aug, bool removed = false)
    {
        StartCoroutine(DisplayAugment(aug, removed));
    }

    IEnumerator DisplayAugment(AugmentStackSO aug, bool removed = false)
    {
        //wait until the last augment finished
        while (augmenting)
            yield return null;

        augmenting = true;

        //grab augmentText if it doesn't exist
        if (!augmentText)
            augmentText = transform.Find("TertiaryText").GetComponent<TextMeshProUGUI>();

        //setup text
        augmentText.text = aug.AugmentName;
        if (removed)
            augmentText.text = "-" + augmentText.text;
        else
            augmentText.text = "+" + augmentText.text;
        augmentText.color = Color.white;

        //do fade
        while (augmentText.color.a > 0)
        {
            augmentText.transform.position += new Vector3(moveSpeed * Time.deltaTime, moveSpeed * Time.deltaTime, 0);
            augmentText.color -= new Color(0, 0, 0, fadeSpeed * Time.deltaTime);

            yield return null;
        }

        //cue next augment
        augmentText.text = "";
        augmenting = false;
    }

    public void RemoveAugmentStack(AugmentStackSO stack, int stackToRemove = 999, bool cleanUp = true)
    {
        StartCoroutine(DisplayAugment(stack, true));

        //Remove stacks
        foreach (AugmentStackSO aug in AUGS)
        {
            if (string.Equals(aug.AugmentName, stack.AugmentName))
            {
                aug.CurrentStacks -= stackToRemove;
                aug.DestroyAugment();
            }
        }

        if(cleanUp)
            CleanUpAUGs();
    }

    public void CleanUpAUGs()
    {
        //Clean up augs if they need to be removed
        foreach (AugmentStackSO augment in AugmentsToRemove)
        {
            AdjustAugs(false, augment);
            Destroy(augment);
        }
    }

    /// <summary>
    /// type 0 = remove debuffs
    /// type 1 = remove buffs
    /// type 2 = remove any
    /// </summary>
    /// <param name="amount"></param>
    /// <param name="type"></param>
    public void RemoveAmountOfAugments(int amount, int type)
    {
        int currentAmount = amount;

        List<AugmentStackSO> tempAUGS = RandomizeAugments();

        switch (type)
        {
            //remove debuffs
            case 0:
                foreach(AugmentStackSO debuff in tempAUGS)
                    if(debuff.IsDebuff && debuff.Removable)
                    {
                        if (currentAmount >= debuff.CurrentStacks)
                        {
                            int currentStacks = debuff.CurrentStacks;

                            StartCoroutine(DisplayAugment(debuff, true));
                            RemoveAugmentStack(debuff, currentStacks, false);
                            currentAmount -= currentStacks;
                        }
                        else if(currentAmount > 0)
                        {
                            StartCoroutine(DisplayAugment(debuff, true));
                            DisplayAugment(debuff, true);
                            RemoveAugmentStack(debuff, currentAmount);
                            currentAmount = 0;
                        }                        
                        else
                        {
                            break; //no more stacks to remove
                        }
                    }
                break;
            //remove buffs
            case 1:
                foreach (AugmentStackSO buff in tempAUGS)
                    if (buff.IsBuff && buff.Removable)
                    {
                        if (currentAmount >= buff.CurrentStacks)
                        {
                            int currentStacks = buff.CurrentStacks;

                            StartCoroutine(DisplayAugment(buff, true));
                            RemoveAugmentStack(buff, currentStacks, false);
                            currentAmount -= currentStacks;
                        }
                        else if (currentAmount > 0)
                        {
                            StartCoroutine(DisplayAugment(buff, true));
                            RemoveAugmentStack(buff, currentAmount);
                            currentAmount = 0;
                        }
                        else
                        {
                            break; //no more stacks to remove
                        }
                    }
                break;
            //remove augments
            case 2:
                foreach (AugmentStackSO augment in tempAUGS)
                    if(augment.Removable)
                    {
                        if (currentAmount >= augment.CurrentStacks)
                        {
                            int currentStacks = augment.CurrentStacks;

                            StartCoroutine(DisplayAugment(augment, true));
                            RemoveAugmentStack(augment, currentStacks, false);
                            currentAmount -= currentStacks;
                        }
                        else if (currentAmount > 0)
                        {
                            StartCoroutine(DisplayAugment(augment, true));
                            RemoveAugmentStack(augment, currentAmount);
                            currentAmount = 0;
                        }
                        else
                        {
                            break; //no more stacks to remove
                        }
                    }
                break;
            default:
                break;
        }

        CleanUpAUGs();
    }

    /// <summary>
    /// type 0 = proliferate debuffs
    /// type 1 = proliferate buffs
    /// type 2 = proliferate any
    /// </summary>
    /// <param name="amount"></param>
    /// <param name="type"></param>
    public void ProliferateAugment(int amount, int type)
    {
        int currentAmount = amount;

        List<AugmentStackSO> tempAUGS = RandomizeAugments();

        switch (type)
        {
            //proliferate debuffs
            case 0:
                foreach (AugmentStackSO debuff in tempAUGS)
                    if (debuff.IsDebuff && debuff.CurrentStacks < debuff.MaxStacks)
                    {
                        int amountToProliferate = debuff.MaxStacks - debuff.CurrentStacks;

                        if (currentAmount >= amountToProliferate)
                        {
                            StartCoroutine(DisplayAugment(debuff, false));
                            AddAugmentStack(debuff, amountToProliferate);
                            currentAmount -= amountToProliferate;
                        }
                        else if (currentAmount > 0)
                        {
                            StartCoroutine(DisplayAugment(debuff, false));
                            AddAugmentStack(debuff, currentAmount);
                            currentAmount -= currentAmount;
                        }
                        else
                        {
                            return;
                        }
                    }
                break;
            //proliferate buffs
            case 1:
                foreach (AugmentStackSO buff in tempAUGS)
                    if (buff.IsBuff && buff.CurrentStacks < buff.MaxStacks)
                    {
                        int amountToProliferate = buff.MaxStacks - buff.CurrentStacks;

                        if (currentAmount >= amountToProliferate)
                        {
                            StartCoroutine(DisplayAugment(buff, false));
                            AddAugmentStack(buff, amountToProliferate);
                            currentAmount -= amountToProliferate;
                        }
                        else if (currentAmount > 0)
                        {
                            StartCoroutine(DisplayAugment(buff, false));
                            AddAugmentStack(buff, currentAmount);
                            currentAmount -= currentAmount;
                        }
                        else
                        {
                            return;
                        }
                    }
                break;
            //proliferate augments
            case 2:
                foreach (AugmentStackSO augment in tempAUGS)
                {
                    if (augment.CurrentStacks < augment.MaxStacks)
                    {
                        int amountToProliferate = augment.MaxStacks - augment.CurrentStacks;

                        if (currentAmount >= amountToProliferate)
                        {
                            StartCoroutine(DisplayAugment(augment, false));
                            AddAugmentStack(augment, amountToProliferate);
                            currentAmount -= amountToProliferate;
                        }
                        else if (currentAmount > 0)
                        {
                            StartCoroutine(DisplayAugment(augment, false));
                            AddAugmentStack(augment, currentAmount);
                            currentAmount -= currentAmount;
                        }
                        else
                        {
                            return;
                        }
                    }
                }
                break;
            default:
                break;
        }
    }

    private List<AugmentStackSO> RandomizeAugments()
    {
        //Randomize AUGS
        System.Random random = new System.Random();
        List<AugmentStackSO> tempAUGS = AUGS;

        for (int i = 0; i < tempAUGS.Count - 1; i++)
        {
            int j = random.Next(i, tempAUGS.Count);
            AugmentStackSO temp = tempAUGS[i];
            tempAUGS[i] = tempAUGS[j];
            tempAUGS[j] = temp;
        }

        return tempAUGS;
    }

    /// <summary>
    /// type 0 = double debuffs
    /// type 1 = double buffs
    /// type 2 = double any
    /// </summary>
    /// <param name="amount"></param>
    /// <param name="type"></param>
    public void DoubleAugment(int type)
    {
        List<AugmentStackSO> tempAUGS = RandomizeAugments();

        switch (type)
        {
            //double debuffs
            case 0:
                foreach (AugmentStackSO debuff in tempAUGS)
                    if (debuff.IsDebuff && debuff.CurrentStacks < debuff.MaxStacks)
                    {
                        AddAugmentStack(debuff, debuff.CurrentStacks);
                        return;
                    }
                break;
            //double buffs
            case 1:
                foreach (AugmentStackSO buff in tempAUGS)
                    if (buff.IsBuff && buff.CurrentStacks < buff.MaxStacks)
                    {
                        AddAugmentStack(buff, buff.CurrentStacks);
                        return;
                    }
                break;
            //double augments
            case 2:
                foreach (AugmentStackSO augment in tempAUGS)
                {
                    if(augment.CurrentStacks < augment.MaxStacks)
                    {
                        AddAugmentStack(augment, augment.CurrentStacks);
                        return;
                    }
                }
                break;
            default:
                break;
        }
    }

    public void FlipCharacter(int position, bool player)
    {
        StartCoroutine(AdjustSpritePosition(position, player));
    }

    public IEnumerator AdjustSpritePosition(int position, bool player)
    {
        int oldSpot = CombatSpot;
        CombatSpot = position;
        
        //Adjust enemy manager
        if (player)
            EnemyManager.Instance.PlayerCombatSpots[CombatSpot] = this;
        else
            EnemyManager.Instance.EnemyCombatSpots[CombatSpot] = this;

        //Flip than swap position
        while (TargetingSprites[0].transform.eulerAngles.y < 180)
        {
            transform.Rotate(new Vector3(0, spriteRotationSpeed * Time.deltaTime, 0));
            yield return null;
        }
        while (TargetingSprites[0].transform.eulerAngles.y > 90)
        {
            transform.Rotate(new Vector3(0, spriteRotationSpeed * Time.deltaTime, 0));
            yield return null;
        }

        //Set exact rotation then move position
        transform.eulerAngles = new Vector3(0, 90, 0);

        //Rest order then add new combat spot
        foreach (SpriteRenderer renderer in TargetingSprites)
        {
            renderer.sortingOrder -= oldSpot;

            // Organize sorting groups
            SortingGroup sortGroup = renderer.transform.GetComponent<SortingGroup>(); 
            if (sortGroup != null)
                sortGroup.sortingOrder -= oldSpot; 
        }
        foreach (SpriteRenderer renderer in ShieldSprites)
            renderer.sortingOrder -= oldSpot;
        foreach (SpriteRenderer renderer in TargetingSprites)
        {
            renderer.sortingOrder += CombatSpot;

            // Organize sorting groups
            SortingGroup sortGroup = renderer.transform.GetComponent<SortingGroup>();
            if (sortGroup != null)
                sortGroup.sortingOrder += CombatSpot;
        }
        foreach (SpriteRenderer renderer in ShieldSprites)
            renderer.sortingOrder += CombatSpot;

        yield return null;

        transform.position = EnemyManager.Instance.PlayerLocations[CombatSpot].position;

        //Finish flip
        while (transform.eulerAngles.y <= 90)
        {
            transform.Rotate(new Vector3(0, spriteRotationSpeed * Time.deltaTime, 0));
            yield return null;
        }

        //Set exact rotation then move position
        transform.eulerAngles = Vector3.zero;
    }

    public virtual void AdjustMaxHealth(int adjuster)
    {
        Health += adjuster;
        if (Health <= 0)
            Health = 1;
        if (CurrentHealth > Health)
            CurrentHealth = Health;
    }

    public abstract void AdjustDefense(int defense);

    public void AdjustSpeed(int speed)
    {
        Stats.TrueSpeed += speed;

        //max double speed and min 40% reduction
        if (Stats.TrueSpeed > 200)
            Stats.Speed = 200;
        else if (Stats.TrueSpeed < 60)
            Stats.Speed = 60;
        else
            Stats.Speed = Stats.TrueSpeed;
    }

    public void AdjustVigor(int vigor)
    {
        Stats.TrueVigor += vigor;

        //max double vigor and min 0% healing
        if (Stats.TrueVigor > 200)
            Stats.Vigor = 200;
        else if (Stats.TrueVigor < 0)
            Stats.Vigor = 0;
        else
            Stats.Vigor = Stats.TrueVigor;
    }

    public void AdjustDamage(int damage)
    {
        Stats.TrueDamage += damage;

        //max x3 damage min 60% damage
        if (Stats.TrueDamage > 300)
            Stats.Damage = 300;
        else if (Stats.TrueDamage < 60)
            Stats.Damage = 60;
        else
            Stats.Damage = Stats.TrueDamage;
    }

    public void AdjustRestoration(int restoration)
    {
        Stats.TrueRestoration += restoration;

        //max double healing and min 40%
        if (Stats.TrueRestoration > 200)
            Stats.Damage = 200;
        else if (Stats.TrueRestoration < 40)
            Stats.Restoration = 40;
        else
            Stats.Restoration = Stats.TrueRestoration;
    }
    
    public void AdjustGravity(float gravityModifier)
    {
        Stats.TrueGravity += gravityModifier;

        if (Stats.TrueGravity > 2)
            Stats.Gravity = 2;
        else if (Stats.TrueGravity <= 0)
            Stats.Gravity = .01f;
        else
            Stats.Gravity = Stats.TrueGravity;
    }

    public void RandomizeStats(int amount, int positiveStatNumber, int negativeStatNumber)
    {
        List<int> randomNumbers = new List<int>();
        int totalRandom = positiveStatNumber + negativeStatNumber;
        int random = -1;

        for(int i = 0; i < totalRandom; i++)
        {
            while(randomNumbers.Contains(random) || random == -1)
                random = UnityEngine.Random.Range(0, totalRandom);

            randomNumbers.Add(random);
        }

        for(int i = 0; i < positiveStatNumber; i++)
        {
            UpdateStat(randomNumbers[i], amount);
        }
        for(int i = 0; i < negativeStatNumber; i++)
        {
            UpdateStat(randomNumbers[i], -amount);
        }
    }

    public void UpdateStat(int statIndex, int amount)
    {
        switch(statIndex)
        {
            case 0:
                AdjustDefense(amount);
                break;
            case 1:
                AdjustVigor(amount);
                break;
            case 2:
                AdjustSpeed(amount);
                break;
            case 3:
                AdjustDamage(amount);
                break;
            case 4:
                AdjustRestoration(amount);
                break;
            default:
                break;
        }
    }

    public abstract void StartTurn();
    public abstract void EndTurn();

    [Space(10)]
    [Header("Test AUG")]
    [SerializeField] AugmentStackSO test;

    [ContextMenu("Test AUG")]
    public void TestAUG()
    {
        AddAugmentStack(test, 1, true);
    }

    public void FadeAugments()
    {
        AugmentsToRemove.Clear();

        foreach (AugmentStackSO augment in AUGS)
            augment.Fade();
        foreach (AugmentStackSO augment in AugmentsToRemove)
            AdjustAugs(false, augment);
    }
}

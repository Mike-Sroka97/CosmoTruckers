using UnityEngine;

public class FunGun : CombatMove
{
    /// <summary>
    /// Min-Max values of oscillator
    /// </summary>
    [SerializeField] Vector2 oscillatorValues; 

    private void Start()
    {
        GenerateLayout();
        SetOscillatorValues(); 
    }

    public override void StartMove()
    {
        base.StartMove();
        FindObjectOfType<FGBulletSpawner>().SpawnBullet();
    }

    public override void EndMove()
    {
        if (FindObjectOfType<InaPractice>())
            return;

        MoveEnded = true;

        //Calculate Damage
        if (Score < 0)
            Score = 0;
        if (Score >= maxScore)
            Score = maxScore;

        int currentDamage;
        currentDamage = Score * Damage;

        currentDamage += baseDamage;

        //1 being base damage
        float DamageAdj = 1;
        float HealingAdj = 1;

        //Damage on players must be divided by 100 to multiply the final
        DamageAdj = CombatManager.Instance.GetCurrentCharacter.Stats.Damage / 100;
        HealingAdj = CombatManager.Instance.GetCurrentCharacter.Stats.Restoration / 100;

        LongDogMana mana = FindObjectOfType<LongDogMana>();
        if (currentDamage > 0)
        {
            switch (mana.LoadedBullets[0])
            {
                case 0:
                    CombatManager.Instance.GetCharactersSelected[0].TakeDamage((int)(currentDamage * DamageAdj + CombatManager.Instance.GetCurrentCharacter.FlatDamageAdjustment), true);
                    break;
                case 1:
                    currentDamage = mana.GoldBulletDamageAdjustment(currentDamage);
                    CombatManager.Instance.GetCharactersSelected[0].TakeDamage((int)(currentDamage * DamageAdj + CombatManager.Instance.GetCurrentCharacter.FlatDamageAdjustment), true);
                    break;
                case 2:
                    CombatManager.Instance.GetCharactersSelected[0].AddAugmentStack(DebuffToAdd, 2);
                    CombatManager.Instance.GetCharactersSelected[0].TakeHealing((int)(currentDamage * HealingAdj + CombatManager.Instance.GetCurrentCharacter.FlatHealingAdjustment), true);
                    break;
            }
        }

        mana.Shoot();
    }

    private void SetOscillatorValues()
    {
        Oscillator[] oscillators = FindObjectsOfType<Oscillator>();

        float differential = oscillatorValues.y - oscillatorValues.x; 
        float value1 = Random.Range(0, differential);
        oscillators[0].SetFrequency(oscillatorValues.x + value1); 
        oscillators[1].SetFrequency(oscillatorValues.y - value1); 
    }

    public override string TrainingDisplayText => $"You scored {Score = (Score > maxScore ? maxScore : Score)}/{maxScore} dealing {Score * Damage + baseDamage} damage with your gun.";
}

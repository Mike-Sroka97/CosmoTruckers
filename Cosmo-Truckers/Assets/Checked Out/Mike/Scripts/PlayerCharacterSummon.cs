using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerCharacterSummon : PlayerCharacter
{
    [Header("Special Effects")]
    [SerializeField] TextMeshProUGUI damageText;
    [SerializeField] TextMeshProUGUI healingText;
    [SerializeField] Color damageColor;
    [SerializeField] Color healingColor;
    [SerializeField] float fadeSpeed;
    [SerializeField] float moveSpeed;
    Vector3 damageTextStartPosition;
    Vector3 healingTextStartPosition;
    public PlayerCharacter Summoner;

    //TODO ADD SUMMON CREATION (ADD SUMMONER AS PARENT!!)
    public override void Die()
    {
        base.Die();
        EnemyManager.Instance.PlayerCombatSpots[CombatSpot] = null;
        TurnOrder.Instance.RemoveFromSpeedList(Stats);
        //TODO ANIMATIONS AND SFX??
        Destroy(gameObject);
    }

    public override void TakeDamage(int damage, bool defensePiercing = false)
    {
        base.TakeDamage(damage, defensePiercing);

        if (!defensePiercing)
            damage = AdjustAttackDamage(damage);

        StartCoroutine(DamageHealingEffect(true, damage.ToString()));
    }

    public override void TakeMultiHitDamage(int damage, int numberOfHits, bool defensePiercing = false)
    {
        base.TakeMultiHitDamage(damage, numberOfHits, defensePiercing);

        if (!defensePiercing)
            damage = AdjustAttackDamage(damage);

        StartCoroutine(DamageHealingEffect(true, damage.ToString(), numberOfHits));
    }

    public override void TakeHealing(int healing, bool ignoreVigor = false)
    {
        base.TakeHealing(healing, ignoreVigor);

        if (!ignoreVigor)
            healing = AdjustAttackHealing(healing);

        StartCoroutine(DamageHealingEffect(false, healing.ToString()));
    }

    public override void TakeMultiHitHealing(int healing, int numberOfHeals, bool ignoreVigor = false)
    {
        base.TakeMultiHitHealing(healing, numberOfHeals, ignoreVigor);

        if (!ignoreVigor)
            healing = AdjustAttackHealing(healing);

        StartCoroutine(DamageHealingEffect(false, healing.ToString(), numberOfHeals));
    }

    IEnumerator DamageHealingEffect(bool damage, string text = null, int numberOfHits = 1)
    {
        for (int i = 0; i < numberOfHits; i++)
        {
            damageTextStartPosition = damageText.transform.localPosition;
            healingTextStartPosition = healingText.transform.localPosition;

            //COLE TODO replace color with SFX
            if (damage)
            {
                damageText.text = text;

                foreach (SpriteRenderer renderer in TargetingSprites)
                    renderer.color = damageColor;

                damageText.color = damageColor;
                while (damageText.color.a > 0)
                {
                    damageText.transform.position += new Vector3(-moveSpeed * Time.deltaTime, moveSpeed * Time.deltaTime, 0);
                    damageText.color -= new Color(0, 0, 0, fadeSpeed * Time.deltaTime);

                    foreach (SpriteRenderer renderer in TargetingSprites)
                        renderer.color += new Color(fadeSpeed * Time.deltaTime, fadeSpeed * Time.deltaTime, fadeSpeed * Time.deltaTime, 1);

                    yield return null;
                }
            }

            //COLE TODO replace color with SFX
            else if (!damage)
            {
                healingText.text = text;

                foreach (SpriteRenderer renderer in TargetingSprites)
                    renderer.color = healingColor;

                healingText.color = healingColor;
                while (healingText.color.a > 0)
                {
                    healingText.transform.position += new Vector3(-moveSpeed * Time.deltaTime, moveSpeed * Time.deltaTime, 0);
                    healingText.color -= new Color(0, 0, 0, fadeSpeed * Time.deltaTime);

                    foreach (SpriteRenderer renderer in TargetingSprites)
                        renderer.color += new Color(fadeSpeed * Time.deltaTime, fadeSpeed * Time.deltaTime, fadeSpeed * Time.deltaTime, 1);

                    yield return null;
                }
            }

            damageText.transform.localPosition = damageTextStartPosition;
            healingText.transform.localPosition = healingTextStartPosition;
        }
    }
}

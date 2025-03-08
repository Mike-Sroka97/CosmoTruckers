using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AS_Heart : MonoBehaviour
{
    [SerializeField] float flashTime;
    [SerializeField] Color damageFlashColor;
    [SerializeField] TextMeshProUGUI damageTotal;

    IEnumerator FlashRed()
    {
        GetComponent<SpriteRenderer>().color = damageFlashColor;
        yield return new WaitForSeconds(flashTime);
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("EnemyDamaging"))
        {
            StartCoroutine(FlashRed());
            int totalDamage = Int32.Parse(damageTotal.text);
            totalDamage += collision.GetComponent<AS_AmberStar>().GetDamage();
            damageTotal.text = totalDamage.ToString();
            Destroy(collision.transform.gameObject);
        }
    }
}

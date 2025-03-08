using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparkShieldCollectable : MonoBehaviour
{
    [SerializeField] int score;

    [SerializeField] bool isProton;
    [SerializeField] bool isNeutron;
    [SerializeField] bool isElectron;

    SparkShield miniGame;

    public void Initialize()
    {
        miniGame = FindObjectOfType<SparkShield>();
    }

    private void TypeSpecificTrigger(bool playerIframes)
    {
        if(isProton)
        {
            if (playerIframes)
                return;

            miniGame.Score += score;
            miniGame.CheckScoreAndAugmentSuccess(); 
        }
        else if(isNeutron)
        {
            miniGame.AugmentScore++;
            miniGame.CheckScoreAndAugmentSuccess();
        }
        else if(isElectron)
        {
            if (playerIframes)
                return;

            miniGame.Score -= score;
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
            TypeSpecificTrigger(FindObjectOfType<Player>().iFrames);
    }
}

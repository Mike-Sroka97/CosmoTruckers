using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CounterBopJumpCheck : MonoBehaviour
{
    [SerializeField] float timeToFillBalls;
    [SerializeField] SpriteRenderer bg;

    bool fillingYourBalls;
    bool ballsFilled;
    float currentBallsFillingSessionDuration;
    Image myImage;
    CounterBop minigame;
    Collider2D myCollider;

    private void Awake()
    {
        myImage = GetComponentInChildren<Image>();
        minigame = GetComponentInParent<CounterBop>();
        myCollider = GetComponentInChildren<Collider2D>();
    }

    void Update()
    {
        ManageBallFilling();
    }

    private void ManageBallFilling()
    {
        if (ballsFilled)
            return;

        if (fillingYourBalls)
        {
            currentBallsFillingSessionDuration += Time.deltaTime;

            if(currentBallsFillingSessionDuration >= timeToFillBalls)
            {
                minigame.IncrementScore();
                ballsFilled = true;
                myCollider.enabled = false;
                return;
            }

            myImage.fillAmount = currentBallsFillingSessionDuration / timeToFillBalls;
            myImage.color = new Color((timeToFillBalls - currentBallsFillingSessionDuration) / timeToFillBalls, 1, 0);
        }
        else
        {
            currentBallsFillingSessionDuration -= Time.deltaTime * 2;
            if (currentBallsFillingSessionDuration < 0)
                currentBallsFillingSessionDuration = 0;

            myImage.fillAmount = currentBallsFillingSessionDuration / timeToFillBalls;
            myImage.color = new Color((timeToFillBalls - currentBallsFillingSessionDuration) / timeToFillBalls, 1, 0);
        }
    }

    public void EnableBalls()
    {
        bg.enabled = true;
        myCollider.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            fillingYourBalls = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            fillingYourBalls = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolidShlurpingPlatforms : MonoBehaviour
{
    [SerializeField] SpriteRenderer[] platforms;
    [SerializeField] Color phaseOutColor;
    [SerializeField] Color startingColor;
    [SerializeField] float blinkSpeed = 0.25f;

    float blinkTimer = 0;
    bool blinking = false; 

    private void Update()
    {
        if (blinking)
        {
            blinkTimer += Time.deltaTime; 

            if (blinkTimer > blinkSpeed)
            {
                if (platforms[0].color == startingColor)
                {
                    foreach (SpriteRenderer platform in platforms)
                    {
                        platform.color = phaseOutColor;
                    }
                }
                else
                {
                    foreach (SpriteRenderer platform in platforms)
                    {
                        platform.color = startingColor;
                    }
                }

                blinkTimer = 0; 
            }
        }
    }

    private void OnEnable()
    {
        foreach (SpriteRenderer platform in platforms)
        {
            platform.color = startingColor;
        }
    }

    public void StartBlinking()
    {
        blinking = true; 
    }

    public void PhaseOut()
    {
        blinking = false;

        foreach(SpriteRenderer platform in platforms)
        {
            platform.color = phaseOutColor;
        }
    }
}

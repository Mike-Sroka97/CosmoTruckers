using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolidShlurpingPlatforms : MonoBehaviour
{
    [SerializeField] SpriteRenderer[] platforms;
    [SerializeField] Color phaseOutColor;
    [SerializeField] Color startingColor;

    private void OnEnable()
    {
        foreach (SpriteRenderer platform in platforms)
        {
            platform.color = startingColor;
        }
    }

    public void PhaseOut()
    {
        foreach(SpriteRenderer platform in platforms)
        {
            platform.color = phaseOutColor;
        }
    }
}

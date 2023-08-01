using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstorIncubation : CombatMove
{
    [SerializeField] float ballDelayTime;
    [SerializeField] GameObject[] platformsToDisable;

    private void Start()
    {
        StartMove();
        GenerateLayout();
        Invoke("DisablePlatforms", ballDelayTime);
    }

    private void Update()
    {
        TrackTime();        
    }

    private void DisablePlatforms()
    {
        foreach(GameObject platform in platformsToDisable)
        {
            platform.SetActive(false);
        }
    }

    protected override void TrackTime()
    {
        base.TrackTime();
    }

    public override void EndMove()
    {
        base.EndMove();
    }
}

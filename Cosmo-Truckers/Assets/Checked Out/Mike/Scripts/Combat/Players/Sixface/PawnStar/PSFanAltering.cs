using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSFanAltering : MonoBehaviour
{
    [SerializeField] float timeBetweenSwitch;

    GeneralUseFan[] fans;
    PSFanColorHandler[] colorHandlers;
    float currentTime = 0;

    private void Start()
    {
        fans = GetComponentsInChildren<GeneralUseFan>();
        colorHandlers = GetComponentsInChildren<PSFanColorHandler>();

        FanSwap();
    }

    private void Update()
    {
        TrackTime();   
    }

    private void TrackTime()
    {
        currentTime += Time.deltaTime;
        if(currentTime >= timeBetweenSwitch)
        {
            currentTime = 0;
            FanSwap();
        }
    }

    private void FanSwap()
    {
        //activate all fans then deactivate two
        foreach(GeneralUseFan fan in fans)
        {
            fan.enabled = true;
        }
        foreach(PSFanColorHandler fan in colorHandlers)
        {
            fan.ActivateColor();
        }

        //randomize active fans

        int random1 = UnityEngine.Random.Range(0, 2);
        int random2 = random1;
        while(random2 == random1)
        {
            random2 = UnityEngine.Random.Range(0, fans.Length);
        }

        colorHandlers[random1].DeactivateColor();
        colorHandlers[random2].DeactivateColor();
        fans[random1].enabled = false;
        fans[random2].enabled = true;
    }
}

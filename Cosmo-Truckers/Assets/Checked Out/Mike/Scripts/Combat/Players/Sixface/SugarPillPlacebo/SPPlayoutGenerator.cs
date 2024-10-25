using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SPPlayoutGenerator : MonoBehaviour
{
    [SerializeField] GameObject[] layouts;
    [SerializeField] SPPswitch mySwitch;
    [SerializeField] float layoutDuration;

    float currentTime = 0;
    bool activeLayout = false;
    int random = -1;
    int lastRandom = -1;
    GameObject currentActiveLayout;
    TMP_Text timerText; 

    private void Update()
    {
        if(activeLayout)
        {
            currentTime += Time.deltaTime;
            if(currentTime > layoutDuration)
            {
                DestroyMe();
            }
            else
            {
                timerText.text = (Mathf.RoundToInt(layoutDuration - currentTime)).ToString();
            }
        }
    }

    public void DestroyMe()
    {
        timerText.text = ""; 
        currentTime = 0;
        activeLayout = false;
        mySwitch.ResetMe();
        Destroy(currentActiveLayout);
    }

    public void GenerateLayout(TMP_Text text)
    {
        timerText = text;
        activeLayout = true;

        while(random == lastRandom)
        {
            random = UnityEngine.Random.Range(0, layouts.Length);
        }

        lastRandom = random;
        currentActiveLayout = Instantiate(layouts[random], transform);
    }
}

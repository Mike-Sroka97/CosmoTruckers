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

    private void Update()
    {
        if(activeLayout)
        {
            currentTime += Time.deltaTime;
            if(currentTime > layoutDuration)
            {
                DestroyMe();
            }
        }
    }

    public void DestroyMe()
    {
        currentTime = 0;
        activeLayout = false;
        mySwitch.ResetMe();
        Destroy(currentActiveLayout);
    }

    public void GenerateLayout(bool left)
    {
        activeLayout = true;

        while(random == lastRandom)
            random = Random.Range(0, layouts.Length);

        lastRandom = random;

        currentActiveLayout = Instantiate(layouts[random], transform);

        if (!left)
            currentActiveLayout.transform.localEulerAngles = new Vector3(currentActiveLayout.transform.localEulerAngles.x, 180, currentActiveLayout.transform.localEulerAngles.z);
    }
}

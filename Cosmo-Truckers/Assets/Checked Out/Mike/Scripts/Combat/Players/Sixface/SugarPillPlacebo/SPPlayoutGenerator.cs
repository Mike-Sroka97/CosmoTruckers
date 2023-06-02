using System.Collections;
using System.Collections.Generic;
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
                currentTime = 0;
                activeLayout = false;
                mySwitch.ResetMe();
                Destroy(currentActiveLayout);
            }
        }
    }

    public void GenerateLayout()
    {
        activeLayout = true;


        //TODO Cole unlock this method from its chains once you have 2+ layouts
        //while(random == lastRandom)
        //{
        //    random = UnityEngine.Random.Range(0, layouts.Length);
        //}

        random = UnityEngine.Random.Range(0, layouts.Length);
        lastRandom = random;
        currentActiveLayout = Instantiate(layouts[random], transform);
    }
}

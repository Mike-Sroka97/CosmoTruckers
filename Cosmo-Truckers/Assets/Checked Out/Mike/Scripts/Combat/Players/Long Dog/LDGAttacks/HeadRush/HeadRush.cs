using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadRush : MonoBehaviour
{
    GameObject currentLayout;
    List<GameObject> usedLayouts = new List<GameObject>();
    [SerializeField] GameObject[] layouts;
    [SerializeField] int maxLayouts = 3;
    int numberOfLayouts = 0; 

    [HideInInspector] public int Score;
    int random;
    int lastRandom = -1;

    private void Start()
    {
        DetermineLayout();
    }

    public void DetermineLayout()
    {
        if (currentLayout)
        {
            Destroy(currentLayout);
        }

        if (lastRandom == -1)
        {
            random = Random.Range(0, layouts.Length);
            currentLayout = layouts[random];
            numberOfLayouts++;

            currentLayout = Instantiate(layouts[random], currentLayout.transform.position, Quaternion.identity, gameObject.transform);
            usedLayouts.Add(layouts[random]);
            lastRandom = random;
        }
        else if (numberOfLayouts <= maxLayouts)
        {
            while (usedLayouts.Contains(layouts[random]))
            {
                random = Random.Range(0, layouts.Length);
                currentLayout = layouts[random];
                numberOfLayouts++;
            }

            currentLayout = Instantiate(layouts[random], currentLayout.transform.position, Quaternion.identity, gameObject.transform);
            usedLayouts.Add(layouts[random]);
            lastRandom = random;
        }
        

    }
}

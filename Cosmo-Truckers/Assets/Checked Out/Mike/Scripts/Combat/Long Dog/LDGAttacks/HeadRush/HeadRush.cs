using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadRush : MonoBehaviour
{
    [SerializeField] GameObject currentLayout;
    [SerializeField] GameObject[] layouts;

    [HideInInspector] public int SuccessRate;
    int random;
    int lastRandom = 0;

    private void Start()
    {
        DetermineLayout();
    }

    public void DetermineLayout()
    {
        if (currentLayout.transform.childCount > 0)
        {
            Destroy(currentLayout.transform.GetChild(0));
        }

        //while (lastRandom == random)
        //{
        //    random = Random.Range(0, layouts.Length);
        //    currentLayout = layouts[random];
        //}
        random = Random.Range(0, layouts.Length);
        Instantiate(layouts[random], currentLayout.transform);

        lastRandom = random;
    }
}

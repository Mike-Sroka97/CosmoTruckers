using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class TestTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public float maxTime;
    private float currentTime;
    public Image blackImage; 

    private void Start()
    {
        currentTime = maxTime + 1;
        blackImage.enabled = false; 
    }

    // Update is called once per frame
    void Update()
    {
        currentTime -= Time.deltaTime; 
        int time = (int)currentTime;
        timerText.text = time.ToString();

        if (currentTime <= 0)
        {
            blackImage.enabled = true;
        }
    }
}

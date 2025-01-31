using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPPcheckpoint : MonoBehaviour
{
    [SerializeField] SPPswitch mySwitch;

    SugarPillPlacebo minigame;

    private void Start()
    {
        minigame = FindObjectOfType<SugarPillPlacebo>();
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" )
        {
            minigame.CurrentCheckPointLocation = transform.position;
            
            if (minigame.CurrentSwitch != mySwitch)
            {
                minigame.CurrentSwitch = mySwitch;
                mySwitch.ResetMe();
                FindObjectOfType<SPPlayoutGenerator>().DestroyMe();
            }
        }
    }
}

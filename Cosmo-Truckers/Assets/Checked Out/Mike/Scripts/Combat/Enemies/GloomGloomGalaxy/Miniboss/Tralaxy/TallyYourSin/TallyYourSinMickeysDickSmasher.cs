using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TallyYourSinMickeysDickSmasher : MonoBehaviour
{
    [SerializeField] GameObject hurtZone;

    public void ToggleMickeysDickSmasher()
    {
        hurtZone.SetActive(!hurtZone.activeInHierarchy);
    }
}

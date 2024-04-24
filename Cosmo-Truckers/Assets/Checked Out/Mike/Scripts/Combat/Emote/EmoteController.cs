using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmoteController : MonoBehaviour
{
    public GameObject[] Emotes;

    private void Start()
    {
        foreach(GameObject emote in Emotes)
            Instantiate(emote, transform);

        //gameObject.SetActive(false);
    }
}

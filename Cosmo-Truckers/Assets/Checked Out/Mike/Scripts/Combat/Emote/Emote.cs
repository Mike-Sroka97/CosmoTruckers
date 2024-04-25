using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Emote : MonoBehaviour
{
    public GameObject EmoteToSpawn;
    public Image Icon;

    public void InitEmote(Emote emoteToModel)
    {
        if (!Icon)
            Icon = GetComponentInChildren<Image>();

        EmoteToSpawn = emoteToModel.EmoteToSpawn;
        Icon.sprite = emoteToModel.Icon.sprite;
    }
}

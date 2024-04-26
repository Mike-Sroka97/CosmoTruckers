using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnedEmote : MonoBehaviour
{
    public void ResetEmoteSelector()
    {
        transform.parent.GetComponentInParent<EmoteController>().CanOpen = true;
        Destroy(gameObject);
    }
}

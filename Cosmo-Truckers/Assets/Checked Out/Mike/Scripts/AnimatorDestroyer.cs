using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorDestroyer : MonoBehaviour
{
    public void DestroyMe()
    {
        Destroy(gameObject);
    }
}

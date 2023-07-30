using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBody : MonoBehaviour
{
    public Player Body;

    private void Start()
    {
        if(Body == null)
            Body = GetComponentInParent<Player>();
    }
}

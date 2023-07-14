using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    Animator myAnimator;

    private void Start()
    {
        myAnimator = GetComponent<Animator>();
    }

    public void SetSpeed(float speed)
    {
        myAnimator.speed = speed;
    }
}

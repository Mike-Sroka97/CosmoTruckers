using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PorkanatorMeatballHelper : MonoBehaviour
{
    [SerializeField] bool spinRight;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.GetComponent<PorkanatorHurt>())
        {
            collision.transform.GetComponent<PorkanatorHurt>().SetSpinRight(spinRight);
        }
    }
}

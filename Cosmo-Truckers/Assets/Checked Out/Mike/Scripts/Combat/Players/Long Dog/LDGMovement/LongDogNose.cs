using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongDogNose : MonoBehaviour
{
    [SerializeField] float neckWaitTime = .5f;

    bool waitTime = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "PlayerHurtable" && collision.tag != "PlayerAttack" && collision.tag != "LDGNoInteraction")
        {
            if (collision.name == "NeckLineRenderer(Clone)" && waitTime)
                GetComponentInParent<LongDogINA>().StretchingCollision(collision.tag);
            else if (collision.name != "NeckLineRenderer(Clone)")
                GetComponentInParent<LongDogINA>().StretchingCollision(collision.tag);
        }
    }

    private void OnEnable()
    {
        waitTime = false;
        StartCoroutine(FirstNeckHelper());
    }

    private IEnumerator FirstNeckHelper()
    {
        yield return new WaitForSeconds(neckWaitTime);
        waitTime = true;
    }
}

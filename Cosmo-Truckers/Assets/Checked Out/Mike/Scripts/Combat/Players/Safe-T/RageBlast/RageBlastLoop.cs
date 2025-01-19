using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RageBlastLoop : MonoBehaviour
{
    [SerializeField] public float MoveSpeed;
    [SerializeField] float colorAdjuster;

    Rigidbody2D myBody;
    CapsuleCollider2D myCollider;
    RageBlast minigame;
    SpriteRenderer[] myRenderers;
    bool addedScore = false;

    public void InitializeLoop(bool right)
    {
        myRenderers = GetComponentsInChildren<SpriteRenderer>();
        myCollider = GetComponentInChildren<CapsuleCollider2D>();
        myBody = GetComponent<Rigidbody2D>();
        minigame = FindObjectOfType<RageBlast>();

        if (right)
        {
            MoveSpeed = -MoveSpeed;
            transform.localEulerAngles = new Vector3(0, 180, 0);
        }

        myBody.velocity = new Vector2(MoveSpeed, 0);
    }


    public IEnumerator FadeOut()
    {
        if(!addedScore)
        {
            minigame.Score++;
            Debug.Log(minigame.Score);
            addedScore = true;
            minigame.CheckSuccess();
        }

        myCollider.enabled = false;

        while(myRenderers[0].color.a > 0)
        {
            foreach(SpriteRenderer renderer in myRenderers)
            {
                renderer.color += new Color(Time.deltaTime * colorAdjuster, Time.deltaTime * colorAdjuster, Time.deltaTime * colorAdjuster, Time.deltaTime * -colorAdjuster);
            }
            transform.localScale += new Vector3(Time.deltaTime, Time.deltaTime, Time.deltaTime);
            yield return new WaitForSeconds(Time.deltaTime);
        }

        Destroy(gameObject);
    }
}

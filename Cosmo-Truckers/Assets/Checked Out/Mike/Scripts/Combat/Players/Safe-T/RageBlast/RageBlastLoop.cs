using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RageBlastLoop : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float colorAdjuster;

    Rigidbody2D myBody;
    CapsuleCollider2D myCollider;
    RageBlast minigame;
    SpriteRenderer[] myRenderers;
    bool addedScore = false;

    private void Start()
    {
        myRenderers = GetComponentsInChildren<SpriteRenderer>();
        myCollider = GetComponentInChildren<CapsuleCollider2D>();
        myBody = GetComponent<Rigidbody2D>();
        myBody.velocity = new Vector2(moveSpeed, 0);
        minigame = FindObjectOfType<RageBlast>();
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

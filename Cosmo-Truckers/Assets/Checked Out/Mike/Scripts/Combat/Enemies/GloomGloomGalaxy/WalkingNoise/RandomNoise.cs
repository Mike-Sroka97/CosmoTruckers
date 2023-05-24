using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomNoise : MonoBehaviour
{
    [SerializeField] float fadeSpeed;
    [SerializeField] float fadeOutSpeed;

    SpriteRenderer myRenderer;
    Color startingColor;
    Collider2D myCollider;
    WhiteNoise minigame;
    bool active = false;

    private void Start()
    {
        myRenderer = GetComponent<SpriteRenderer>();
        startingColor = myRenderer.color;
        myCollider = GetComponent<Collider2D>();
        minigame = FindObjectOfType<WhiteNoise>();
    }

    public void NewSpawn()
    {
        StopAllCoroutines();
        myRenderer.color = startingColor;
        StartCoroutine(Fade());
    }

    IEnumerator Fade()
    {
        while(myRenderer.color.a < 1)
        {
            myRenderer.color = new Color(myRenderer.color.r, myRenderer.color.g, myRenderer.color.b, myRenderer.color.a + Time.deltaTime * fadeSpeed);
            yield return new WaitForSeconds(Time.deltaTime);

            if(myRenderer.color.a > .25f && !active)
            {
                active = true;
                myCollider.enabled = true;
            }
        }
        while (myRenderer.color.a > 0)
        {
            myRenderer.color = new Color(myRenderer.color.r, myRenderer.color.g, myRenderer.color.b, myRenderer.color.a - Time.deltaTime * fadeOutSpeed);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        myCollider.enabled = false;
        active = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            minigame.Score++;
            myCollider.enabled = false;
            Debug.Log(minigame.Score);
        }
    }
}

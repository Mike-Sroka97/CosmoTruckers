using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CascadingGoliathNodes : MonoBehaviour
{
    [HideInInspector] public bool Hit = false;
    [SerializeField] float iFrameDuration = .5f;

    CascadingGoliath minigame;
    int hitpoints;
    int currentHit = 0;
    bool iFrames;

    private void Start()
    {
        minigame = FindObjectOfType<CascadingGoliath>();

        hitpoints = FindObjectsOfType<Player>().Length;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "PlayerAttack" && !iFrames)
        {
            StartCoroutine(Iframes());
            currentHit++;
        }

        if(collision.tag == "PlayerAttack" && currentHit >= hitpoints)
        {
            Hit = true;
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
            minigame.CheckPhase();
        }
    }

    IEnumerator Iframes()
    {
        iFrames = true;
        GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 1);
        yield return new WaitForSeconds(iFrameDuration);
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        iFrames = false;
    }
}

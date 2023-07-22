using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoDShape : MonoBehaviour
{
    [SerializeField] bool goodShape;
    [SerializeField] float colliderTime = 0.5f;
    [SerializeField] float resetTime = 1.5f;
    [SerializeField] int score;

    SpriteRenderer myRenderer; 
    LongevityOfDog minigame;
    LoDShapeGenerator shapeGenerator;
    Collider2D myCollider;

    private void Start()
    {
        myRenderer = GetComponent<SpriteRenderer>();
        minigame = transform.parent.GetComponentInParent<LongevityOfDog>();
        shapeGenerator = GetComponentInParent<LoDShapeGenerator>();
        myCollider = GetComponent<Collider2D>();
        StartCoroutine(ColliderDelay());
    }

    IEnumerator ColliderDelay()
    {
        myCollider.enabled = false;
        yield return new WaitForSeconds(colliderTime);
        myCollider.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "Attack Zone")
        {
            if (goodShape)
            {
                myCollider.enabled = false;
                myRenderer.color = new Color(0, 0, 1);
                shapeGenerator.CorrectShapes++;
                if (shapeGenerator.CorrectShapes >= 3)
                {
                    FindObjectOfType<LongDogINA>().SetDamaged(false);
                    StartCoroutine(ResetWait());
                }
            }
            else
            {
                FindObjectOfType<LongDogINA>().SetDamaged(true);
                myCollider.enabled = false;
                myRenderer.color = new Color(1, 0, 0);
                StartCoroutine(ResetWait());
            }
        }
    }

    IEnumerator ResetWait()
    {
        minigame.Score += score;
        yield return new WaitForSeconds(resetTime);
        Destroy(transform.parent.gameObject);
        minigame.ResetShapes();
    }
}

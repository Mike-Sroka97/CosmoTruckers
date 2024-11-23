using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class LoDShape : MonoBehaviour
{
    [SerializeField] bool goodShape;
    [SerializeField] float colliderTime = 0.5f;
    [SerializeField] float resetTime = 0.5f;
    // This is for layouts where there are two of the same shape, but one is false. Allows player to choose either one first
    public bool SecondShapeFalse; 
    int scoreToAdd = 1;

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

        // Set the score
        scoreToAdd = goodShape? 3 : -1;
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
            myRenderer.material = minigame.offMaterial;
            myCollider.enabled = false;

            if (goodShape)
            {
                if (SecondShapeFalse)
                {
                    LoDShape[] shapes = FindObjectsOfType<LoDShape>();
                    foreach (LoDShape shape in shapes)
                    {
                        if (shape.SecondShapeFalse && shape.gameObject != gameObject)
                            shape.SetThisShapeFalse(); 
                    }
                }

                myRenderer.color = minigame.correctShapeColor;
                shapeGenerator.CorrectShapes++;
                if (shapeGenerator.CorrectShapes >= 3)
                {
                    FindObjectOfType<LoDShapeToMake>().UpdateShapeColors(minigame.correctShapeColor); 
                    FindObjectOfType<LongDogINA>().SetDamaged(false);
                    StartCoroutine(ResetWait());
                }
            }
            else
            {
                FindObjectOfType<LongDogINA>().SetDamaged(true);
                myRenderer.color = minigame.wrongShapeColor;
                FindObjectOfType<LoDShapeToMake>().UpdateShapeColors(minigame.wrongShapeColor);
                StartCoroutine(ResetWait());
            }
        }
    }

    public void SetThisShapeFalse()
    {
        this.goodShape = false;
        scoreToAdd = 0; 
    }

    IEnumerator ResetWait()
    {
        minigame.Score += scoreToAdd;
        FindObjectOfType<LongDogINA>().SetDamaged(false);
        yield return new WaitForSeconds(resetTime);
        minigame.CheckSuccess(); 
        Destroy(transform.parent.gameObject);
        minigame.ResetShapes();
    }
}

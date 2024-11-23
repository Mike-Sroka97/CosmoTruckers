using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class LoDShape : MonoBehaviour
{
    [SerializeField] bool goodShape;
    [SerializeField] float colliderTime = 0.5f;
    [SerializeField] float resetTime = 0.15f;
    // This is for layouts where there are two of the same shape, but one is false. Allows player to choose either one first
    public bool SecondShapeFalse; 
    int scoreToAdd = 3;

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

        if (!goodShape)
            SetThisShapeFalse(); 
    }

    IEnumerator ColliderDelay()
    {
        myCollider.enabled = false;
        yield return new WaitForSeconds(colliderTime);
        myCollider.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if((collision.name == "Attack Zone" || collision.tag == "Player") && minigame.Score == 0)
        {
            myRenderer.material = minigame.offMaterial;
            myCollider.enabled = false;

            if (goodShape)
            {
                // Because they're being instantiated, we do it this way
                if (SecondShapeFalse)
                {
                    LoDShape[] shapes = FindObjectsOfType<LoDShape>();
                    foreach (LoDShape shape in shapes)
                    {
                        if (shape.SecondShapeFalse && shape.gameObject != gameObject)
                        {
                            shape.SetThisShapeFalse();
                            break; 
                        }

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
                if (collision.name == "Attack Zone")
                    FindObjectOfType<LongDogINA>().SetDamaged(true);

                myRenderer.color = minigame.wrongShapeColor;
                FindObjectOfType<LoDShapeToMake>().UpdateShapeColors(minigame.wrongShapeColor);
                minigame.PlayerDead = true;
                StartCoroutine(ResetWait());
            }
        }
    }

    public void SetThisShapeFalse()
    {
        gameObject.AddComponent<TrackPlayerDeath>();
        goodShape = false;
        scoreToAdd = -3; 
    }

    IEnumerator ResetWait()
    {
        minigame.Score += scoreToAdd;
        yield return new WaitForSeconds(resetTime);
        minigame.CheckScoreEqualsValue(scoreToAdd); // Always call it here
    }
}

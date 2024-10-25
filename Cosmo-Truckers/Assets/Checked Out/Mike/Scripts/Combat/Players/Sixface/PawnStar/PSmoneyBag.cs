using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSmoneyBag : MonoBehaviour
{
    [SerializeField] Color hurtColor;
    [SerializeField] float deactiveTime;
    [SerializeField] float amplitude;
    [SerializeField] float frequency;
    [SerializeField] float xBounds;
    [SerializeField] float movementSpeed;

    SpriteRenderer myRenderer;
    Color startingColor;
    PawnStar minigame;
    Collider2D myCollider;
    Vector3 startingPosition;
    bool movingRight;

    private void Start()
    {
        minigame = FindObjectOfType<PawnStar>();
        myRenderer = GetComponent<SpriteRenderer>();
        myCollider = GetComponent<Collider2D>();
        startingColor = myRenderer.color;
        startingPosition = transform.localPosition;

        int random = UnityEngine.Random.Range(0, 2); //coin flip huzzah
        if (random == 0)
            movingRight = false;
        else
            movingRight = true;
    }

    private void Update()
    {
        HorizontalMovement();
        Oscillate();
    }

    private void HorizontalMovement()
    {
        if(movingRight)
        {
            transform.localPosition += new Vector3(movementSpeed * Time.deltaTime, 0, 0);
            if(Mathf.Abs(transform.localPosition.x) > xBounds)
            {
                movingRight = !movingRight;
                transform.localPosition = new Vector3(xBounds, transform.localPosition.y, transform.localPosition.z);
            }
        }
        else
        {
            transform.localPosition -= new Vector3(movementSpeed * Time.deltaTime, 0, 0);
            if (Mathf.Abs(transform.localPosition.x) > xBounds)
            {
                movingRight = !movingRight;
                transform.localPosition = new Vector3(-xBounds, transform.localPosition.y, transform.localPosition.z);
            }
        }
    }

    private void Oscillate()
    {
        float newY = startingPosition.y + Mathf.Sin(Time.time * frequency) * amplitude;
        transform.localPosition = new Vector3(transform.localPosition.x, newY, transform.localPosition.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "PlayerAttack")
        {
            minigame.Score++;
            minigame.CheckSuccess(); 
            Debug.Log(minigame.Score);
            StartCoroutine(ResetHitbox());
        }
    }

    IEnumerator ResetHitbox()
    {
        myCollider.enabled = false;
        myRenderer.color = hurtColor;
        yield return new WaitForSeconds(deactiveTime);
        myCollider.enabled = true;
        myRenderer.color = startingColor;
    }
}

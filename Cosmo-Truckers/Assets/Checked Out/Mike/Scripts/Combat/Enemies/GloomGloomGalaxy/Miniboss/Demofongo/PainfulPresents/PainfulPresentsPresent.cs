using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PainfulPresentsPresent : MonoBehaviour
{
    [Header("Movement Variables")]
    [SerializeField] Transform centerGoal;
    [SerializeField] float moveSpeed;

    [Space(20)]
    [Header("Collectable Variables")]
    [SerializeField] Material badMaterial;
    [SerializeField] Material goodMaterial;
    [SerializeField] Sprite goodSprite, badSprite, healSprite; 

    SpriteRenderer myRenderer;
    bool movingInward = true;
    Vector3 startingPosition;

    private void Start()
    {
        myRenderer = GetComponent<SpriteRenderer>();
        startingPosition = transform.localPosition;
    }

    private void Update()
    {
        MoveMe();
    }

    private void MoveMe()
    {
        if(movingInward)
        {
            Vector3 goal = centerGoal.position - transform.parent.position;
            transform.localPosition = Vector2.MoveTowards(transform.localPosition, goal, moveSpeed * Time.deltaTime);

            if (transform.localPosition == goal)
                movingInward = false;
        }
        else
        {
            Vector3 goal = startingPosition;
            transform.localPosition = Vector2.MoveTowards(transform.localPosition, goal, moveSpeed * Time.deltaTime);

            if (transform.localPosition == goal)
                movingInward = true;
        }
    }

    public void SetPresent(int typeOfPresent)
    {
        if (myRenderer == null)
        {
            myRenderer = GetComponent<SpriteRenderer>();
        }

        switch (typeOfPresent)
        {
            //bad case
            case 0:
                myRenderer.material = badMaterial;
                myRenderer.sprite = badSprite;
                Destroy(GetComponent<PlayerPickup>());
                break;
            //good case
            case 1:
                myRenderer.material = goodMaterial;
                myRenderer.sprite = goodSprite;
                GetComponent<PlayerPickup>().SetScoringTypes(true, false);
                GetComponent<PlayerPickup>().SetScore(-1);
                Destroy(GetComponent<TrackPlayerDeath>());
                break;
            //heal  case
            case 2:
                myRenderer.material = goodMaterial;
                myRenderer.sprite = healSprite; 
                GetComponent<PlayerPickup>().SetScoringTypes(false, true);
                Destroy(GetComponent<TrackPlayerDeath>());
                break;
        }
    }
}

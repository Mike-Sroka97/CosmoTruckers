using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwTrain : OverworldCharacter
{
    [SerializeField] Sprite horizontalSprite;
    [SerializeField] Sprite upSprite;
    [SerializeField] Sprite downSprite;

    SpriteRenderer yedRenderer;
    SpriteRenderer cartRenderer;

    Transform yed;
    Transform trainCart; //TODO FOLLOWER TYPE BEAT

    OverworldCharacter characterController;

    protected override void Start()
    {
        yed = transform.Find("Yed");
        trainCart = transform.Find("Train Cart (0)");
        overworld = FindObjectOfType<Overworld>();

        enabled = false;
    }

    public void Board(Transform spawnPosition, OverworldCharacter character, Transform[] pointsToTraverse)
    {
        characterController = character;
        yed.position = spawnPosition.position;
        characterController.transform.position = trainCart.transform.position;

        character.StopAllCoroutines();
        character.enabled = false;
        character.transform.parent = trainCart;

        StartCoroutine(Move(pointsToTraverse));
    }

    public void Unboard(Transform spawnPosition, OverworldCharacter character)
    {
        characterController = character;
        yed.position = spawnPosition.position;
        characterController.transform.position = trainCart.transform.position;

        enabled = false;
    }

    protected override IEnumerator Move(Transform[] pointsToTraverse)
    {
        moving = true;
        int currentPoint = 0;

        while (currentPoint < pointsToTraverse.Length)
        {
            //Move toward each point one by one
            while (yed.position != pointsToTraverse[currentPoint].transform.position)
            {
                float lastX = transform.position.x;
                float lastY = transform.position.y;

                yed.position = Vector3.MoveTowards(yed.position, pointsToTraverse[currentPoint].transform.position, moveSpeed * Time.deltaTime);

                //Flip on complex up / down movements
                if (lastX > transform.position.x)
                {
                    yedRenderer.flipX = true;
                    yedRenderer.sprite = horizontalSprite;
                }
                else if (lastX < transform.position.x)
                {
                    yedRenderer.flipX = false;
                    yedRenderer.sprite = horizontalSprite;
                }

                //Flip up down based on y
                if (lastY > transform.position.y)
                    yedRenderer.sprite = downSprite;
                else if (lastY < transform.position.y)
                    yedRenderer.sprite = upSprite;

                //Wait a frame you fucking nerd
                yield return null;
            }
            currentPoint++;
        }

        overworld.CurrentNode.SetupNode();

        moving = false;
    }
}

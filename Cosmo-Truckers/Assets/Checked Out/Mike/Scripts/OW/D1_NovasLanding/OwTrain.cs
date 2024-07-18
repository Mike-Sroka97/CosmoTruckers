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
    Transform trainCart;

    CameraController camera;

    protected override void Start()
    {
        yed = transform.Find("Yed");
        trainCart = transform.Find("Train Cart (0)");
        yedRenderer = yed.GetComponent<SpriteRenderer>();
        cartRenderer = trainCart.GetComponent<SpriteRenderer>();
        overworld = FindObjectOfType<Overworld>();
        camera = FindObjectOfType<CameraController>();

        enabled = false;
    }

    public void Board(Transform spawnPosition, OverworldCharacter character, Transform[] pointsToTraverse)
    {
        enabled = true;
        yed.position = spawnPosition.position;

        character.StopAllCoroutines();
        character.enabled = false;
        character.transform.parent = trainCart.Find("Train Cart Player Position");
        character.transform.localPosition = Vector3.zero;
        camera.Leader = yed;

        StartCoroutine(Move(pointsToTraverse));
    }

    public void Unboard(Transform spawnPosition, OverworldCharacter character, Transform[] pointsToTraverse)
    {
        StopAllCoroutines();
        yed.position = spawnPosition.position;
        trainCart.position = yed.position;

        character.enabled = true;
        character.transform.parent = transform.parent;
        StartCoroutine(character.Move(pointsToTraverse));
        camera.Leader = character.transform;

        enabled = false;
    }

    public override IEnumerator Move(Transform[] pointsToTraverse)
    {
        moving = true;
        int currentPoint = 0;

        while (currentPoint < pointsToTraverse.Length)
        {
            Vector3 point = new Vector3(pointsToTraverse[currentPoint].transform.position.x, pointsToTraverse[currentPoint].transform.position.y + .57f, pointsToTraverse[currentPoint].transform.position.z);

            //Move toward each point one by one
            while (yed.position != point)
            {
                float lastX = yed.localPosition.x;
                float lastY = yed.localPosition.y;

                yed.position = Vector3.MoveTowards(yed.position, point, moveSpeed * Time.deltaTime);

                //Flip on complex up / down movements
                if (lastX > yed.localPosition.x)
                {
                    yedRenderer.sprite = horizontalSprite;
                    yedRenderer.flipX = true;
                }
                else if (lastX < yed.localPosition.x)
                {
                    yedRenderer.sprite = horizontalSprite;
                    yedRenderer.flipX = false;
                }

                //Flip up down based on y
                if(lastX != yed.position.x)
                {
                    if (lastY > yed.localPosition.y)
                        yedRenderer.sprite = downSprite;
                    else if (lastY < yed.localPosition.y)
                        yedRenderer.sprite = upSprite;
                } 

                //Wait a frame you fucking nerd
                yield return null;
            }
            currentPoint++;
        }

        overworld.CurrentNode.SetupNode();

        moving = false;
    }
}

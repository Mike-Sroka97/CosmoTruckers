using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwTrainFollower : Follower
{
    [SerializeField] Sprite horizontalSprite;
    [SerializeField] Sprite verticalSprite;

    float lastY;
    float lastX;
    SpriteRenderer myRenderer;

    private void Start()
    {
        myRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void LateUpdate()
    {
        if (lastX != transform.localPosition.x)
        {
            myRenderer.sprite = horizontalSprite;
            transform.Find("Train Cart Front").gameObject.SetActive(true);
        }

        else if (lastY != transform.localPosition.y)
        {
            myRenderer.sprite = verticalSprite;
            transform.Find("Train Cart Front").gameObject.SetActive(false);
        }

        lastX = transform.localPosition.x;
        lastY = transform.localPosition.y;

        base.LateUpdate();
    }
}

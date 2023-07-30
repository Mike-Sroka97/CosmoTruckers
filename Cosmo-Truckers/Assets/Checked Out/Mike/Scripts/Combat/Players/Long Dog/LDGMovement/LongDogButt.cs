using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongDogButt : MonoBehaviour
{
    [SerializeField] float speed;

    LongDogNeck neck;
    LongDogINA dogINA;
    List<Vector2> points;

    bool isStretching = false;

    private void Start()
    {
        dogINA = transform.parent.parent.GetComponent<LongDogINA>();
    }

    private void Update()
    {
        if(isStretching && points.Count > 0)
        {
            if((Vector2)transform.localPosition != points[0])
            {
                transform.localPosition = Vector2.MoveTowards(transform.localPosition, points[0], speed * Time.deltaTime);
                if(points.Count > 1)
                    transform.right = -(new Vector3(points[1].x, points[1].y, 0) - transform.position);
            }
            else
            {
                neck.RemovePoint();
                
                if(points.Count <= 0)
                {
                    transform.right = Vector3.right;
                    dogINA.ATHDone();
                    isStretching = false;
                }
            }
        }
        else if(isStretching)
        {
            dogINA.ATHDone();
            isStretching = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "EnemyDamaging")
        {
            dogINA.StretchingCollision(collision.gameObject.tag);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "EnemyDamaging")
        {
            dogINA.StretchingCollision(collision.gameObject.tag);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.tag == "EnemyDamaging")
        {
            dogINA.StretchingCollision(collision.gameObject.tag);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.tag == "EnemyDamaging")
        {
            dogINA.StretchingCollision(collision.gameObject.tag);
        }
    }

    public void StartButtToHeadMovement()
    {
        neck = FindObjectOfType<LongDogNeck>();
        if (neck)
        {
            points = neck.GetPointList();
            if(points.Count > 0)
            {
                dogINA.SetCanMove(false);
                isStretching = true;
                transform.localPosition = points[0];
            }
            else if(!isStretching)
            {
                isStretching = true;
            }
        }
    }
}

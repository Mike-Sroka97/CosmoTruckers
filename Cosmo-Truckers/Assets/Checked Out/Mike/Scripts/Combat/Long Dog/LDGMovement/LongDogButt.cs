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
                Vector2 dir = points[0] - (Vector2)transform.position;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle - 180);
            }
            else
            {
                neck.RemovePoint();
                
                if(points.Count <= 0)
                {
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

    public void StartButtToHeadMovement()
    {
        neck = FindObjectOfType<LongDogNeck>();
        if(neck)
        {
            points = neck.GetPointList();
            if(points.Count > 0)
            {
                dogINA.SetCanMove(false);
                isStretching = true;
                transform.localPosition = points[0];
            }
        }
    }
}

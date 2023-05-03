using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongDogButt : MonoBehaviour
{
    [SerializeField] float speed;

    int currentIndex;
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
        if(isStretching)
        {
            if((Vector2)transform.localPosition != points[currentIndex])
            {
                transform.localPosition = Vector2.MoveTowards(transform.localPosition, points[currentIndex], speed * Time.deltaTime);
                Vector2 dir = points[currentIndex] - (Vector2)transform.position;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle - 180);
            }
            else
            {
                neck.RemovePoint();
                
                if(points.Count == 0)
                {
                    currentIndex = 0;
                    dogINA.ATHDone();
                    isStretching = false;
                }
            }
        }
    }

    public void StartButtToHeadMovement()
    {
        currentIndex = 0;
        dogINA.SetCanMove(false);
        neck = FindObjectOfType<LongDogNeck>();
        if(neck)
        {
            points = neck.GetPointList();
            isStretching = true;
            transform.localPosition = points[0];
        }
    }
}

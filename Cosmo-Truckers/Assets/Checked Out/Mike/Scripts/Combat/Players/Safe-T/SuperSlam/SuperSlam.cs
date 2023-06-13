using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperSlam : MonoBehaviour
{
    [HideInInspector] public int Score;
    [SerializeField] Transform[] objectsToMove;
    [SerializeField] SuperSlamScroll upScroll;
    [SerializeField] SuperSlamScroll downScroll;
    [HideInInspector] public float MaxY;
    public float MinY;

    private void Start()
    {
        MaxY = 0;
    }

    public void ScrollUp(float scrollSpeed)
    {
        foreach(Transform obj in objectsToMove)
        {
            float tempY = obj.localPosition.y + scrollSpeed * Time.deltaTime;

            if (obj.GetComponent<Rigidbody2D>())
            {
                if(!upScroll.enabled && scrollSpeed > 0)
                {
                    obj.GetComponent<Rigidbody2D>().velocity = new Vector2(obj.GetComponent<Rigidbody2D>().velocity.x, 0);
                }
            }
            else
            {
                if (tempY > MaxY && scrollSpeed > 0)
                {
                    downScroll.enabled = false;
                    upScroll.enabled = true;
                    obj.localPosition = new Vector3(obj.localPosition.x, MaxY, obj.localPosition.z);
                }
                else if (-tempY < MinY && scrollSpeed < 0)
                {
                    downScroll.enabled = true;
                    upScroll.enabled = false;
                    obj.localPosition = new Vector3(obj.localPosition.x, MinY, obj.localPosition.z);
                }
                else if (tempY < MaxY && tempY > MinY)
                {
                    obj.localPosition += new Vector3(0, scrollSpeed * Time.deltaTime, 0);
                }
            }
        }
    }
}

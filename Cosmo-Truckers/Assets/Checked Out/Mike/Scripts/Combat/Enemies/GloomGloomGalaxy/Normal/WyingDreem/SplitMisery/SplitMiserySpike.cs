using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitMiserySpike : MonoBehaviour
{
    [SerializeField] float amplitude = .875f;
    [SerializeField] float frequency = 1.0f;
    [SerializeField] bool upSideDown = false;
    [SerializeField] bool useX;
    [SerializeField] bool left;

    private Vector3 initialPosition;
    Collider2D myCollider;

    private void Start()
    {
        initialPosition = transform.position;
        myCollider = GetComponentInChildren<Collider2D>();
    }

    private void Update()
    {
        TrackPosition();
        MoveMe();
    }

    private void TrackPosition()
    {
        if(!useX)
        {
            if (!upSideDown)
            {
                if (transform.position.y < initialPosition.y)
                {
                    myCollider.enabled = false;
                }
                else
                {
                    myCollider.enabled = true;
                }
            }
            else
            {
                if (transform.position.y > initialPosition.y)
                {
                    myCollider.enabled = false;
                }
                else
                {
                    myCollider.enabled = true;
                }
            }
        }
        else
        {
            if (!left)
            {
                if (transform.position.x < initialPosition.x)
                {
                    myCollider.enabled = false;
                }
                else
                {
                    myCollider.enabled = true;
                }
            }
            else
            {
                if (transform.position.x > initialPosition.x)
                {
                    myCollider.enabled = false;
                }
                else
                {
                    myCollider.enabled = true;
                }
            }
        }
    }

    private void MoveMe()
    {
        if(!useX)
        {
            float newY = initialPosition.y + amplitude * Mathf.Cos(Time.time * frequency);
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }
        else
        {
            float newX = initialPosition.x + amplitude * Mathf.Cos(Time.time * frequency);
            transform.position = new Vector3(newX, transform.position.y, transform.position.z);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitMiserySpike : MonoBehaviour
{
    [SerializeField] float amplitude = .875f;
    [SerializeField] float frequency = 1.0f;

    private Vector3 initialPosition;
    Collider2D myCollider;
    bool initialized = false;
    public void Initialize()
    {
        initialPosition = transform.localPosition;
        myCollider = GetComponentInChildren<Collider2D>();
        initialized = true;
    }

    private void Update()
    {
        if (!initialized)
            return;

        TrackPosition();
        MoveMe();
    }

    private void TrackPosition()
    {
        if (transform.localPosition.y > initialPosition.y)
        {
            myCollider.enabled = true;
        }
        else
        {
            myCollider.enabled = false;
        }
    }

    private void MoveMe()
    {
        float newY = initialPosition.y + amplitude * Mathf.Cos(Time.time * frequency);
        transform.localPosition = new Vector3(transform.localPosition.x, newY, transform.localPosition.z);
    }
}

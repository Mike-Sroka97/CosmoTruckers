using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QDgust : MonoBehaviour
{
    [SerializeField] float gustDuration;

    float currentTime = 0;
    private void Update()
    {
        TrackTime();
    }

    private void TrackTime()
    {
        currentTime += Time.deltaTime;
        if(currentTime >= gustDuration)
        {
            Destroy(transform.parent.parent.gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongDogStretchHUB : MonoBehaviour
{
    [SerializeField] float maxY;
    [SerializeField] float speed;

    public void StartStretch()
    {
        GetComponent<Animator>().enabled = false;
        StartCoroutine(Stretch());
    }

    private IEnumerator Stretch()
    {
        while(transform.position.y < maxY)
        {
            transform.position += new Vector3(0, speed * Time.deltaTime, 0);
            yield return null;
        }

        transform.position = new Vector3(transform.position.x, maxY, transform.position.z);
    }
}

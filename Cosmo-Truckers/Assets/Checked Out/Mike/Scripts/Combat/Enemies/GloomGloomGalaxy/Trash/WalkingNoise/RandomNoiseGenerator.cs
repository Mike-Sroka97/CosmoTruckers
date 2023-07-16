using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomNoiseGenerator : MonoBehaviour
{
    [SerializeField] RandomNoise[] noises;
    [SerializeField] float radius;
    [SerializeField] float timeBetweenNoise;
    [SerializeField] float xBound;
    [SerializeField] float yBound;

    int currentIndex = 0;
    float currentTime = 0;
    GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        TrackTime();
    }

    private void TrackTime()
    {
        currentTime += Time.deltaTime;
        if(currentTime >= timeBetweenNoise)
        {
            currentTime = 0;
            MoveRandomNoise();
        }
    }

    private void MoveRandomNoise()
    {
        float angle = Random.Range(0f, 2f * Mathf.PI);
        float x = Mathf.Sin(angle) * radius;
        float y = Mathf.Cos(angle) * radius;
        x = PutInBoundsX(x);
        y = PutInBoundsY(y);
        Vector3 randomPoint = new Vector3(x, y, 0) + player.transform.position;
        noises[currentIndex].transform.position = randomPoint;
        noises[currentIndex].NewSpawn();
        if(currentIndex + 1 >= noises.Length)
        {
            currentIndex = 0;
        }
        else
        {
            currentIndex++;
        }
    }

    private float PutInBoundsX(float x)
    {
        if(x + player.transform.position.x >= xBound || x + player.transform.position.x <= -xBound)
        {
            return -x;
        }
        else
        {
            return x;
        }
    }

    private float PutInBoundsY(float y)
    {
        if (y + player.transform.position.y >= yBound || y + player.transform.position.y <= -yBound)
        {
            return -y;
        }
        else
        {
            return y;
        }
    }
}

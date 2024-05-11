using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TallyYourSinCircle : MonoBehaviour
{
    [SerializeField] TallyYourSinSin[] sins;
    [SerializeField] float radius;
    [SerializeField] float timeBetweenShots;

    bool[] firedSins;
    PlayerBody player;
    int sinCount;

    bool initialized = false;

    private void Start()
    {
        firedSins = new bool[sins.Length];

        sinCount = sins.Length;

        float angleIncrement = 360 / sins.Length;

        for (int i = 0; i < sins.Length; i++)
        {
            float angleDegrees = i * angleIncrement;
            float angleRadians = angleDegrees * Mathf.Deg2Rad;

            float x = radius * Mathf.Cos(angleRadians);
            float y = radius * Mathf.Sin(angleRadians);

            sins[i].transform.localPosition = new Vector3(x, y, 0);
        }
    }

    public void Initialize()
    {
        player = FindObjectOfType<PlayerBody>();

        StartCoroutine(FireSin());
        initialized = true;
    }

    private void Update()
    {
        if (!initialized)
            return;

        // Special case for Long Dog targeting head
        transform.position = player.transform.position;
    }

    IEnumerator FireSin()
    {
        yield return new WaitForSeconds(timeBetweenShots);

        int random = Random.Range(0, sins.Length);

        while(firedSins[random] == true)
        {
            random = Random.Range(0, sins.Length);
        }

        firedSins[random] = true;

        // Special case for Long Dog targeting head
        sins[random].FireMe(player.transform);

        sinCount--;
        if(sinCount > 0)
        {
            StartCoroutine(FireSin());
        }
    }
}

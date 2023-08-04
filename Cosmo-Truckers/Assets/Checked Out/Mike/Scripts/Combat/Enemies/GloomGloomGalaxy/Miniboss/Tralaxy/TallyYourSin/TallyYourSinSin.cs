using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TallyYourSinSin : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float moveDuration;
    [SerializeField] float fireDelay;

    Player player;

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }
    public void FireMe()
    {
        transform.parent = null;
        transform.right = player.transform.position - transform.position;
        StartCoroutine(FireCo());
    }

    IEnumerator FireCo()
    {
        yield return new WaitForSeconds(fireDelay);

        float currentTime = 0;

        while(currentTime < moveDuration)
        {
            transform.position += transform.right * moveSpeed * Time.deltaTime;
            currentTime += Time.deltaTime;
            yield return null;
        }
    }
}

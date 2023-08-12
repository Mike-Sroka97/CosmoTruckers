using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunOfTheMawHead : MonoBehaviour
{
    [SerializeField] float dashDelay;
    [SerializeField] float dashSpeed;

    Player player;

    private void Start()
    {
        player = FindObjectOfType<Player>();

        StartCoroutine(Dash());
    }

    IEnumerator Dash()
    {
        yield return new WaitForSeconds(dashDelay);

        Vector3 target = player.transform.position;

        while(transform.position != target)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, dashSpeed * Time.deltaTime);

            yield return null;
        }

        StartCoroutine(Dash());
    }
}

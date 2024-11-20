using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarStormStar : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float rotateSpeed;
    [SerializeField] int numberOfFlashes;
    [SerializeField] float flashDuration;
    [SerializeField] GameObject babyStar;
    [SerializeField] float spawnInterval = 0.2f;
    [Header("0 = right, 1 = left, 2 = up, 3 = down")]
    [SerializeField] int direction;

    StarStorm minigame;
    bool isMoving = false;
    SpriteRenderer arrow;

    private void Start()
    {
        minigame = FindObjectOfType<StarStorm>();
        arrow = GetComponentsInChildren<SpriteRenderer>()[1];
        StartCoroutine(Flash());
    }

    IEnumerator Flash()
    {
        int currentNumberOfFlashes = 0;

        while(currentNumberOfFlashes < numberOfFlashes)
        {
            arrow.enabled = false;
            yield return new WaitForSeconds(flashDuration);
            arrow.enabled = true;
            yield return new WaitForSeconds(flashDuration);
            currentNumberOfFlashes++;
        }

        arrow.enabled = false;
        isMoving = true;

        StartCoroutine(SpawnStar());
    }

    IEnumerator SpawnStar()
    {
        Instantiate(babyStar, transform.position, transform.rotation, transform.parent);

        yield return new WaitForSeconds(spawnInterval);

        StartCoroutine(SpawnStar());
    }

    private void Update()
    {
        MoveMe();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            minigame.Score++;
            minigame.AugmentScore++;
            Destroy(gameObject);
        }
        else if(collision.GetComponent<StarStormBlock>())
        {
            Destroy(gameObject);
        }
    }

    private void MoveMe()
    {
        if (!isMoving)
            return;

        transform.Rotate(new Vector3(0, 0, rotateSpeed * Time.deltaTime));

        switch (direction)
        {
            case 0:
                transform.position += Vector3.right * moveSpeed * Time.deltaTime;
                break;
            case 1:
                transform.position += Vector3.left * moveSpeed * Time.deltaTime;
                break;
            case 2:
                transform.position += Vector3.up * moveSpeed * Time.deltaTime;
                break;
            case 3:
                transform.position += Vector3.down * moveSpeed * Time.deltaTime;
                break;
            default:
                break;
        }
    }
}

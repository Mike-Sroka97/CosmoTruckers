using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullCourseProjectile : MonoBehaviour
{
    [SerializeField] bool goodProjectile;
    [SerializeField] float fallSpeed;
    [SerializeField] float rotateSpeed;

    FullCourse minigame;

    private void Start()
    {
        minigame = FindObjectOfType<FullCourse>();
    }

    private void Update()
    {
        MoveMe();
        RotateMe();
    }

    private void MoveMe()
    {
        transform.position -= new Vector3(0, fallSpeed * Time.deltaTime, 0);
    }

    private void RotateMe()
    {
        transform.Rotate(0, 0, -rotateSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(goodProjectile && collision.tag == "Player")
        {
            minigame.Score++;
            Debug.Log(minigame.Score);
            Destroy(gameObject);
        }
        else if(collision.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}

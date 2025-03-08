using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullCourseProjectile : MonoBehaviour
{
    [SerializeField] bool goodProjectile;
    [SerializeField] float fallSpeed;
    [SerializeField] float rotateSpeed;
    [SerializeField] float fallSpeedVariance = 0.2f;

    FullCourse minigame;

    private void Start()
    {
        fallSpeed += Random.Range(-fallSpeedVariance, fallSpeedVariance);
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
        if(goodProjectile && collision.CompareTag("Player"))
        {
            minigame.Score++;
            minigame.CheckSuccess(); 
            Debug.Log(minigame.Score);
            Destroy(gameObject);
        }
        else if(collision.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}

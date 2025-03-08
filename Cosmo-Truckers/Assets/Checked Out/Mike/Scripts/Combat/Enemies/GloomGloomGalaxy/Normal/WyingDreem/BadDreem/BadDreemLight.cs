using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadDreemLight : MonoBehaviour
{
    [SerializeField] Transform[] waypoints;
    [SerializeField] float moveSpeed;

    int currentIndex = 0;
    Transform parent;
    CombatMove minigame; 

    private void Start()
    {
        parent = transform.parent;
        minigame = FindObjectOfType<CombatMove>();
    }

    private void Update()
    {
        MoveToWaypoint();
    }

    private void MoveToWaypoint()
    {
        if (!minigame.MoveEnded)
        {
            if (currentIndex >= waypoints.Length)
                return;

            parent.position = Vector2.MoveTowards(parent.position, waypoints[currentIndex].position, moveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("EnemyNonDamaging"))
        {
            currentIndex++;
            Destroy(collision.gameObject);
        }
    }
}

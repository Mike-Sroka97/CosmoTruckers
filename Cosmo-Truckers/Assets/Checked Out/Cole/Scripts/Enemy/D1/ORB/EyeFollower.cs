using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeFollower : MonoBehaviour
{
    Player[] players;
    Player currentTarget;
    float minDistance = Mathf.Infinity;
    [SerializeField] float rotateSpeed;

    // Start is called before the first frame update
    void Start()
    {
        players = FindObjectsOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        LookAtPlayer(); 
    }

    private void LookAtPlayer()
    {
        CalculatePlayerDistances();

        float angle = Mathf.Atan2(currentTarget.transform.position.y - transform.position.y, currentTarget.transform.position.x - transform.position.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
    }

    public void CalculatePlayerDistances()
    {
        float closestDistance = 100;

        foreach (Player player in players)
        {
            if (Vector2.Distance(transform.position, player.transform.position) < closestDistance)
            {
                currentTarget = player;
                closestDistance = Vector2.Distance(transform.position, player.transform.position);
            }
        }
    }
}

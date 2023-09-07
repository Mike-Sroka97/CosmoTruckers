using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionBasedMovement : MonoBehaviour
{
    [Header("down = 0 up = 1 left = 2 right = 3")]
    [SerializeField] int direction;
    [SerializeField] float moveSpeed; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MovePosition();
    }

    private void MovePosition()
    {
        if (direction == 0)
        {
            transform.position += new Vector3(0, -moveSpeed * Time.deltaTime, 0);
        }
        else if (direction == 1)
        {
            transform.position += new Vector3(0, moveSpeed * Time.deltaTime, 0);
        }
        else if (direction == 2)
        {
            transform.position += new Vector3(-moveSpeed * Time.deltaTime, 0, 0);
        }
        else if (direction == 3)
        {
            transform.position += new Vector3(moveSpeed * Time.deltaTime, 0, 0);
        }
    }
}

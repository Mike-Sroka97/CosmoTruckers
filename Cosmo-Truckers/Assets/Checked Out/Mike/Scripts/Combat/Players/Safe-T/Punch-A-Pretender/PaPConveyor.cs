using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaPConveyor : MonoBehaviour
{
    [SerializeField] float moveSpeed;

    Rigidbody2D myBody;

    private void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
        myBody.velocity = new Vector2(moveSpeed, 0);
    }

    public float GetMoveSpeed() { return moveSpeed; }
}

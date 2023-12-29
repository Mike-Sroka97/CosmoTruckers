using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MelancholyPrecipitationLayoutMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed;

    Rigidbody2D myBody;

    private void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
    }

    public void Initialize()
    {
        myBody.velocity = new Vector2(0, -moveSpeed);
        GetComponent<ParentPlayer>().AdjustPlayerVelocity(myBody.velocity.x, myBody.velocity.y, FindObjectOfType<Player>());
    }
}

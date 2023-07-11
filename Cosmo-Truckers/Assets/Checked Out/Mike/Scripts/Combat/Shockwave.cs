using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shockwave : MonoBehaviour
{
    [SerializeField] bool killPlayer = false;
    [SerializeField] float moveSpeed;
    [SerializeField] float shrinkSpeed;
    [SerializeField] float disableColliderScale = 0.2f;
    [SerializeField] float destroyScale = 0.05f;

    CombatMove minigame;
    Collider2D myCollider;

    private void Start()
    {
        myCollider = GetComponentInChildren<Collider2D>();
        minigame = FindObjectOfType<CombatMove>();
        transform.localScale = Vector3.one;
    }

    private void Update()
    {
        MoveMe();
        ShrinkMe();
    }

    private void MoveMe()
    {
        if(transform.eulerAngles.y == 0)
        {
            transform.position += Vector3.left * moveSpeed * Time.deltaTime;
        }
        else
        {
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;
        }
    }

    private void ShrinkMe()
    {
        transform.localScale -= new Vector3(shrinkSpeed * Time.deltaTime, shrinkSpeed * Time.deltaTime, shrinkSpeed * Time.deltaTime);

        if (transform.localScale.x < disableColliderScale)
        {
            myCollider.enabled = false;
        }

        if (transform.localScale.x < destroyScale)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && killPlayer)
        {
            minigame.PlayerDead = true;
        }
    }
}

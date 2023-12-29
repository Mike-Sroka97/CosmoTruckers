using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shockwave : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float shrinkSpeed;
    [SerializeField] float disableColliderScale = 0.2f;
    [SerializeField] float destroyScale = 0.05f;
    [SerializeField] float destroyClamp;
    [SerializeField] bool startingOneScale = true;

    CombatMove minigame;
    Collider2D myCollider;

    private void Start()
    {
        myCollider = GetComponentInChildren<Collider2D>();
        minigame = FindObjectOfType<CombatMove>();

        if(startingOneScale)
            transform.localScale = Vector3.one;
    }

    private void Update()
    {
        MoveMe();
        ShrinkMe();
        ClampCheck();
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
        if (shrinkSpeed == 0)
            return;

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

    private void ClampCheck()
    {
        if(transform.position.x > destroyClamp || transform.position.x < -destroyClamp)
        {
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CometkazeBall : MonoBehaviour
{
    [SerializeField] float shrinkDistance;
    [SerializeField] float shrinkSpeed;
    [SerializeField] float moveSpeed;
    [SerializeField] float destroySize;

    private void Update()
    {
        Shrink();
        MoveMe();        
    }

    private void Shrink()
    {
        if(Vector3.Distance(transform.position, Vector3.zero) <= shrinkDistance)
        {
            transform.localScale -= new Vector3(shrinkSpeed, shrinkSpeed, shrinkSpeed);
            if(transform.localScale.x <= destroySize)
            {
                Destroy(gameObject);
            }
        }
    }

    private void MoveMe()
    {
        transform.position = Vector3.MoveTowards(transform.position, Vector3.zero, moveSpeed * Time.deltaTime);
    }
}

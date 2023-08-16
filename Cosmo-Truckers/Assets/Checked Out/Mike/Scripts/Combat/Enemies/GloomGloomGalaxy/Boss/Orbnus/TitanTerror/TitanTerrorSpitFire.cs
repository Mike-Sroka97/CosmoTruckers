using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitanTerrorSpitFire : MonoBehaviour
{
    [SerializeField] float startingXvelocity;
    [SerializeField] float startingYvelocity;

    Rigidbody2D myBody;
    const float minY = -5f;

    private void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
        myBody.velocity = new Vector2(startingXvelocity, startingYvelocity);
    }

    private void Update()
    {
        if(transform.position.y < minY)
        {
            Destroy(gameObject);
        }
    }
}

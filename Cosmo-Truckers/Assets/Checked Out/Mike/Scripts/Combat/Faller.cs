using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Faller : MonoBehaviour
{
    [SerializeField] float destroyY;
    [SerializeField] float fallSpeed;

    private void Update()
    {
        Fall();
    }

    private void Fall()
    {
        transform.position -= new Vector3(0, fallSpeed * Time.deltaTime, 0);

        if (transform.position.y <= destroyY)
            Destroy(gameObject);
    }
}

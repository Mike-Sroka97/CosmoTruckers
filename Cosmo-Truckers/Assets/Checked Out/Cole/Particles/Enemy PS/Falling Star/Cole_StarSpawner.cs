using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cole_StarSpawner : MonoBehaviour
{
    public float maxRange;
    public float timeBetweenStars = 0.5f;
    private float timer;
    public GameObject fallingStar; 

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime; 

        if (timer > timeBetweenStars)
        {
            float _positionX = Random.Range(transform.position.x - (maxRange / 2), transform.position.x + (maxRange / 2));

            Vector2 _position = new Vector2(_positionX, transform.position.y); 

            Instantiate(fallingStar, _position, fallingStar.transform.rotation);

            timer = 0; 
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(maxRange, 0f, 0f)); 
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolidShlurpingSaliva : MonoBehaviour
{
    [SerializeField] float fallSpeed;
    [SerializeField] GameObject childSaliva;
    [SerializeField] float yForce;
    [SerializeField] float xForce;
    [SerializeField] bool parent;
    
    Collider2D bottomTeethCollider;
    Rigidbody2D myBody;

    private void Start()
    {
        bottomTeethCollider = GameObject.Find("TeethBottom").GetComponent<Collider2D>();
        myBody = GetComponent<Rigidbody2D>();
        if(parent)
        {
            myBody.velocity = new Vector2(0, -fallSpeed);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == bottomTeethCollider)
        {
            if(parent)
            {
                for (int i = 0; i < 3; i++)
                {
                    GameObject tempSaliva = Instantiate(childSaliva, transform);
                    tempSaliva.transform.parent = FindObjectOfType<CombatMove>().transform;
                    if(i == 0)
                    {
                        tempSaliva.GetComponent<Rigidbody2D>().AddForce(new Vector2(-xForce, yForce), ForceMode2D.Impulse);
                    }
                    else if(i == 1)
                    {
                        tempSaliva.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, yForce), ForceMode2D.Impulse);
                    }
                    else if(i == 2)
                    {
                        tempSaliva.GetComponent<Rigidbody2D>().AddForce(new Vector2(xForce, yForce), ForceMode2D.Impulse);
                    }
                }
            }   

            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FGProjectile : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] GameObject successParticle;

    CombatMove minigame; 

    private void Start()
    {
        minigame = FindObjectOfType<CombatMove>(); 
    }

    private void Update()
    {
        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "LDGNoInteraction")
        {
            Instantiate(successParticle, collision.transform.position, Quaternion.identity, minigame.transform); 
            Destroy(collision.gameObject);
            minigame.Score++;
            minigame.CheckSuccess(); 
            Debug.Log(minigame.Score);
        }
        else if (collision.tag == "Player")
        {
            Destroy(gameObject); 
        }
    }
}

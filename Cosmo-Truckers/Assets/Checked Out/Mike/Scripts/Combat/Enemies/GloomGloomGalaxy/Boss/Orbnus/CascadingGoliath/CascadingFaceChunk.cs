using UnityEngine;

public class CascadingFaceChunk : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GameObject particle = transform.GetChild(0).gameObject; 
            particle.SetActive(true);
            particle.transform.parent = FindObjectOfType<CombatMove>().transform;
            
            Destroy(gameObject); 
        }
    }
}

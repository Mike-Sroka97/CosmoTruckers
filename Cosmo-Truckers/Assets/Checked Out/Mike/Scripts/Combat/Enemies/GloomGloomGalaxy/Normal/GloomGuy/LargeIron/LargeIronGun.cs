using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargeIronGun : MonoBehaviour
{
    [SerializeField] bool isPlayerGun = false;
    [SerializeField] Sprite damagedSprite;
    SpriteRenderer gunRenderer;

    // Start is called before the first frame update
    void Start()
    {
        gunRenderer = transform.parent.GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        LargeIronBullet bullet = collision.GetComponent<LargeIronBullet>(); 

        if (bullet != null)
        {
            if (!isPlayerGun)
            {
                CombatMove minigame = FindObjectOfType<CombatMove>(); 

                if (minigame.Score > 0)
                {
                    gunRenderer.sprite = damagedSprite;
                    FindObjectOfType<LargeIronClock>().GloomGuySad(); 
                    Destroy(collision.gameObject); 
                }
                else
                {
                    Player player = FindObjectOfType<PlayerBody>().Body;
                    player.TakeDamage();
                }
            }
            else
            {
                Player player = FindObjectOfType<PlayerBody>().Body;
                player.TakeDamage();
                gunRenderer.sprite = damagedSprite;
                Destroy(collision.gameObject);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class LargeIronGun : MonoBehaviour
{
    [SerializeField] bool isPlayerGun = false;
    [SerializeField] Sprite damagedSprite;
    [SerializeField] GameObject gunBreakParticles;
    [SerializeField] Transform barrel;
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
                    DamagePlayerGun(); 
                }
            }
            else
            {
                Player player = FindObjectOfType<PlayerBody>().Body;
                player.TakeDamage();
                gunRenderer.sprite = damagedSprite;
                Destroy(collision.gameObject);
            }

            Instantiate(gunBreakParticles, barrel.position, barrel.rotation, FindObjectOfType<CombatMove>().transform);
        }
    }

    public void DamagePlayerGun()
    {
        gunRenderer.sprite = damagedSprite;
        Player player = FindObjectOfType<PlayerBody>().Body;
        player.TakeDamage();
    }
}

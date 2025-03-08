using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargeIronTrigger : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    [SerializeField] Transform barrel;
    [SerializeField] GameObject smokeParticles; 
    [SerializeField] GameObject gunBreakParticles; 
    [SerializeField] SpriteRenderer gunSpriteRender;
    [SerializeField] Sprite firedSprite;
    [SerializeField] Material toggledMaterial;
    [SerializeField] LargeIronGun playerGun; 

    CombatMove minigame; 
    LargeIronClock clock;
    Collider2D myCollider;
    SpriteRenderer myRenderer; 

    private void Start()
    {
        clock = FindObjectOfType<LargeIronClock>();
        myCollider = GetComponent<Collider2D>();
        minigame = FindObjectOfType<CombatMove>();
        myRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerAttack") && !clock.PlayerFired)
        {
            clock.Fire();
            clock.PlayerFired = true;

            if (!clock.TooEarly())
            {
                myCollider.enabled = false;
                Instantiate(bullet, barrel.position, barrel.rotation, minigame.transform);
                Instantiate(smokeParticles, barrel.position, barrel.rotation, minigame.transform);
                gunSpriteRender.sprite = firedSprite;
            }
            else
            {
                Instantiate(gunBreakParticles, barrel.position, barrel.rotation, minigame.transform);
                playerGun.DamagePlayerGun(); 
            }

            myRenderer.material = toggledMaterial;
            transform.localEulerAngles = new Vector3(0f, 0f, -35f);
            transform.localPosition = new Vector3(transform.localPosition.x, -0.325f, transform.localPosition.z);
        }
    }
}

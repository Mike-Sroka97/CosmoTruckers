using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargeIronTrigger : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    [SerializeField] Transform barrel;
    [SerializeField] GameObject smokeParticles; 
    [SerializeField] SpriteRenderer gunSpriteRender;
    [SerializeField] Sprite firedSprite;

    CombatMove minigame; 
    LargeIronClock clock;
    Collider2D myCollider;

    private void Start()
    {
        clock = FindObjectOfType<LargeIronClock>();
        myCollider = GetComponent<Collider2D>();
        minigame = FindObjectOfType<CombatMove>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerAttack" && !clock.PlayerFired)
        {
            clock.Fire();
            clock.PlayerFired = true;
            myCollider.enabled = false;
            GameObject bulletTemp = Instantiate(bullet, barrel.position, barrel.rotation, minigame.transform);
            GameObject spokeParticle = Instantiate(smokeParticles, barrel.position, barrel.rotation, minigame.transform);
            gunSpriteRender.sprite = firedSprite;
            transform.localEulerAngles = new Vector3(0f, 0f, -35f);
            transform.localPosition = new Vector3(transform.localPosition.x, -0.325f, transform.localPosition.z);
        }
    }
}

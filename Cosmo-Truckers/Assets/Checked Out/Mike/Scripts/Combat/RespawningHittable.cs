using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawningHittable : BaseHittable
{
    [SerializeField] float respawnDelay;
    [SerializeField] Color deadColor;

    Collider2D[] myColliders;
    SpriteRenderer[] myRenderers;

    private void Start()
    {
        myColliders = GetComponentsInChildren<Collider2D>();
        myRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

    protected override void ReceiveHit()
    {
        HandleDeath(false);
        StartCoroutine(Respawn());
    }

    private void HandleDeath(bool respawn)
    {
        foreach (Collider2D collider in myColliders)
            collider.enabled = respawn;
        foreach (SpriteRenderer renderer in myRenderers)
            renderer.color = respawn ? Color.white : deadColor;
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnDelay);

        HandleDeath(true);
    }
}

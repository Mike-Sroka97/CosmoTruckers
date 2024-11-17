using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceToInaTransparency : MonoBehaviour
{
    [SerializeField] float distanceToStartFading = .05f;
    [SerializeField] float fadeSpeed = .05f;
    DungeonCharacter ina;
    SpriteRenderer myRenderer;

    private void Start()
    {
        ina = FindObjectOfType<DungeonCharacter>();
        myRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        HandleTransparency();
    }

    private void HandleTransparency()
    {
        if (Vector3.Distance(transform.position, ina.transform.position) <= distanceToStartFading)
            StartCoroutine(StartFade());
    }

    IEnumerator StartFade()
    {
        while(myRenderer.color.a > 0)
        {
            myRenderer.color = new Color(myRenderer.color.r, myRenderer.color.g, myRenderer.color.b, myRenderer.color.a - fadeSpeed * Time.deltaTime);
            yield return null;
        }

        Destroy(this);
    }
}

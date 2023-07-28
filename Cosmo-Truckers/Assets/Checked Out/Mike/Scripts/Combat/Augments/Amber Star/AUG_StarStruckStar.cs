using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AUG_StarStruckStar : MonoBehaviour
{
    [SerializeField] GameObject starExplosion;
    [SerializeField] float minY = -5.3f;

    bool dying = false;

    private void Update()
    {
        TrackYPosition();
    }

    private void TrackYPosition()
    {
        if (transform.position.y <= minY && !dying)
        {
            dying = true;
            GameObject star = Instantiate(starExplosion, transform);
            star.transform.parent = null;
            GetComponentInChildren<SpriteRenderer>().enabled = false;
            GetComponent<MoveForward>().enabled = false;
            Invoke("DestroyMe", 1f);
        }
    }

    private void DestroyMe()
    {
        Destroy(transform.parent.gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportBattery : MonoBehaviour
{
    [SerializeField] bool rechargeTeleport = true;
    [SerializeField] bool destroyMe = false;
    [SerializeField] float disabledDuration;
    [SerializeField] Color disabledColor;

    [SerializeField] float floatSpeed;
    [SerializeField] float clamp;

    [SerializeField] GameObject regenParticle; 

    ProtoINA proto;
    CombatMove minigame;
    bool movingUp;
    Vector3 startingPosition;
    Collider2D myCollider;
    SpriteRenderer myRenderer;
    Color startColor;

    private void Start()
    {
        myCollider = GetComponent<Collider2D>();
        myRenderer = GetComponent<SpriteRenderer>();
        minigame = FindObjectOfType<CombatMove>();
        startColor = myRenderer.color;
        startingPosition = transform.localPosition;

        int random = UnityEngine.Random.Range(0, 2);

        if(random == 1)
        {
            movingUp = true;
        }
        else
        {
            movingUp = false;
        }
    }

    private void Update()
    {
        MoveMe();
    }

    private void MoveMe()
    {
        if(movingUp)
        {
            transform.localPosition += new Vector3(0, floatSpeed * Time.deltaTime, 0);

            if(transform.localPosition.y > startingPosition.y + clamp)
            {
                movingUp = false;
            }
        }
        else
        {
            transform.localPosition -= new Vector3(0, floatSpeed * Time.deltaTime, 0);

            if (transform.localPosition.y < startingPosition.y - clamp)
            {
                movingUp = true;
            }
        }
    }

    IEnumerator DisableMe()
    {
        GameObject particle = Instantiate(regenParticle, transform.position, Quaternion.identity, minigame.transform);
        myCollider.enabled = false;
        myRenderer.color = disabledColor;

        yield return new WaitForSeconds(disabledDuration);

        myCollider.enabled = true;
        myRenderer.color = startColor;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if(proto == null)
                proto = FindObjectOfType<ProtoINA>();

            proto.SetCanTeleport(rechargeTeleport);
            if(destroyMe)
            {
                Destroy(gameObject);
            }
            else
            {
                StartCoroutine(DisableMe());
            }
        }
    }
}

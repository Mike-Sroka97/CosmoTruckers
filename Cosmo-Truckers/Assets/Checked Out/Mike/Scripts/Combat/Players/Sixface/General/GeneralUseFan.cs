using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralUseFan : MonoBehaviour
{
    [SerializeField] float blowForce;
    [SerializeField] float hoverBlowForce;

    SixfaceINA six;
    PlayerBody sixFace;
    Rigidbody2D sixFaceBody;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(!six)
            if (FindObjectOfType<SixfaceINA>())
            {
                six = FindObjectOfType<SixfaceINA>();
                sixFace = six.GetComponentInChildren<PlayerBody>();
                sixFaceBody = six.GetComponent<Rigidbody2D>();
            }

        if (sixFace != null && collision.gameObject == sixFace.gameObject)
        {
            if(six.IsHovering && !six.damaged)
            {
                sixFaceBody.velocity = new Vector2(sixFaceBody.velocity.x, sixFaceBody.velocity.y + hoverBlowForce);
            }
            else if(six.damaged)
            {
                sixFaceBody.velocity = new Vector2(sixFaceBody.velocity.x, blowForce);
            }
            else
            {
                sixFaceBody.velocity = new Vector2(sixFaceBody.velocity.x, sixFaceBody.velocity.y + blowForce);
            }
        }
    }
}

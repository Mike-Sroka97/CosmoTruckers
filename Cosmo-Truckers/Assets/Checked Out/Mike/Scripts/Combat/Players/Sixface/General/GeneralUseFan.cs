using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralUseFan : MonoBehaviour
{
    [SerializeField] float blowForce;
    [SerializeField] float hoverBlowForce;

    SixfaceINA sixFace;
    Rigidbody2D sixFaceBody;
    private void Start()
    {
        if(FindObjectOfType<SixfaceINA>())
        {
            sixFace = FindObjectOfType<SixfaceINA>();
            sixFaceBody = sixFace.GetComponent<Rigidbody2D>();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (sixFace != null && collision.gameObject == sixFace.gameObject)
        {
            if(sixFace.IsHovering)
            {
                sixFaceBody.velocity = new Vector2(sixFaceBody.velocity.x, sixFaceBody.velocity.y + hoverBlowForce);
            }
            else
            {
                sixFaceBody.velocity = new Vector2(sixFaceBody.velocity.x, sixFaceBody.velocity.y + blowForce);
            }
        }
    }
}

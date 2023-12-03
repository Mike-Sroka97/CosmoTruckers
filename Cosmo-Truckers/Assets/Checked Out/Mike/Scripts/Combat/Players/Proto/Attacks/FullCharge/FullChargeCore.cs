using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullChargeCore : MonoBehaviour
{
    [SerializeField] float followSpeed;
    [SerializeField] float maxDistance;
    [SerializeField] FullChargeNode[] fullChargeNodes;

    ProtoINA proto;

    [HideInInspector] public bool Following;

    private void Update()
    {
        Follow();
    }

    private void Follow()
    {
        if (!Following)
            return;

        if(proto == null)
            proto = FindObjectOfType<ProtoINA>();

        if (Vector2.Distance(transform.position, proto.transform.position) > maxDistance || proto.IsTeleporting)
        {
            Following = false;
        }

        transform.position = Vector2.MoveTowards(transform.position, proto.transform.position, followSpeed * Time.deltaTime);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "PlayerAttack" && !Following)
        {
            foreach(FullChargeNode node in fullChargeNodes)
            {
                node.SnapCD();
            }
            Following = true;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventNodeHandler : MonoBehaviour
{
    [SerializeField] RectTransform swish;
    [SerializeField] RectTransform swosh;
    [SerializeField] float swishSpeed;

    public int Player = -1;
    DNode currentNode;
    Vector3 swishStartPos;
    Vector3 swoshStartPos;
    EventNodeBase nodeData;

    private void Start()
    {
        swishStartPos = swish.localPosition;
        swoshStartPos = swosh.localPosition;
    }

    public IEnumerator Move(bool intoCamera, DNode node = null)
    {
        if(Player == -1)
            Player = Random.Range(0, 4);

        if (node)
        {
            currentNode = node;
            currentNode.Active = false;
        }

        Vector3 swishGoal;
        Vector3 swoshGoal;

        if(intoCamera)
        {
            nodeData = Instantiate(currentNode.NodeData.GetComponent<DungeonEventNode>().EventToGenerate, swosh).GetComponent<EventNodeBase>();

            swishGoal = new Vector3(0, swishStartPos.y, swishStartPos.z);
            swoshGoal = new Vector3(0, swoshStartPos.y, swoshStartPos.z);

            while(swish.localPosition != swishGoal)
            {
                swish.localPosition = Vector3.MoveTowards(swish.localPosition, swishGoal, swishSpeed * Time.deltaTime);
                yield return null;
            }

            while (swosh.localPosition != swoshGoal)
            {
                swosh.localPosition = Vector3.MoveTowards(swosh.localPosition, swoshGoal, swishSpeed * Time.deltaTime);
                yield return null;
            }

            nodeData.Initialize(this);
        }
        else
        {
            swishGoal = swishStartPos;
            swoshGoal = swoshStartPos;

            while (swosh.localPosition != swoshGoal)
            {
                swosh.localPosition = Vector3.MoveTowards(swosh.localPosition, swoshGoal, swishSpeed * Time.deltaTime);
                yield return null;
            }

            while (swish.localPosition != swishGoal)
            {
                swish.localPosition = Vector3.MoveTowards(swish.localPosition, swishGoal, swishSpeed * Time.deltaTime);
                yield return null;
            }

            currentNode.Active = true;
            currentNode.EventFinished = true;
            currentNode.SetupLineRendererers();

            Destroy(nodeData.gameObject);
        }
    }
}

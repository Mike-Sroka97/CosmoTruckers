using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventNodeHandler : MonoBehaviour
{
    [SerializeField] RectTransform swish;
    [SerializeField] RectTransform swosh;
    [SerializeField] float swishSpeed;

    Vector3 swishStartPos;
    Vector3 swoshStartPos;

    private void Start()
    {
        swishStartPos = swish.localPosition;
        swoshStartPos = swosh.localPosition;
    }

    public IEnumerator Move(bool intoCamera, DNode node)
    {
        node.Active = false;

        Vector3 swishGoal;
        Vector3 swoshGoal;

        if(intoCamera)
        {
            Instantiate(node.NodeData.GetComponent<DungeonEventNode>().EventToGenerate, swosh);

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

            //TODO enable event
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

            node.Active = true;
            node.EventFinished = true;
            node.SetupLineRendererers();
        }
    }
}

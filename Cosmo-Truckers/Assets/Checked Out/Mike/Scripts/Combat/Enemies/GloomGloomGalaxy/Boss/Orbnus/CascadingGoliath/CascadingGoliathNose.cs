using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CascadingGoliathNose : MonoBehaviour
{
    [SerializeField] float minY;
    [SerializeField] float fallSpeed;
    [SerializeField] float endCallWait = 3f;

    CombatMove minigame;
    bool endCalled = false;
    private void Start()
    {
        minigame = FindObjectOfType<CombatMove>();
    }

    private void Update()
    {
        if (transform.localPosition.y <= minY)
        {
            if(!endCalled)
            {
                endCalled = true;
                Invoke("EndBossMove", endCallWait);
            }
            return;
        }

        MoveMe();
    }

    private void MoveMe()
    {
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);

        if(transform.localPosition.y <= minY)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, minY, transform.localPosition.z);
        }
    }

    private void EndBossMove()
    {
        minigame.FightWon = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeGougerHand : MonoBehaviour
{
    [SerializeField] float revealDuration;
    [SerializeField] float revealSpeed;
    [SerializeField] float attackDelay;
    [SerializeField] float attackDuration;
    [SerializeField] float attackSpeed;
    [SerializeField] float holdDuration;
    [SerializeField] float retractSpeed;

    [HideInInspector] public bool Active { get; private set; }

    MoveForward myMoveForward;
    Vector3 startPos;

    private void Start()
    {
        myMoveForward = GetComponent<MoveForward>();
        startPos = transform.position;
    }

    public void ActivateMe()
    {
        Active = true;
        StartCoroutine(MoveMe());
    }

    private IEnumerator MoveMe()
    {
        myMoveForward.MoveSpeed = revealSpeed;
        yield return new WaitForSeconds(revealDuration);

        myMoveForward.MoveSpeed = 0;
        yield return new WaitForSeconds(attackDelay);

        myMoveForward.MoveSpeed = attackSpeed;
        yield return new WaitForSeconds(attackDuration);

        myMoveForward.MoveSpeed = 0;
        yield return new WaitForSeconds(holdDuration);

        while(transform.position != startPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, startPos, retractSpeed * Time.deltaTime);
            yield return null;
        }

        Active = false;
    }
}

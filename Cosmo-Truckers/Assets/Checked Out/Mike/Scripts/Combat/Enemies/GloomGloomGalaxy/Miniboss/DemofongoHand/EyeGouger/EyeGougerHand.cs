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
        startPos = transform.localPosition;
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

        while(transform.localPosition != startPos)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, startPos, retractSpeed * Time.deltaTime);
            yield return null;
        }

        Active = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunOfTheMawHead : MonoBehaviour
{
    [SerializeField] float dashDelay;
    [SerializeField] float dashSpeed;
    [SerializeField] AnimationClip idle, attack; 

    Animator myAnimator; 
    Player player;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        myAnimator = GetComponent<Animator>();

        StartCoroutine(Dash());
    }

    IEnumerator Dash()
    {
        myAnimator.Play(idle.name); 

        yield return new WaitForSeconds(dashDelay);

        Vector3 target = player.transform.position;

        //Flip depending on direction of player
        if (target.x > transform.position.x)
        {
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }
        else
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }


        myAnimator.Play(attack.name);

        while(transform.position != target)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, dashSpeed * Time.deltaTime);

            yield return null;
        }

        StartCoroutine(Dash());
    }
}

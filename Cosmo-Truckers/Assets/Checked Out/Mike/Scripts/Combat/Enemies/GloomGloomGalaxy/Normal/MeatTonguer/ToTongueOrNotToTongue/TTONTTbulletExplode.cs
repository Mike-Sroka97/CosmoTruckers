using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class TTONTTbulletExplode : MonoBehaviour
{
    [SerializeField] float speedReductionSpeedModifier;
    [SerializeField] float childMaxScale;
    [SerializeField] float growSpeed;
    [SerializeField] bool explode = true;
    [SerializeField] GameObject nonExplodingBullet;
    [SerializeField] GameObject burstParticle; 

    MoveForward myBullet;
    Player player;
    const int degreeIncrease = 60;
    const int numberOfChildren = 6;
    Transform child;

    private void Start()
    {
        child = GetComponentsInChildren<Transform>()[1];
        myBullet = GetComponentInChildren<MoveForward>();
        player = FindObjectOfType<Player>();
        if(explode)
        {
            speedReductionSpeedModifier = Vector3.Distance(transform.position, player.transform.position) / speedReductionSpeedModifier; //mod is higher the further the player is
        }
    }

    private void Update()
    {
        TrackBullet();
    }

    private void TrackBullet()
    {
        if(myBullet.MoveSpeed > 0)
        {
            myBullet.MoveSpeed -= speedReductionSpeedModifier * Time.deltaTime;
        }
        else if(myBullet.transform.localScale.x < childMaxScale && explode)
        {
            myBullet.MoveSpeed = 0;
            myBullet.transform.localScale += new Vector3(growSpeed * Time.deltaTime, growSpeed * Time.deltaTime, growSpeed * Time.deltaTime);
        }
        else if(explode)
        {
            for(int i = 0; i < numberOfChildren; i++)
            {
                GameObject tempBullet = Instantiate(nonExplodingBullet, child.position, child.rotation, FindObjectOfType<CombatMove>().transform);
                Instantiate(burstParticle, child.position, child.rotation, FindObjectOfType<CombatMove>().transform);
                tempBullet.transform.Rotate(new Vector3(0, 0, degreeIncrease * i));
            }
            Destroy(gameObject);
        }
        else
        {
            myBullet.MoveSpeed = 0;
        }
    }
}

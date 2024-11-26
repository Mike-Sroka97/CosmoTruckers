using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    [SerializeField] bool destroyParent = true;
    [SerializeField] bool destroyOnContact = true;
    [SerializeField] protected bool checkClamps = true;
    [SerializeField] bool moveHorizontal = true; 
    [SerializeField] ParticleSystem particleTrail; 
    [SerializeField] GameObject destroyParticle; 
    private ParticleSystem.MainModule mainModule;

    public float MoveSpeed;

    const int xClamp = 10;
    const int yClamp = 8;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && destroyOnContact)
        {
            if (particleTrail != null)
            {
                particleTrail.transform.parent = FindObjectOfType<CombatMove>().transform;
                mainModule = particleTrail.main; 

                //Stop it from looping and set it to destroy itself when it's done
                mainModule.loop = false;
                mainModule.stopAction = ParticleSystemStopAction.Destroy; 
            }

            if (destroyParticle != null) 
            {
                Instantiate(destroyParticle, transform.position, Quaternion.identity, FindObjectOfType<CombatMove>().transform);
            }

            if(destroyParent)
                Destroy(transform.parent.gameObject);
            else
                Destroy(gameObject);
        }
    }

    private void Update()
    {
        MoveMe();
    }

    private void MoveMe()
    {
        if (moveHorizontal)
            transform.Translate(Vector3.right * MoveSpeed * Time.deltaTime);
        else
            transform.Translate(Vector3.up * MoveSpeed * Time.deltaTime);

        if (!checkClamps)
            return;

        if(transform.localPosition.y > yClamp 
            || transform.localPosition.y < -yClamp 
            || transform.localPosition.x > xClamp 
            || transform.localPosition.x < -xClamp)
        {
            if(destroyParent)
                Destroy(transform.parent.gameObject);
            Destroy(gameObject);
        }
    }

    public void ToggleClamps()
    {
        checkClamps = true;
    }
}

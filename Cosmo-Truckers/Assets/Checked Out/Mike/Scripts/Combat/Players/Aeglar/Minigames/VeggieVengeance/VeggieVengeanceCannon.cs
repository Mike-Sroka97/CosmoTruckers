using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VeggieVengeanceCannon : MonoBehaviour
{
    [SerializeField] float rotateSpeed;
    [SerializeField] float minRotation;
    [SerializeField] float maxRotation;

    [SerializeField] float shootCD;
    [SerializeField] GameObject projectile;
    [SerializeField] Transform barrel;

    AeglarINA aeglar;
    Rigidbody2D aeglarBody;
    float currentTime;
    bool trackTime;

    private void Start()
    {
        currentTime = shootCD;
        aeglar = FindObjectOfType<AeglarINA>();
        aeglarBody = aeglar.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        TrackTime();
        if(!aeglar.GetDamaged())
        {
            TrackAeglarDash();
            TrackAeglarRotation();
        }
    }

    private void TrackTime()
    {
        if(trackTime)
        {
            currentTime += Time.deltaTime;
            if(currentTime >= shootCD && !aeglar.GetDashState())
            {
                trackTime = false;
            }
        }
    }

    private void TrackAeglarDash()
    {
        if(!aeglar.GetDashState() && currentTime >= shootCD)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        currentTime = 0;
        trackTime = true;
        Instantiate(projectile, barrel.position, transform.rotation);
    }

    private void TrackAeglarRotation()
    {
        if(aeglarBody.velocity.x < 0)
        {
            if(transform.eulerAngles.z < minRotation && transform.eulerAngles.z > maxRotation / 2)
            {
                transform.Rotate(new Vector3(0, 0, rotateSpeed * Time.deltaTime));

            }
        }
        else if(aeglarBody.velocity.x > 0)
        {
            if (transform.eulerAngles.z > maxRotation)
            {
                transform.Rotate(new Vector3(0, 0, -rotateSpeed * Time.deltaTime));
            }
        }

        if (transform.eulerAngles.z > minRotation)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, minRotation);
        }
        else if(transform.eulerAngles.z < maxRotation)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, maxRotation);
        }
    }
}

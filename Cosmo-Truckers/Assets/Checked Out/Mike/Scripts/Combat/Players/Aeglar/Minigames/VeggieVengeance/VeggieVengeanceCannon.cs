using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VeggieVengeanceCannon : MonoBehaviour
{
    [SerializeField] float rotateSpeed;
    [SerializeField] float minRotation;
    [SerializeField] float maxRotation;

    [SerializeField] GameObject projectile;
    [SerializeField] Transform barrel;
    [SerializeField] SpriteRenderer cannonSpriteRenderer;
    [SerializeField] AnimationClip cannonShootAnimation;
    [SerializeField] Material cannonHurtMaterial, cannonToggledMaterial; 

    AeglarINA aeglar;
    Rigidbody2D aeglarBody;
    Animator cannonAnimator;
    Material cannonStartMaterial;
    bool canPlay = true; 
    bool canFire = true; 
    public bool CalculateMove = false;

    public void StartMove()
    {
        aeglar = FindObjectOfType<AeglarINA>();
        aeglarBody = aeglar.GetComponent<Rigidbody2D>();
        CalculateMove = true;
        cannonAnimator = GetComponentInChildren<Animator>();
        cannonStartMaterial = cannonSpriteRenderer.material;
    }

    private void Update()
    {
        if (!CalculateMove)
            return;

        SetCanPlay(); 
        SetCannonMaterials();

        if (canPlay)
        {
            TrackAeglarDash();
            TrackAeglarRotation();
        }
    }

    private void TrackAeglarDash()
    {
        if (!aeglar.GetDashState() && canFire)
        {
              Shoot();
        }
    }

    private void SetCanPlay()
    {
        if (!aeglar.damaged)
        {
            canPlay = true;
        }
        else
        {
            canPlay = false; 
        }
    }

    private void SetCannonMaterials()
    {
        if (canPlay && !aeglar.GetIFramesState())
        {
            if (canFire && aeglar.GetDashState())
            {
                cannonSpriteRenderer.material = cannonStartMaterial;
            }
            else
            {
                cannonSpriteRenderer.material = cannonToggledMaterial; 
            }
        }
        else if (aeglar.GetIFramesState())
        {
            cannonSpriteRenderer.material = cannonHurtMaterial; 
        } 
    }

    private void Shoot()
    {
        canFire = false; 
        Instantiate(projectile, barrel.position, transform.rotation, FindObjectOfType<CombatMove>().transform);
        StartCoroutine(CannonAnimation());
    }

    private IEnumerator CannonAnimation()
    {
        cannonAnimator.Play(cannonShootAnimation.name);
        yield return new WaitForSeconds(cannonShootAnimation.length);

        //By the end of this, the player should be able to fire again
        canFire = true; 
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

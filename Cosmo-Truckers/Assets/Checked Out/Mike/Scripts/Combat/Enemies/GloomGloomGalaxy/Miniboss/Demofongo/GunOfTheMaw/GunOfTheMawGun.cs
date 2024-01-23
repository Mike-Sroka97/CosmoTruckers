using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunOfTheMawGun : MonoBehaviour
{
    [SerializeField] float minScale;
    [SerializeField] float maxScale;
    [SerializeField] float fadeInSpeed;
    [SerializeField] Transform[] spawns;
    [SerializeField] float rotatorSpeed;
    [SerializeField] float spinDelay;
    [SerializeField] float fireDelay;
    [SerializeField] float animationDelay, startSpinDelay; 
    [SerializeField] Transform barrel;
    [SerializeField] GameObject bullet;
    [SerializeField] AnimationClip shoot; 

    [HideInInspector] public int CurrentBlackListedSpawn = 8;

    Animator myAnimator; 
    Rotator myRotator;
    SpriteRenderer myRenderer;
    int lastRandom = -1;

    private void Start()
    {
        myRotator = GetComponent<Rotator>();
        myRenderer = GetComponent<SpriteRenderer>();
        myAnimator = GetComponent<Animator>();

        DeterminePosition();
    }

    private void DeterminePosition()
    {
        int random = Random.Range(0, spawns.Length);

        while(random == CurrentBlackListedSpawn - 1 || random == lastRandom)
        {
            random = Random.Range(0, spawns.Length);
        }

        lastRandom = random;

        transform.position = spawns[random].position;

        StartCoroutine(SpinMe());
    }

    IEnumerator SpinMe()
    {
        transform.localScale = new Vector3(minScale, minScale, minScale);
        myRenderer.color = new Color(myRenderer.color.r, myRenderer.color.g, myRenderer.color.b, 0);
        myRotator.RotateSpeed = rotatorSpeed;

        while(transform.localScale.x < maxScale)
        {
            transform.localScale += (fadeInSpeed * Time.deltaTime) * new Vector3(1, 1, 1);
            myRenderer.color = new Color(myRenderer.color.r, myRenderer.color.g, myRenderer.color.b, transform.localScale.x / maxScale); //this makes the fade a ratio of the scale completion
            yield return null;
        }

        transform.localScale = new Vector3(maxScale, maxScale, maxScale);
        myRotator.RotateSpeed = 0;

        yield return new WaitForSeconds(fireDelay);

        myAnimator.Play(shoot.name);

        yield return new WaitForSeconds(animationDelay);

        GameObject newBullet = Instantiate(bullet, barrel);
        newBullet.transform.parent = transform.parent;

        yield return new WaitForSeconds(startSpinDelay);

        myRotator.RotateSpeed = -rotatorSpeed;

        while (transform.localScale.x > minScale)
        {
            transform.localScale -= (fadeInSpeed * Time.deltaTime) * new Vector3(1, 1, 1);
            myRenderer.color = new Color(myRenderer.color.r, myRenderer.color.g, myRenderer.color.b, transform.localScale.x / maxScale); //this makes the fade a ratio of the scale completion
            yield return null;
        }

        transform.localScale = Vector3.zero;

        yield return new WaitForSeconds(spinDelay);

        DeterminePosition();
    }
}

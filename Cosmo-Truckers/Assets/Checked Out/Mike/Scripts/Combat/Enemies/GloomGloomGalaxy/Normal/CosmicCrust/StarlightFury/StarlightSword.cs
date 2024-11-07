using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarlightSword : MonoBehaviour
{
    [SerializeField] float spinSpeed;
    [SerializeField] float spinTime;
    [SerializeField] float rotateTowardsSpeed;
    [SerializeField] float rotateTowardsTime;
    [SerializeField] float chargeSpeed;
    [HideInInspector] public bool Activated;

    [SerializeField] GameObject[] particleEffects; 

    float xClamp = 7.5f;
    float yClamp = 5.5f;

    StarlightFury minigame;
    GameObject target;
    bool moving;

    public void Initialize()
    {
        target = FindObjectOfType<Player>().gameObject;
        minigame = FindObjectOfType<StarlightFury>();

        if (particleEffects != null)
            SetParticleStates(false);
    }

    private void Update()
    {
        MoveMe();
    }

    private void MoveMe()
    {
        if (!moving)
            return;

        transform.position += transform.right * chargeSpeed * Time.deltaTime;

        if (ClampCheck())
        {
            moving = false;
            minigame.Score++;
        }
    }

    public IEnumerator CCSword()
    {
        Activated = true;

        float currentTime = 0;

        while (currentTime < spinTime)
        {
            transform.Rotate(new Vector3(0, 0, spinSpeed * Time.deltaTime));
            currentTime += Time.deltaTime;
            yield return null;
        }

        currentTime = 0;
        while (currentTime <= rotateTowardsTime)
        {
            float angle = Mathf.Atan2(target.transform.localPosition.y - transform.localPosition.y, target.transform.localPosition.x - transform.localPosition.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateTowardsSpeed * Time.deltaTime);
            currentTime += Time.deltaTime;
            yield return null;
        }

        moving = true;

        if (particleEffects != null)
            SetParticleStates(true);
    }

    private bool ClampCheck()
    {
        if (transform.localPosition.x >= xClamp
            || transform.localPosition.x <= -xClamp
            || transform.localPosition.y >= yClamp
            || transform.localPosition.y <= -yClamp)
        {
            return true;
        }
        return false;
    }

    private void SetParticleStates(bool state)
    {
        foreach (GameObject _object in particleEffects)
        {
            _object.SetActive(state);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class COKO_Rotator : MonoBehaviour
{
    //Random ranges for speed
    [SerializeField] float minStartSpeed;
    [SerializeField] float maxStartSpeed;
    [SerializeField] float minMaxSpeed;
    [SerializeField] float maxMaxSpeed;
    [SerializeField] float minAcceleration;
    [SerializeField] float maxAcceleration;
    //Times
    [SerializeField] float preGameRotation = 3f;
    [SerializeField] float maxGameTime;
    //Sprite
    [SerializeField] SpriteRenderer PC;
    [SerializeField] Sprite happyPC;
    [SerializeField] Sprite sadPC;
    //Success Calculation
    [SerializeField] GameObject target;
    //Combat
    [SerializeField] GameObject sprite;
    [SerializeField] int maxAttacks = 3;
    //Timer
    [SerializeField] TextMeshProUGUI timer;

    //Determined ranges for speed
    float currentSpeed;
    float maxSpeed;
    float acceleration;

    //Tracks pre-game rotation
    float currentTime = 0;

    //tells the game to stop tracking time 
    bool done = false;
    
    //Used for damage
    int currentAttack = 0;
    float success = 0;

    private void Start()
    {
        //Assigns values to random elements of the attack
        currentSpeed = Random.Range(minStartSpeed, maxStartSpeed + 1);
        maxSpeed = Random.Range(minMaxSpeed, maxMaxSpeed + 1);
        acceleration = Random.Range(minAcceleration, maxAcceleration);

        //Puts the target somewhere random on the circle
        target.transform.position = Random.insideUnitCircle.normalized * 2.5f;
    }

    private void Update()
    {
        if(!done)
        {
            TrackTime();
            Accelerate();
            TargetColor();
        }
    }

    private void TargetColor()
    {
        if(currentTime > 0)
        {
            target.GetComponent<SpriteRenderer>().color = new Color(target.GetComponent<SpriteRenderer>().color.r, 1 - currentTime / maxGameTime, target.GetComponent<SpriteRenderer>().color.b, target.GetComponent<SpriteRenderer>().color.a);
        }
    }

    private void Accelerate()
    {
        //Accelerate
        if (currentSpeed < maxSpeed)
        {
            currentSpeed += acceleration;
            if (currentSpeed > maxSpeed)
            {
                currentSpeed = maxSpeed;
            }
        }
    }

    private void TrackTime()
    {
        //Track time
        if (currentTime >= maxGameTime)
        {
            //complete failure
            PC.sprite = sadPC;
            PC.GetComponent<Animator>().enabled = true;
        }
        else if (currentTime >= preGameRotation)
        {
            PC.color = new Color(PC.color.r, PC.color.g, PC.color.b, 1);
            //add player interaction
            if(Input.GetMouseButtonDown(0))
            {
                currentAttack++;
                Instantiate(sprite, PC.transform.position, PC.transform.rotation);
                if(currentAttack >= maxAttacks)
                {
                    PC.enabled = false;
                    done = true;
                }

                CalculateSuccess();
            }
            transform.Rotate(new Vector3(0, 0, Time.deltaTime * currentSpeed));
        }
        else
        {
            transform.Rotate(new Vector3(0, 0, Time.deltaTime * currentSpeed));
        }

        currentTime += Time.deltaTime;
        timer.text = ((int)(maxGameTime - currentTime)).ToString();
    }

    private void CalculateSuccess()
    {
        //Calculates distance to determine success

        float distance = Mathf.Sqrt(Mathf.Pow(PC.transform.position.x - target.transform.position.x, 2) + Mathf.Pow(PC.transform.position.y - target.transform.position.y, 2));

        //Sets score
        success += Score(success, distance);

        Debug.Log(success);
    }

    private float Score(float success, float distance)
    {
        if (distance <= 0.02f)
        {
            success = 10;
        }
        else if (distance <= 0.04f)
        {
            success = 9;
        }
        else if (distance <= 0.06f)
        {
            success = 8;
        }
        else if (distance <= 0.08f)
        {
            success = 7;
        }
        else if (distance <= 0.1f)
        {
            success = 6;
        }
        else if (distance <= 0.12f)
        {
            success = 5;
        }
        else if (distance <= 0.14f)
        {
            success = 4;
        }
        else if (distance <= 0.16f)
        {
            success = 3;
        }
        else if (distance <= 0.18f)
        {
            success = 2;
        }
        else if (distance <= 0.2f)
        {
            success = 1;
        }

        //Adjusts damage based on time
        success *= currentTime / 10;

        //Fuck floats
        if(success > 0)
        {
            success = (int)success;
        }

        return success;
    }
}

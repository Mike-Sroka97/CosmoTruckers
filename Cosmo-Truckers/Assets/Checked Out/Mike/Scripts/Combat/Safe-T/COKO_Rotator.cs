using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] Canvas targetCanvas;
    //Combat
    [SerializeField] GameObject sprite;
    [SerializeField] TextMeshProUGUI damageCounter;
    [SerializeField] int maxAttacks = 3;
    [SerializeField] float[] successDistance;
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

    private void Awake()
    {
        //Puts the target somewhere random on the circle
        float randomRotation = Random.Range(0, 361);
        targetCanvas.transform.Rotate(new Vector3(0, 0, randomRotation));
        //target.transform.position = Random.insideUnitCircle.normalized * 2.5f;
    }

    private void Start()
    {
        //Assigns values to random elements of the attack
        currentSpeed = Random.Range(minStartSpeed, maxStartSpeed + 1);
        maxSpeed = Random.Range(minMaxSpeed, maxMaxSpeed + 1);
        acceleration = Random.Range(minAcceleration, maxAcceleration);
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
            target.GetComponentInChildren<Image>().color = new Color(target.GetComponent<SpriteRenderer>().color.r, 1 - currentTime / maxGameTime, target.GetComponent<SpriteRenderer>().color.b, target.GetComponent<SpriteRenderer>().color.a);
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
        Debug.Log("Distance = " + distance);

        //Sets score
        success += Score(success, distance);

        if(currentAttack == 3)
        {
            damageCounter.text = success.ToString();
            damageCounter.color = new Color(1, 1 - success / 30, 1 - success / 30); //30 is max damage
            Debug.Log("Total attack value: " + success);
        }
    }

    private float Score(float success, float distance)
    {
        success = 0;

        if (distance <= successDistance[0])
        {
            success = 10;
        }
        else if (distance <= successDistance[1])
        {
            success = 9;
        }
        else if (distance <= successDistance[2])
        {
            success = 8;
        }
        else if (distance <= successDistance[3])
        {
            success = 7;
        }
        else if (distance <= successDistance[4])
        {
            success = 6;
        }
        else if (distance <= successDistance[5])
        {
            success = 5;
        }
        else if (distance <= successDistance[6])
        {
            success = 4;
        }
        else if (distance <= successDistance[7])
        {
            success = 3;
        }
        else if (distance <= successDistance[8])
        {
            success = 2;
        }
        else if (distance <= successDistance[9])
        {
            success = 1;
        }

        //Adjusts damage based on time
        //success *= currentTime / 10;

        //Fuck floats
        if(success > 0)
        {
            success = (int)success;
        }

        Debug.Log("Current attack value: " + success);

        return success;
    }
}

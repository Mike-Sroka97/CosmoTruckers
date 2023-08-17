using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AttackUI : MonoBehaviour
{
    [SerializeField] int currentAttack = 0;
    public int GetCurrentAttack { get => currentAttack; }

    float speed = 50f;
    [SerializeField] float speedIncrease = 100f;
    [SerializeField] float baseSpeed = 50;

    //All these variables will need to pull from save data at some point to see how many attacks the player has
    const float radius = 40f;
    int numberOfAttacks = 10;
    float rotationDistance;

    bool spinning = false;
    [SerializeField] RectTransform[] children;
    PlayerCharacter currentPlayer;

    public void StartTurn(PlayerCharacter player)
    {
        speed = baseSpeed;
        currentPlayer = player;
        numberOfAttacks = player.GetAllAttacks.Count;

        float angle = 0f;
        rotationDistance = 360f / 10;
        float x;
        float y;

        angle += rotationDistance;

        for (int i = 0; i < 10; i++)
        {
            children[i].gameObject.SetActive(true);
            if (i < numberOfAttacks)
                children[i].gameObject.GetComponent<TMP_Text>().text = player.GetAllAttacks[i].AttackName;
            else
                children[i].gameObject.GetComponent<TMP_Text>().text = "";

            x = radius * Mathf.Cos(angle * Mathf.Deg2Rad);
            y = radius * Mathf.Sin(angle * Mathf.Deg2Rad);
            children[i].transform.localPosition = new Vector3(x, y, 0);
            angle -= rotationDistance;
        }

        SetOpasity(0);
    }

    private void OnDisable()
    {
        for (int i = 0; i < children.Length; i++)
            children[i].gameObject.SetActive(false);


        currentAttack = 0;
    }
    private void Update()
    {
        GetInput();
    }

    private void GetInput()
    {
        if (!spinning)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                RotateWheel(-rotationDistance);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                RotateWheel(rotationDistance);
            }
            else if(Input.GetKeyDown(KeyCode.Space))
            {
                if(currentPlayer.GetAllAttacks[currentAttack].CanUse)
                    StartAttack();
            }
        }
    }

    void StartAttack()
    {
        int hold = currentAttack;

        currentPlayer.EndTurn();

        FindObjectOfType<CombatManager>().StartCombat(currentPlayer.GetAllAttacks[hold], currentPlayer);
    }

    private void RotateWheel(float rotationValue)
    {
        StartCoroutine(SpinWheel(rotationValue));
    }
    IEnumerator SpinWheel(float rotationValue)
    {
        spinning = true;
        
        float currentDegree = 0;
        bool negative = rotationValue < 0;
        Vector3 currentRotation = transform.eulerAngles;

        while(currentDegree < MathF.Abs(rotationValue))
        {
            if(negative)
            {
                transform.Rotate(0, 0, -Time.deltaTime * speed);
            }
            else
            {
                transform.Rotate(0, 0, Time.deltaTime * speed);
            }

            foreach (RectTransform child in children)
            {
                if (!child.GetComponent<AttackUI>())
                {
                    child.rotation = Quaternion.identity;
                }
            }

            currentDegree += Time.deltaTime * speed;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, currentRotation.z + rotationValue);
        foreach (RectTransform child in children)
        {
            if (!child.GetComponent<AttackUI>())
            {
                child.rotation = Quaternion.identity;
            }
        }

        currentAttack = (int)(Mathf.Round(transform.eulerAngles.z) / (360 / 10));

        SetOpasity(currentAttack);

        if (String.IsNullOrEmpty(children[currentAttack].gameObject.GetComponent<TMP_Text>().text))
        {
            speed = speedIncrease;
            RotateWheel(rotationValue);
        }
        else
        {
            speed = baseSpeed;
            spinning = false;
        }
    }

    void SetOpasity(int attack)
    {
        for (int i = 0; i < 10; i++)
            children[i].gameObject.GetComponent<TMP_Text>().color = new Color(1, 1, 1, .25f);

        if (currentPlayer.GetAllAttacks.Count > currentAttack && currentPlayer.GetAllAttacks[currentAttack].CanUse)
            children[currentAttack].gameObject.GetComponent<TMP_Text>().color = new Color(1, 1, 1, 1);
        else
            children[currentAttack].gameObject.GetComponent<TMP_Text>().color = new Color(1, 0, 0, 1);

        if (currentAttack == 0)
        {
            children[currentAttack + 1].gameObject.GetComponent<TMP_Text>().color = new Color(1, 1, 1, .50f);
            children[numberOfAttacks].gameObject.GetComponent<TMP_Text>().color = new Color(1, 1, 1, .50f);
        }
        else if(currentAttack == numberOfAttacks)
        {
            children[0].gameObject.GetComponent<TMP_Text>().color = new Color(1, 1, 1, .70f);
            children[numberOfAttacks - 1].gameObject.GetComponent<TMP_Text>().color = new Color(1, 1, 1, .50f);
        }
        else
        {
            children[currentAttack + 1].gameObject.GetComponent<TMP_Text>().color = new Color(1, 1, 1, .50f);
            children[currentAttack - 1].gameObject.GetComponent<TMP_Text>().color = new Color(1, 1, 1, .50f);
        }
    }
}

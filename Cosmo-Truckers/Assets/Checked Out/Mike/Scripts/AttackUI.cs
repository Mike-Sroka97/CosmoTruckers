using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AttackUI : MonoBehaviour
{
    [SerializeField] int currentAttack = 0;
    public int GetCurrentAttack { get => currentAttack; }

    [SerializeField] float speed = 5f;

    //All these variables will need to pull from save data at some point to see how many attacks the player has
    const float radius = 40f;
    int numberOfAttacks = 16;
    float rotationDistance;

    bool spinning = false;
    [SerializeField] RectTransform[] children;
    PlayerCharacter currentPlayer;

    public void StartTurn(PlayerCharacter player)
    {
        currentPlayer = player;
        numberOfAttacks = player.GetAllAttacks.Length;

        float angle = 0f;
        rotationDistance = 360f / numberOfAttacks;
        float x;
        float y;

        for (int i = 0; i < numberOfAttacks; i++)
        {
            children[i].gameObject.SetActive(true);
            children[i].gameObject.GetComponent<TMP_Text>().text = player.GetAllAttacks[i].AttackName;

            x = radius * Mathf.Cos(angle * Mathf.Deg2Rad);
            y = radius * Mathf.Sin(angle * Mathf.Deg2Rad);
            children[i].transform.localPosition = new Vector3(x, y, 0);
            angle -= rotationDistance;
        }
    }

    //private void OnEnable()
    //{        
    //    float angle = 0f;
    //    rotationDistance = 360f / numberOfAttacks;
    //    float x;
    //    float y;
    //    for (int i = 0; i < numberOfAttacks; i++)
    //        children[i].gameObject.SetActive(true);

    //    for (int i = 0; i < numberOfAttacks; i++)
    //    {
    //        x = radius * Mathf.Cos(angle * Mathf.Deg2Rad);
    //        y = radius * Mathf.Sin(angle * Mathf.Deg2Rad);
    //        children[i].transform.localPosition = new Vector3(x, y, 0);
    //        angle += rotationDistance;
    //    }
    //}
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
                StartAttack();
            }
        }
    }

    void StartAttack()
    {
        currentPlayer.EndTurn();

        FindObjectOfType<CombatManager>().StartCombat(currentPlayer.GetAllAttacks[currentAttack]);
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
        spinning = false;

        currentAttack = (int)(Mathf.Round(transform.eulerAngles.z) / (360 / numberOfAttacks));
    }
}

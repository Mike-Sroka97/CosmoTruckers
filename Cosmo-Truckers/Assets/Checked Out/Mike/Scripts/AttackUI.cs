using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AttackUI : MonoBehaviour
{
    [SerializeField] protected int currentAttack = 0;
    public int GetCurrentAttack { get => currentAttack; }

    float speed = 500f;
    [SerializeField] float speedIncrease = 100f;
    [SerializeField] float baseSpeed = 50;
    [SerializeField] protected PlayerCharacter myCharacter;

    //All these variables will need to pull from save data at some point to see how many attacks the player has
    const float radius = 40f;
    int numberOfAttacks = 10;
    protected float rotationDistance;

    protected bool spinning = false;
    [SerializeField] List<RectTransform> children;
    protected PlayerCharacter currentPlayer;

    public void StartTurn(PlayerCharacter player)
    {
        currentAttack = 0;
        transform.eulerAngles = Vector3.zero;

        speed = baseSpeed;
        currentPlayer = player;
        numberOfAttacks = player.GetAllAttacks.Count;
        player.GetComponent<Mana>().CheckCastableSpells();

        float angle = 0f;
        rotationDistance = 360f / 10;
        float x;
        float y;

        angle += rotationDistance;

        for (int i = 0; i < myCharacter.GetAllAttacks.Count; i++)
        {
            children[i].gameObject.SetActive(true);
            children[i].rotation = Quaternion.identity;
            if (i < numberOfAttacks)
                children[i].gameObject.GetComponent<TMP_Text>().text = player.GetAllAttacks[i].AttackName;

            x = radius * Mathf.Cos(angle * Mathf.Deg2Rad);
            y = radius * Mathf.Sin(angle * Mathf.Deg2Rad);
            children[i].transform.localPosition = new Vector3(x, y, 0);
            angle -= rotationDistance;
        }

        SetOpacity(0);
    }

    private void OnDisable()
    {
        for (int i = 0; i < children.Count; i++)
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
                if(children[currentAttack].gameObject.activeSelf && currentPlayer.GetAllAttacks[currentAttack].CanUse)
                    StartAttack();
            }
        }
    }

    protected virtual void StartAttack()
    {
        int hold = currentAttack;

        currentPlayer.EndTurn();

        CombatManager.Instance.StartCombat(currentPlayer.GetAllAttacks[hold], currentPlayer);
    }

    protected void RotateWheel(float rotationValue)
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

        SetOpacity(currentAttack);

        if (!children[currentAttack].gameObject.activeSelf || !currentPlayer.GetAllAttacks[currentAttack].CanUse)
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

    void SetOpacity(int attack)
    {
        for(int i = 0; i < children.Count; i++)
        {
            if(i == attack)
            {
                children[i].gameObject.GetComponent<TMP_Text>().color = new Color(0, 0, 0, 1);
            }
            else if((attack == 0 && i == children.Count - 1) || (attack == children.Count - 1 && i == 0))
            {
                children[i].gameObject.GetComponent<TMP_Text>().color = new Color(0, 0, 0, .5f);
            }
            else if(i == attack - 1 || i == attack + 1)
            {
                children[i].gameObject.GetComponent<TMP_Text>().color = new Color(0, 0, 0, .5f);
            }
            else
            {
                children[i].gameObject.GetComponent<TMP_Text>().color = new Color(0, 0, 0, .25f);
            }
        }

        SetColor();
    }

    private void SetColor()
    {
        for (int i = 0; i < numberOfAttacks; i++)
            if (currentPlayer.GetAllAttacks[i].CanUse)
                children[i].gameObject.GetComponent<TMP_Text>().color = new Color(1, 1, 1, children[i].gameObject.GetComponent<TMP_Text>().color.a);
            else
                children[i].gameObject.GetComponent<TMP_Text>().color = new Color(1, 0, 0, children[i].gameObject.GetComponent<TMP_Text>().color.a);
    }
}

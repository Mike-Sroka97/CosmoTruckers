using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class AttackUI : MonoBehaviour
{
    [SerializeField] protected int currentAttack = 0;
    public int GetCurrentAttack { get => currentAttack; }

    float speed = 500f;
    const float speedIncrease = 200;
    const float baseSpeed = 200;
    protected PlayerCharacter myCharacter;
    private bool attackDescriptionActive = false;

    //All these variables will need to pull from save data at some point to see how many attacks the player has
    const float radius = 40f;
    int numberOfAttacks = 10;
    protected float rotationDistance;

    protected bool spinning = false;
    protected PlayerCharacter currentPlayer;
    Transform heheHahaCircle;
    public UnityEvent AttackSelected = new UnityEvent();

    public void StartTurn(PlayerCharacter player)
    {
        heheHahaCircle = transform.parent.Find("heheHahaCircle");
        myCharacter = player;

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

        for (int i = 0; i < player.GetAllAttacks.Count; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
            transform.GetChild(i).rotation = Quaternion.identity;
            if (i < numberOfAttacks)
                transform.GetChild(i).gameObject.GetComponent<TMP_Text>().text = player.GetAllAttacks[i].AttackName;

            x = radius * Mathf.Cos(angle * Mathf.Deg2Rad);
            y = radius * Mathf.Sin(angle * Mathf.Deg2Rad);
            transform.GetChild(i).transform.localPosition = new Vector3(x, y, 0);
            angle -= rotationDistance;
        }

        SetOpacity(0);
    }

    public abstract void HandleMana();

    private void OnEnable()
    {
        attackDescriptionActive = false;
    }

    private void OnDisable()
    {
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(false);
    }
    private void Update()
    {
        GetInput();
    }

    private void GetInput()
    {
        if (!spinning && !CombatManager.Instance.GetCurrentCharacter.GetComponent<PlayerCharacter>().RevokeControls)
        {
            if(Input.GetKeyDown(KeyCode.E) && !attackDescriptionActive)
            {
                attackDescriptionActive = true;
                CombatManager.Instance.AttackDescription.gameObject.SetActive(true);
                UpdateAttackDescription();
            }
            else if(Input.GetKeyDown(KeyCode.E) && attackDescriptionActive)
            {
                attackDescriptionActive = false;
                CombatManager.Instance.AttackDescription.gameObject.SetActive(false);
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                RotateWheel(-rotationDistance);
                if (attackDescriptionActive)
                    UpdateAttackDescription();
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                RotateWheel(rotationDistance);
            }
            else if(Input.GetKeyDown(KeyCode.Space))
            {
                attackDescriptionActive = false;
                CombatManager.Instance.AttackDescription.gameObject.SetActive(false);

                if (transform.GetChild(currentAttack).gameObject.activeSelf && currentPlayer.GetAllAttacks[currentAttack].CanUse)
                {
                    AttackSelected.Invoke();
                    StartCoroutine(StartAttack());
                }
            }
        }
    }

    private void UpdateAttackDescription()
    {
        CombatManager.Instance.AttackDescription.MyAttackDescription.text = currentPlayer.GetAllAttacks[currentAttack].AttackDescription;
        CombatManager.Instance.AttackDescription.MyAttackName.text = currentPlayer.GetAllAttacks[currentAttack].AttackName;
        CombatManager.Instance.AttackDescription.UpdateCost(currentPlayer.GetAllAttacks[currentAttack].CostTitle, currentPlayer.GetAllAttacks[currentAttack].CostDescription); 
        CombatManager.Instance.AttackDescription.UpdateTargetType(currentPlayer.GetAllAttacks[currentAttack].TargetingType.ToString());
        CombatManager.Instance.AttackDescription.MyVideoPlayer.clip = currentPlayer.GetAllAttacks[currentAttack].MinigameDemo;
        CombatManager.Instance.AttackDescription.MyVideoPlayer.frame = 0;
    }

    IEnumerator StartAttack()
    {
        yield return null; //Lol fuck my life

        currentPlayer.EndTurn();

        CombatManager.Instance.StartCombat(currentPlayer.GetAllAttacks[currentAttack], currentPlayer);
    }

    protected void RotateWheel(float rotationValue)
    {
        CombatManager.Instance.AttackDescription.Static.SetActive(true);
        CombatManager.Instance.AttackDescription.Screen.SetActive(false);

        float tempRotation = rotationValue;

        currentAttack = (int)(Mathf.Round(transform.eulerAngles.z + rotationValue) / (360 / 10));

        if (currentAttack == -1)
            currentAttack = transform.childCount - 1;
        else if (currentAttack == transform.childCount)
            currentAttack = 0;

        while (!transform.GetChild(currentAttack).gameObject.activeSelf)
        {
            speed = speedIncrease;
            tempRotation += rotationValue;
            currentAttack = (int)(Mathf.Round(transform.eulerAngles.z + tempRotation) / (360 / 10));

            if (currentAttack < 0)
                currentAttack = transform.childCount + currentAttack;
            else if (currentAttack == transform.childCount)
                currentAttack = 0;
        }

        SetOpacity(currentAttack);

        StartCoroutine(SpinWheel(tempRotation));
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
                heheHahaCircle.Rotate(0, 0, -Time.deltaTime * speed);
            }
            else
            {
                transform.Rotate(0, 0, Time.deltaTime * speed);
                heheHahaCircle.Rotate(0, 0, Time.deltaTime * speed);
            }

            foreach (RectTransform child in transform)
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
        foreach (RectTransform child in transform)
        {
            if (!child.GetComponent<AttackUI>())
            {
                child.rotation = Quaternion.identity;
            }
        }

        speed = baseSpeed;
        spinning = false;

        CombatManager.Instance.AttackDescription.MyVideoPlayer.targetTexture.Release();
        CombatManager.Instance.AttackDescription.Screen.SetActive(true);
        CombatManager.Instance.AttackDescription.MyVideoPlayer.targetTexture.Create();
        CombatManager.Instance.AttackDescription.Static.SetActive(false);

        UpdateAttackDescription();
    }

    void SetOpacity(int attack)
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            if(i == attack)
            {
                transform.GetChild(i).gameObject.GetComponent<TMP_Text>().color = new Color(0, 0, 0, 1);
            }
            else if(i == attack - 1 || i == attack + 1)
            {
                transform.GetChild(i).gameObject.GetComponent<TMP_Text>().color = new Color(0, 0, 0, .5f);
            }
            else
            {
                transform.GetChild(i).gameObject.GetComponent<TMP_Text>().color = new Color(0, 0, 0, 0.3f);
            }
        }

        SetColor();
    }

    private void SetColor()
    {
        for (int i = 0; i < numberOfAttacks; i++)
            if (currentPlayer.GetAllAttacks[i].CanUse)
                transform.GetChild(i).gameObject.GetComponent<TMP_Text>().color = new Color(1, 1, 1, transform.GetChild(i).gameObject.GetComponent<TMP_Text>().color.a);
            else
                transform.GetChild(i).gameObject.GetComponent<TMP_Text>().color = new Color(1, 0, 0, transform.GetChild(i).gameObject.GetComponent<TMP_Text>().color.a);
    }
}

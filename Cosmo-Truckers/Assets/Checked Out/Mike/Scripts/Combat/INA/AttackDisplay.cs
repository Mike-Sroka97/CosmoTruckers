using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AttackDisplay : MonoBehaviour
{
    [Header("Player Lights")]
    [SerializeField] SpriteRenderer[] playerLights;
    [SerializeField] Color[] playerColors;
    [SerializeField] Color offColor;

    [Space(20)]
    [Header("Movement Variables")]
    [SerializeField] float rotateSpeed = 2.2f;
    [SerializeField] int numberOfFlashes = 3;
    [SerializeField] float onTime = 0.9f;
    [SerializeField] float offTime = 0.3f;

    bool opened = false;
    TextMeshProUGUI attackText;

    private void Start()
    {
        attackText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetAttack(string attack, List<PlayerCharacter> activePlayers)
    {
        attackText.text = attack;
        SetLights(activePlayers);
        StartCoroutine(RotateMe(true));
    }

    public void SetEnemyIntentions(string attack)
    {
        attackText.text = attack;

        if(!opened)
            StartCoroutine(FlipOpen());
    }

    private void SetLights(List<PlayerCharacter> activePlayers)
    {
        foreach (SpriteRenderer light in playerLights)
            light.color = offColor;
        foreach (PlayerCharacter player in activePlayers)
            if(player)
                playerLights[player.PlayerNumber - 1].color = playerColors[player.PlayerNumber - 1];
    }

    private IEnumerator RotateMe(bool activate)
    {
        if(activate)
        {
            while (transform.localEulerAngles.x < 359)
            {
                transform.Rotate(new Vector3(rotateSpeed * Time.deltaTime, 0, 0));

                if (transform.localEulerAngles.x < 100)
                    break;

                yield return null;
            }

            transform.localEulerAngles = Vector3.zero;
            StartCoroutine(FlashMove());
        }
        else
        {
            transform.localEulerAngles = new Vector3(359, 0, 0);

            while (transform.localEulerAngles.x > 270)
            {
                transform.Rotate(new Vector3(-rotateSpeed * Time.deltaTime, 0, 0));
                yield return null;
            }

            transform.localEulerAngles = new Vector3(270, 0, 0);

            CombatManager.Instance.PauseAttack = false;
        }
    }

    private IEnumerator FlashMove()
    {
        int currentFlashes = 0;

        while(currentFlashes < numberOfFlashes)
        {
            attackText.enabled = true;
            yield return new WaitForSeconds(onTime);
            attackText.enabled = false;
            yield return new WaitForSeconds(offTime);

            currentFlashes++;
        }

        StartCoroutine(RotateMe(false));
    }

    private IEnumerator FlipOpen()
    {
        opened = true;

        while (transform.localEulerAngles.x < 359)
        {
            transform.Rotate(new Vector3(rotateSpeed * Time.deltaTime, 0, 0));

            if (transform.localEulerAngles.x < 100)
                break;

            yield return null;
        }

        transform.localEulerAngles = Vector3.zero;

        StartCoroutine(FlashIndefinetly());
    }

    private IEnumerator FlashIndefinetly()
    {
        while (opened)
        {
            attackText.enabled = true;
            yield return new WaitForSeconds(onTime);
            attackText.enabled = false;
            yield return new WaitForSeconds(offTime);
        }
    }

    public void StartClose()
    {
        StopAllCoroutines();
        StartCoroutine(FlipClose());
    }

    private IEnumerator FlipClose()
    {
        opened = false;

        transform.eulerAngles = new Vector3(359, 0, 0);

        while (transform.rotation.x < .75)
        {
            transform.Rotate(new Vector3(-rotateSpeed * Time.deltaTime, 0, 0));
            yield return null;
        }

        transform.localEulerAngles = new Vector3(270, 0, 0);
    }
}

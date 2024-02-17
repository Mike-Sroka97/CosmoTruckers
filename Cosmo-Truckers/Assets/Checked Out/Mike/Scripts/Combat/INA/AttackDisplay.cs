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

    //TODO Lights
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

    private void SetLights(List<PlayerCharacter> activePlayers)
    {
        foreach (SpriteRenderer light in playerLights)
            light.color = offColor;
        foreach (PlayerCharacter player in activePlayers)
            playerLights[player.PlayerNumber - 1].color = playerColors[player.PlayerNumber - 1];
    }

    private IEnumerator RotateMe(bool activate)
    {
        if(activate)
        {
            while (transform.eulerAngles.x < 359)
            {
                transform.Rotate(new Vector3(rotateSpeed * Time.deltaTime, 0, 0));

                if (transform.eulerAngles.x < 100)
                    break;

                yield return null;
            }

            transform.eulerAngles = Vector3.zero;
            StartCoroutine(FlashMove());
        }
        else
        {
            transform.eulerAngles = new Vector3(359, 0, 0);

            while (transform.eulerAngles.x > 270)
            {
                transform.Rotate(new Vector3(-rotateSpeed * Time.deltaTime, 0, 0));
                yield return null;
            }

            transform.eulerAngles = new Vector3(270, 0, 0);

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
}

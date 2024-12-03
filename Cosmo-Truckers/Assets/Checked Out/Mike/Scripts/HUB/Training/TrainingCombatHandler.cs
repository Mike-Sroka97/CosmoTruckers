using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TrainingCombatHandler : MonoBehaviour
{
    [SerializeField] TMP_Text Timer;

    [HideInInspector] public bool PauseCoroutine;

    GameObject miniGame;
    BaseAttackSO attack;
    InaPractice INA;
    GameObject currentPlayer;

    private void Start()
    {
        INA = GetComponent<InaPractice>();
    }

    public void PopulateMinigameData()
    {
        miniGame = INA.CurrentAttack.CombatPrefab;
        attack = INA.CurrentAttack;

        StartCoroutine(StartMiniGame());
    }

    private IEnumerator StartMiniGame()
    {
        if (!attack.AutoCast)
        {
            StartCoroutine(INA.PrePreMinigameStuffs());

            //PAUSE
            while (PauseCoroutine)
                yield return null;

            miniGame = Instantiate(attack.CombatPrefab, INA.transform);

            float miniGameTime = miniGame.GetComponent<CombatMove>().MinigameDuration;

            if (attack.BossMove)
                Timer.text = "";
            else
                Timer.text = miniGameTime.ToString();

            StartCoroutine(INA.PreMinigameStuffs());

            //PAUSE
            while (PauseCoroutine)
                yield return null;

            //Attack SO Start
            foreach (Player player in FindObjectsOfType<Player>())
            {
                player.enabled = true;
            }

            attack.StartCombat();

            //Boss move handler. Does not track time. Tracks Fight won and players dead
            if (attack.BossMove)
            {
                while (!miniGame.GetComponentInChildren<CombatMove>().FightWon && !miniGame.GetComponentInChildren<CombatMove>().PlayerDead && !miniGame.GetComponentInChildren<CombatMove>().MoveEnded)
                {
                    yield return null;
                }
            }

            //Timer and end move handler for non-boss moves
            else
            {
                while (miniGameTime >= 0 && !miniGame.GetComponentInChildren<CombatMove>().PlayerDead && !miniGame.GetComponentInChildren<CombatMove>().MoveEnded)
                {
                    miniGameTime -= Time.deltaTime;
                    Timer.text = ((int)miniGameTime).ToString();

                    yield return null;
                }
            }

            Timer.text = "";

            //ina practice cleanup method
            StartCoroutine(INA.MinigameCleanup());
        }
    }

    public void SpawnPlayers(PlayerCharacter playerToSpawn)
    {
        GameObject character = Instantiate(playerToSpawn.GetCharacterController, FindObjectOfType<CombatMove>().transform);
        currentPlayer = character;
        character.GetComponent<Player>().MoveSpeed = character.GetComponent<Player>().MoveSpeed * playerToSpawn.GetComponent<CharacterStats>().Speed * .01f; //adjusts speed
        character.GetComponent<Player>().enabled = false;
        character.GetComponent<Player>().MyCharacter = playerToSpawn;
    }

    public void CleanupMinigame()
    {
        //Clean up INA
        Destroy(currentPlayer);

        miniGame.gameObject.SetActive(false);
    }
}

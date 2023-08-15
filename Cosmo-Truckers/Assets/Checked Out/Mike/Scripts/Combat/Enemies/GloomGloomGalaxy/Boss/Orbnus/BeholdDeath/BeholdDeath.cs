using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeholdDeath : CombatMove
{
    [SerializeField] Transform[] meteoriteSpawns;
    [SerializeField] float meteoriteDelay;
    [SerializeField] GameObject meteorite;

    Player[] players;

    private void Start()
    {
        StartMove();
        GenerateLayout();

        players = FindObjectsOfType<Player>();

        StartCoroutine(SpawnMeteorites());
    }

    IEnumerator SpawnMeteorites()
    {
        yield return new WaitForSeconds(meteoriteDelay);

        foreach(Player player in players)
        {
            int random = Random.Range(0, meteoriteSpawns.Length);

            GameObject newMeteorite = Instantiate(meteorite, meteoriteSpawns[random]);
            newMeteorite.transform.right = player.transform.position - newMeteorite.transform.position;
        }

        StartCoroutine(SpawnMeteorites());
    }
}

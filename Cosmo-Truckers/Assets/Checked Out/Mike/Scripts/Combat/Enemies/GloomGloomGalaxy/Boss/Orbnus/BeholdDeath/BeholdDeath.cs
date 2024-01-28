using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeholdDeath : CombatMove
{
    [SerializeField] Transform[] meteoriteSpawns;
    [SerializeField] float meteoriteDelay;
    [SerializeField] GameObject meteorite;

    private void Start()
    {
        GenerateLayout();
    }

    public override void StartMove()
    {
        GetComponentInChildren<BeholdDeathDross>().enabled = true;
        GetComponentInChildren<BeholdDeathDross>().transform.GetComponent<MoveForward>().enabled = true;
        foreach (EyeFollower eye in GetComponentsInChildren<EyeFollower>())
            eye.enabled = true;
        foreach (Follower follower in GetComponentsInChildren<Follower>())
            follower.enabled = true;

        SetupMultiplayer();
        StartCoroutine(SpawnMeteorites());
        base.StartMove();
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CombatMove : MonoBehaviour
{
    [SerializeField] TargetType TypeOfAttack;
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] protected GameObject[] layouts;
    //For games that can deal player damage
    [SerializeField] protected int Damage;
    [SerializeField] protected DebuffStackSO DebuffToAdd;
    public float MinigameDuration;

    [HideInInspector] public int Score;
    [HideInInspector] public bool PlayerDead = false;
    [HideInInspector] public bool MoveEnded = false;
    [HideInInspector] public int Hits = 0;

    protected float currentTime = 0;

    private void Start()
    {
        StartMove();
    }

    public enum TargetType
    {
        NoTarget,
        SelfTarget,
        SingleTarget,
        MultiTarget,
        Cone,
        MultiTargetChoice,
        AOE,
        AllTarget
    }

    protected void StartMove()
    {
        if (spawnPoints.Length == 0) return; //TEMP

        Player[] players = FindObjectsOfType<Player>();

        if(players.Length <= 1)
        {
            players[0].transform.position = spawnPoints[0].position;
        }
        else
        {
            //set each alive player to a different spawn
        }
    }
    public virtual void EndMove()
    {
        Debug.Log(Score);
        MoveEnded = true;

        if (CombatManager.Instance != null) //In the combat screen
        {
            foreach (Character character in CombatManager.Instance.GetCharactersSelected)
            {
                //TODO split damage and AUG calls?
                character.GetComponent<Character>().TakeDamage(Damage * Hits);

                if (DebuffToAdd != null)
                    for (int i = 0; i < Hits; i++)
                        character.GetComponent<Character>().AddDebuffStack(DebuffToAdd);
            }
        }
        else //Running tests
        {
            Debug.Log($"{Damage * Hits} done to player");
            if (DebuffToAdd != null) Debug.Log($"{Hits} stacks of {DebuffToAdd.DebuffName} added");
        }
    }
    private void ApplyAugments()
    {

    }

    protected void GenerateLayout()
    {
        if(layouts.Length > 0)
        {
            int random = UnityEngine.Random.Range(0, layouts.Length);
            Instantiate(layouts[random], transform);
        }
    }

    protected virtual void TrackTime()
    {
        if (MoveEnded)
            return;

        currentTime += Time.deltaTime;

        if ((currentTime >= MinigameDuration || PlayerDead) && !MoveEnded)
        {
            EndMove();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerVesselManager : NetworkBehaviour
{
    [SerializeField] RectTransform[] vesselSpawns;
    [SerializeField] Transform lowerPos;
    [SerializeField] float moveSpeed;

    [HideInInspector] public static PlayerVesselManager Instance;
    [HideInInspector] public PlayerVessel[] PlayerVessels;

    Vector3 startPos;
    public bool IsMoving;

    private void Awake() => Instance = this;

    public void Initialize()
    {
        startPos = transform.position;

        for (int i = 0; i < EnemyManager.Instance.Players.Count; i++)
        {
            if(EnemyManager.Instance.Players[i].PlayerVessel != null)
            {
                PlayerVessel currentVessel = Instantiate(EnemyManager.Instance.Players[i].PlayerVessel, vesselSpawns[i]).GetComponent<PlayerVessel>();
                NetworkServer.Spawn(currentVessel.gameObject, NetworkTestManager.Instance.GetPlayers[i].gameObject);
                currentVessel.Initialize(EnemyManager.Instance.Players[i]);
            }
        }

        PlayerVessels = GetComponentsInChildren<PlayerVessel>();
    }


    public IEnumerator MoveMe(bool up)
    {
        IsMoving = true;

        if(up)
        {
            while(transform.position.y < startPos.y)
            {
                transform.position += new Vector3(0, moveSpeed * Time.deltaTime, 0);
                yield return null;
            }

             transform.position = startPos;
        }
        else
        {
            while (transform.position.y > lowerPos.position.y)
            {
                transform.position -= new Vector3(0, moveSpeed * Time.deltaTime, 0);
                yield return null;
            }

            transform.position = lowerPos.position;
        }

        IsMoving = false;
    }
}

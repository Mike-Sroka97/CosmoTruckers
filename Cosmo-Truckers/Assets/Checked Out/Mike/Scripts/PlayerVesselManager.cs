using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVesselManager : MonoBehaviour
{
    [SerializeField] RectTransform[] vesselSpawns;
    [SerializeField] Transform lowerPos;
    [SerializeField] float moveSpeed;

    [HideInInspector] public static PlayerVesselManager Instance;

    Vector3 startPos;

    private void Awake() => Instance = this;

    public void Initialize()
    {
        startPos = transform.position;

        for (int i = 0; i < EnemyManager.Instance.Players.Count; i++)
        {
            if(EnemyManager.Instance.Players[i].PlayerVessel != null)
            {
                PlayerVessel currentVessel = Instantiate(EnemyManager.Instance.Players[i].PlayerVessel, vesselSpawns[i]).GetComponent<PlayerVessel>();
                currentVessel.Initialize(EnemyManager.Instance.Players[i]);
            }
        }
    }

    public IEnumerator MoveMe(bool up)
    {
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
    }
}

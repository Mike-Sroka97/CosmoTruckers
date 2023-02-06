using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TruckDropItems : MonoBehaviour
{
    [Header("Items")]
    [SerializeField] protected Transform[] DropPoints;
    [SerializeField] GameObject[] ItemsToDrop;
    [SerializeField] Vector3 ItemSpeed;
    [SerializeField] Vector2 RotationSpeed;

    [Header("Timing")]
    [SerializeField] Vector2 DropTiming;

    [SerializeField] float DestoryTime;
    float NextTime;
    List<GameObject> toRemove;

    private void Start()
    {
        //Get first timing
        GetNewTime();

        toRemove = new List<GameObject>();
    }
    private void Update()
    {
        if (Time.time >= NextTime)
        {
            GetNewTime();

            GameObject dropped = Instantiate(ItemsToDrop[Random.Range(0, ItemsToDrop.Length)]);
            dropped.transform.position = DropPoints[Random.Range(0, DropPoints.Length)].position;
            toRemove.Add(dropped);
            StartCoroutine(RotateAndKill(dropped));
        }
    }


    IEnumerator RotateAndKill(GameObject item)
    {
        float time = DestoryTime + Time.time;
        while(Time.time < time)
        {
            item.transform.position += ItemSpeed * Time.deltaTime;
            item.transform.Rotate(Vector3.forward * Time.deltaTime * Random.Range(RotationSpeed.x, RotationSpeed.y));
            yield return null;
        }

        toRemove.Remove(item);
        Destroy(item);
    }

    float GetNewTime() => NextTime = Time.time + Random.Range(DropTiming.x, DropTiming.y);
    private void OnDisable()
    {
        foreach(var item in toRemove)
        {
            Destroy(item);
        }
    }
}


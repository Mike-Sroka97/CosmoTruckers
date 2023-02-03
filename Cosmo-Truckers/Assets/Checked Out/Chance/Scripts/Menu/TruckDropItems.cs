using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckDropItems : MonoBehaviour
{
    [Header("Items")]
    [SerializeField] Transform[] DropPoints;
    [SerializeField] GameObject[] ItemsToDrop;
    [SerializeField] Vector3 ItemSpeed;
    [SerializeField] float RotationSpeed;

    [Header("Timing")]
    [SerializeField] [Range(0.0f, 10.0f)] float MinTime;
    [SerializeField] [Range(0.0f, 10.0f)] float MaxTime;

    [SerializeField] float DestoryTime;
    float NextTime;

    private void Start()
    {
        //Get first timing
        GetNewTime();
    }
    private void Update()
    {
        if (Time.time >= NextTime)
        {
            GetNewTime();

            GameObject dropped = Instantiate(ItemsToDrop[Random.Range(0, ItemsToDrop.Length)]);
            dropped.transform.position = DropPoints[Random.Range(0, DropPoints.Length)].position;
            StartCoroutine(RotateAndKill(dropped));
        }
    }


    IEnumerator RotateAndKill(GameObject item)
    {
        float time = DestoryTime + Time.time;
        while(Time.time < time)
        {
            item.transform.position += ItemSpeed * Time.deltaTime;
            item.transform.Rotate(Vector3.forward * Time.deltaTime * RotationSpeed);
            yield return null;
        }

        Destroy(item);
    }
    float GetNewTime() => NextTime = Time.time + Random.Range(MinTime, MaxTime); 
}

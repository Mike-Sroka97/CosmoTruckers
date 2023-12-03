using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemShockTurret : MonoBehaviour
{
    [SerializeField] float rotateSpeed;
    [SerializeField] float fireDelay;
    [SerializeField] Transform barrel;
    [SerializeField] GameObject zap;

    bool initialized = false;
    float currentTime = 0;
    ProtoINA proto;
    public void Initialize()
    {
        proto = FindObjectOfType<ProtoINA>();
        initialized = true;
    }

    private void Update()
    {
        if (!initialized)
            return;

        RotateMe();
        TrackTime();
    }

    private void RotateMe()
    {
        Vector3 direction = proto.transform.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
    }

    private void TrackTime()
    {
        currentTime += Time.deltaTime;

        if(currentTime >= fireDelay)
        {
            currentTime = 0;
            GameObject zapTemp = Instantiate(zap, barrel);
            zapTemp.transform.parent = null;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullChargeNode : MonoBehaviour
{
    [SerializeField] FullChargeNode pairingNode;
    [SerializeField] FullChargeCore myCore;
    [SerializeField] float snapDistance;
    [SerializeField] float snapCooldown;
    [SerializeField] LineRenderer pairLine;
    [SerializeField] Color pairLineColor;
    [SerializeField] Sprite completedSprite; 
    [SerializeField] GameObject gate;


    FullCharge minigame;
    SpriteRenderer myRenderer; 
    bool connectionMade = false;
    bool nodeFull = false;
    bool trackTime = false;
    float currentTime;

    private void Start()
    {
        minigame = FindObjectOfType<FullCharge>();
        myRenderer = GetComponent<SpriteRenderer>(); 
        currentTime = snapCooldown;
    }

    private void Update()
    {
        CheckPair();
        CheckNode();
        TrackTime();
    }

    private void CheckPair()
    {
        if(nodeFull && pairingNode.nodeFull && !connectionMade)
        {
            connectionMade = true;
            minigame.AugmentScore++;
            minigame.CheckAugmentSuccess(); 

            pairLine.startColor = pairLineColor;
            pairLine.endColor = pairLineColor;

            myRenderer.sprite = completedSprite; 

            if(gate != null)
            {
                gate.SetActive(false);
            }
        }
    }

    private void CheckNode()
    {
        if (nodeFull || trackTime || connectionMade)
            return;

        if(Vector2.Distance(transform.position, myCore.transform.position) <= snapDistance && currentTime >= snapCooldown)
        {
            nodeFull = true;
            myCore.transform.position = transform.position;
            myCore.Following = false;
        }
    }

    private void TrackTime()
    {
        if (!trackTime)
            return;

        currentTime += Time.deltaTime;
        if(currentTime >= snapCooldown)
        {
            trackTime = false;
        }
    }

    public void SnapCD()
    {
        nodeFull = false;
        trackTime = true;
        currentTime = 0;
    }
}

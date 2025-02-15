using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterBopFace : MonoBehaviour
{
    [SerializeField] Sprite[] faces;
    [SerializeField] Transform scoreSpawn;
    [SerializeField] GameObject[] scoresToSpawn;
    [SerializeField] Vector2[] objectShakeValues;

    ObjectShaker myShaker; 
    SpriteRenderer myRenderer;
    CounterBop minigame; 

    private void Start()
    {
        myRenderer = GetComponent<SpriteRenderer>();
        minigame = FindObjectOfType<CounterBop>();
        myShaker = GetComponent<ObjectShaker>(); 
    }

    public void IncrementScore(int score)
    {
        myRenderer.sprite = faces[score];
        myShaker.SetNewValues(objectShakeValues[score].x, objectShakeValues[score].y); 
        Instantiate(scoresToSpawn[score], scoreSpawn.position, Quaternion.identity, minigame.transform);
    }
}

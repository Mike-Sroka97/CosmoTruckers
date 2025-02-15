using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterBopFace : MonoBehaviour
{
    [SerializeField] Sprite[] faces;
    [SerializeField] Transform scoreSpawn;
    [SerializeField] GameObject[] scoresToSpawn;

    SpriteRenderer myRenderer;
    CounterBop minigame; 

    private void Start()
    {
        myRenderer = GetComponent<SpriteRenderer>();
        minigame = FindObjectOfType<CounterBop>();
    }

    public void IncrementScore(int score)
    {
        myRenderer.sprite = faces[score];
        Instantiate(scoresToSpawn[score], scoreSpawn.position, Quaternion.identity, minigame.transform);
    }
}

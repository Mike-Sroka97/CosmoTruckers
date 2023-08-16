using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitanTerrorHand : MonoBehaviour
{
    [SerializeField] TitanTerrorHand otherHand;
    [SerializeField] float triggerY;
    [SerializeField] Transform shockWaveSpawn;
    [SerializeField] GameObject shockWave;
    [SerializeField] GameObject[] SpitfireLayouts;

    public bool canTrigger;
    Crusher otherHandCrusher;

    private void Start()
    {
        otherHandCrusher = otherHand.GetComponent<Crusher>();
    }

    private void Update()
    {
        CheckTriggerDistance();
    }

    private void CheckTriggerDistance()
    {
        if(transform.position.y < triggerY && canTrigger)
        {
            canTrigger = false;
            otherHand.canTrigger = true;
            StartCoroutine(otherHandCrusher.Crush());
            Instantiate(shockWave, shockWaveSpawn.position, shockWaveSpawn.rotation);
            int random = Random.Range(0, SpitfireLayouts.Length);
            Instantiate(SpitfireLayouts[random], shockWaveSpawn.position, shockWaveSpawn.rotation);
        }
    }
}

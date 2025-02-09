using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreakOutSpikeSpawner : MonoBehaviour
{
    [SerializeField] Transform[] spawns;
    [SerializeField] GameObject spikePrefab;
    [SerializeField] GameObject safeSpikePrefab;
    [SerializeField] float materialFlashDelay;
    [SerializeField] float spinDelay;
    [SerializeField] float[] rotations;
    [SerializeField] float fireDelay;
    [SerializeField] float newRoundDelay;
    [SerializeField] float rotationTimeModifier;
    [SerializeField] float rotationSpeed;
    [SerializeField] int flashTimes = 3; 

    List<FreakOutSpike> spikes = new List<FreakOutSpike>();
    FreakOutSpike nonActiveSpike;
    FreakOut minigame;

    private void Start()
    {
        minigame = GetComponentInParent<FreakOut>();
    }

    public void SpawnSpikes(bool[] activeSpikes)
    {
        for(int i = 0; i < spawns.Length; i++)
        {
            if(activeSpikes[i])
            {
                FreakOutSpike spike = Instantiate(spikePrefab, spawns[i]).GetComponent<FreakOutSpike>();
                spike.Initialize(); 
                spikes.Add(spike);
            }
            else
            {
                nonActiveSpike = Instantiate(safeSpikePrefab, spawns[i]).GetComponent<FreakOutSpike>();
                nonActiveSpike.Initialize(); 
                spikes.Add(nonActiveSpike);
            }
        }

        StartCoroutine(SpikeActivation());
    }

    IEnumerator SpikeActivation()
    {
        // Wait for the spikes to spin in
        yield return new WaitForSeconds(spinDelay);

        // F.U. buddy, F.U. 
        Vector3 scale = nonActiveSpike.transform.localScale;
        nonActiveSpike.GetComponent<Animator>().enabled = false;
        nonActiveSpike.transform.localScale = scale;

        // Flash the spikes
        for (int i = 0; i < flashTimes; i++)
        {
            if (i % 2 == 0)
            {
                nonActiveSpike.SetOutline(false);
            }
            else
            {
                nonActiveSpike.SetOutline(true);
            }

            yield return new WaitForSeconds(materialFlashDelay); 
        }

        nonActiveSpike.SetOutline(true);

        //handle spinning
        int random = UnityEngine.Random.Range(0, rotations.Length);

        float targetRotation = rotations[random];
        float rotationTime = Mathf.Abs(targetRotation / rotationTimeModifier);
        float elapsedTime = 0f;
        float startRotation = transform.rotation.eulerAngles.z;
        float endRotation = startRotation + targetRotation;

        while (elapsedTime < rotationTime && !(transform.rotation == Quaternion.Euler(0f, 0f, endRotation)))
        {
            float currentRotation = Mathf.Lerp(startRotation, endRotation, elapsedTime / rotationTime * rotationSpeed);
            transform.rotation = Quaternion.Euler(0f, 0f, currentRotation);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = Quaternion.Euler(0f, 0f, endRotation);

        yield return new WaitForSeconds(fireDelay);

        // F.U. buddy, F.U
        nonActiveSpike.GetComponent<Animator>().enabled = true; 
        
        foreach (FreakOutSpike spike in spikes)
        {
            spike.SetMoving();
        }

        yield return new WaitForSeconds(newRoundDelay);

        if(minigame.Score < 2 && !minigame.MoveEnded)
        {
            spikes.Clear();
            minigame.Score++;
            minigame.SpawnSpikes();
        }
        else if(!minigame.MoveEnded)
        {
            minigame.MoveEnded = true;
        }
    }
}

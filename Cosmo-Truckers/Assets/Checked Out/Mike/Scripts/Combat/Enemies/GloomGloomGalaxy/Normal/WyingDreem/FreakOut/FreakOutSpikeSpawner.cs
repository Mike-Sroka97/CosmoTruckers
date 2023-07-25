using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreakOutSpikeSpawner : MonoBehaviour
{
    [SerializeField] Transform[] spawns;
    [SerializeField] GameObject activeSpike;
    [SerializeField] GameObject nonActiveSpike;
    [SerializeField] float materialLossDelay;
    [SerializeField] float spinDelay;
    [SerializeField] float[] rotations;
    [SerializeField] float fireDelay;
    [SerializeField] float newRoundDelay;
    [SerializeField] float rotationTimeModifier;
    [SerializeField] float rotationSpeed;

    List<FreakOutSpike> spikes = new List<FreakOutSpike>();
    FreakOut minigame;

    private void Start()
    {
        minigame = GetComponentInParent<FreakOut>();
    }

    public void SpawnSpikes(bool[] activeSpikes)
    {
        for(int i = 0; i < spawns.Length; i++)
        {
            GameObject spike;

            if(activeSpikes[i])
            {
                spike = Instantiate(activeSpike, spawns[i]);
            }
            else
            {
                spike = Instantiate(nonActiveSpike, spawns[i]);
            }

            spikes.Add(spike.GetComponent<FreakOutSpike>());
        }

        StartCoroutine(SpikeActivation());
    }

    IEnumerator SpikeActivation()
    {
        yield return new WaitForSeconds(materialLossDelay + 1); // +1 for animation duration

        foreach(FreakOutSpike spike in spikes)
        {
            spike.RemoveOutline();
        }

        yield return new WaitForSeconds(spinDelay);

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
            minigame.EndMove();
        }
    }
}

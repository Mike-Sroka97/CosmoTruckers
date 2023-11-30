using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMaterial : MonoBehaviour
{
    List<Material> myMaterials;
    float myTime;

    void Update()
    {
        MaterialUpdater(); 
    }

    public void StartMaterialLerp()
    {
        GetMaterials(GetComponent<Character>().TargetingSprites);
        myTime = 0f;
    }

    public void StopMaterialLerp()
    {
        myMaterials.Clear(); 
    }

    void GetMaterials(SpriteRenderer[] myRenderers)
    {
        for (int i = 0; i < myRenderers.Length; i++)
        {
            myMaterials.Add(myRenderers[i].material); 
        }
    }

    void MaterialUpdater()
    {
        myTime += Time.deltaTime;

        foreach (Material _material in myMaterials)
        {
            _material.SetFloat("_MyTime", myTime); 
        }
    }

}
